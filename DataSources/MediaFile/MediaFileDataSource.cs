using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources.MediaFile
{
    public class MediaFileDataSource : IDataSource
    {
        private string mediaFilePath;
        private Template template;
        private string currentSong;
        private bool isChanged = false;

        public MediaFileDataSource(string mediafilepath)
        {
            this.mediaFilePath = mediafilepath;
            this.RefreshInterval = 5000;
        }

        public ContentType ContentType
        {
            get { return ContentType.MediaFile; }
        }

        public Priority Priority { get; set; }

        public ConcurrentQueue<string> Queue { get; set; }

        public string GetRDSText()
        {
            string output = string.Empty;
            MethodInfo methodInfo = this.GetType().GetMethod(this.template.MethodName);
            object[] values = (object[])methodInfo.Invoke(this, null);
            return string.Format(template.Text, values);
        }

        public string[] GetSongTitle()
        {
            string song = string.Empty;
            if (File.Exists(this.mediaFilePath))
            {
                using (StreamReader sr = new StreamReader(File.Open(this.mediaFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    song = sr.ReadLine();
                    isChanged = (currentSong != song);
                    if (isChanged)
                    {
                        currentSong = song;
                    }
                    sr.Close();
                }
            }
            return new string[] {(isChanged ? song : string.Empty)};
        }

        public int RefreshInterval { get; set; }
        public void Add(Template template)
        {
            this.template = template;
        }
    }
}