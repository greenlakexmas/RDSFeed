using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GreenlakeChristmas.RDSFeed.Configuration;
using GreenlakeChristmas.RDSFeed.DataSources;

namespace GreenlakeChristmas.RDSFeed
{

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConcurrentQueue<string> generalQueue = new ConcurrentQueue<string>();
                ConcurrentQueue<string> immediateQueue = new ConcurrentQueue<string>();
                Dictionary<Priority, ConcurrentQueue<string>> queues =
                    new Dictionary<Priority, ConcurrentQueue<string>>
                        {{Priority.General, generalQueue}, {Priority.Immediate, immediateQueue}};

                RDSConfig rdsConfig = new RDSConfig();
                IEnumerable<IDataSource> dataSources = 
                    Program.GetDataSources(rdsConfig, queues);


                foreach (IDataSource dataSource in dataSources)
                {
                    object[] objParams = new object[] { dataSource };
                    Thread thread = new Thread(RunDataSource);
                    thread.Start(objParams);
                }

                long intervaltime = DateTime.Now.Ticks;

                while (Thread.CurrentThread.ThreadState == ThreadState.Running)
                {
                    string output = string.Empty;
                    if (!immediateQueue.IsEmpty)
                    {
                        if (!immediateQueue.TryDequeue(out output)) continue;
                    }
                    else
                    {
                        if (Program.GetMillisecondDifference(intervaltime) < rdsConfig.RefreshInterval) continue;
                        if (!generalQueue.TryDequeue(out output)) continue;
                        intervaltime = DateTime.Now.Ticks;
                    }
                    if (string.IsNullOrEmpty(output)) continue;

                    using (StreamWriter sw = new StreamWriter(rdsConfig.MergeFile, false))
                    {
                        sw.WriteLine(output);
                        sw.Close();
                        Console.WriteLine("RDS output --> {0}", output);
                    }

                }

            }
            catch (Exception x)
            {
                Console.WriteLine("Main Error [{0}]", x.Message);
            }
        }

        public static void RunDataSource(object objParams)
        {
            object[] objArray = (object[]) objParams;
            IDataSource dataSource = (IDataSource) objArray[0];
            ConcurrentQueue<string> mainQueue = dataSource.Queue;
            Console.WriteLine("Running {0} every {1}", dataSource.GetType(), dataSource.RefreshInterval);

            while(Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                string output = dataSource.GetText();
                if (string.IsNullOrEmpty(output)) continue;
                mainQueue.Enqueue(output);
                Thread.Sleep(dataSource.RefreshInterval);
            }
        }
        
        private static IEnumerable<IDataSource> GetDataSources(
            RDSConfig rdsConfig, 
            IDictionary<Priority, ConcurrentQueue<string>> queues)
        {
            HashSet<IDataSource> dataSources = new HashSet<IDataSource>();
            foreach (Source source in rdsConfig.Sources)
            {
                IDataSource dataSource =
                    (IDataSource)
                    Activator.CreateInstance(source.SourceType, source.Constructor.GetConstructorArgs());
                dataSource.RefreshInterval = source.RefreshInterval;
                dataSource.Priority = source.Priority;
                dataSource.Queue = queues[dataSource.Priority];
                dataSource.Options = source.Options;
                dataSource.TemplateRotation = source.TemplateRotation;

                foreach (Template template in source.Templates)
                {
                    dataSource.Add(template);
                }
                dataSources.Add(dataSource);
            }
            return dataSources;
        } 

        private static long GetMillisecondDifference(long fromMilliseconds)
        {
            return (DateTime.Now.Ticks - fromMilliseconds)/10000;
        }
    }

}
