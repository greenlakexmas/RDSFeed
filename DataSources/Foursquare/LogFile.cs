using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using GreenlakeChristmas.FourSquare.Objects;

namespace GreenlakeChristmas.RDSFeed.DataSources.Foursquare
{
    public class LogFile
    {

        public LogFile(string filePath)
        {
            this.FilePath = filePath;
            if (!File.Exists(this.FilePath))
            {
                Console.WriteLine("{0} created", this.FilePath);
                File.Create(this.FilePath);
            }
            using (StreamReader sr = new StreamReader(this.FilePath))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    LogRecord logRecord = new LogRecord(line);
                    this.logRecords.Add(logRecord);
                }
            }
        }

        public string FilePath { get; protected set; }

        public void Append(List<Checkin> checkins)
        {
            this.Append(checkins, string.Empty);
        }

        public void Append(List<Checkin> checkins, string exclusiveKey)
        {
            if (checkins.Count == 0)
            {
                //Console.WriteLine("No checkin records found.");
                return;
            }
            using (StreamWriter w = File.AppendText(this.FilePath))
            {
                foreach(Checkin checkin in checkins)
                {
                    LogRecord logRecord = new LogRecord(checkin);
                    bool add = true;
                    string value = string.Empty;
                    if (!string.IsNullOrEmpty(exclusiveKey))
                    {
                        value = logRecord.GetValue(exclusiveKey);
                        add = !this.HasRecord(exclusiveKey, value);
                    }
                    if (!add)
                    {
                        //Console.WriteLine("Record with key {0} and value {1} already exists.", exclusiveKey, value);
                        continue;
                    }
                    this.logRecords.Add(logRecord);
                    Console.WriteLine("LogRecord added:");
                    Console.WriteLine(logRecord.ToString());
                    w.WriteLine(logRecord.ToString());
                }
                // Close the writer and underlying file.
                w.Close();
            }
        }

        private List<LogRecord> logRecords = new List<LogRecord>();
        public LogRecord[] LogRecords
        {
            get { return this.logRecords.ToArray(); }
        } 

        public bool HasRecord(string key, string value)
        {
            return (this.logRecords.Any(lr => lr.GetValue(key) == value));
        }

        public LogRecord GetRandomRecord()
        {
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] randbytes = new byte[4];
            csp.GetBytes(randbytes);
            int seed = BitConverter.ToInt32(randbytes, 0);

            Random random = new Random(seed);
            int index = random.Next(0, this.LogRecords.Length);
            if (index > this.LogRecords.Length) index = this.LogRecords.Length - 1;
            return this.LogRecords[index];
        }

    }
}