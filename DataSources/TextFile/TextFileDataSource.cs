using System.IO;

namespace GreenlakeChristmas.RDSFeed.DataSources.TextFile
{
    public class TextFileDataSource : BaseDataSource
    {
        private string textFilePath;
        private string currentSong;
        private bool isChanged = false;

        public TextFileDataSource(string textfilepath)
        {
            this.textFilePath = textfilepath;
            this.RefreshInterval = 5000;
        }

        public string[] GetSongTitle()
        {
            string song = string.Empty;
            if (File.Exists(this.textFilePath))
            {
                using (StreamReader sr = new StreamReader(File.Open(this.textFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
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
            return new string[] { (isChanged ? song : string.Empty) };
        }

        public override ContentType ContentType
        {
            get { return ContentType.TextFile; }
        }

    }
}