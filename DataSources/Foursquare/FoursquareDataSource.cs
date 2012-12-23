using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using GreenlakeChristmas.FourSquare.Objects;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources.Foursquare
{
    public class FoursquareDataSource : IDataSource
    {
        private string oauth_token;
        private string venue_id;
        private string data_file_path;
        private LogFile logFile;
        private DataMode dataMode;
        private List<Template> templates;

        private enum DataMode
        {
            Unknown,
            Checkin,
            Mayor
        }

        public FoursquareDataSource(string oauthtoken, string venueid, string datafilepath)
        {
            if (string.IsNullOrEmpty(oauthtoken) ||
                string.IsNullOrEmpty(venueid) ||
                string.IsNullOrEmpty(datafilepath))
            {
                throw new ConfigurationErrorsException("FoursquareDataSource requires non-empty values for oauthtoken, venueid and datafilepath.");
            }
            this.oauth_token = oauthtoken;
            this.venue_id = venueid;
            this.data_file_path = datafilepath;
            this.logFile = new LogFile(this.data_file_path);
            this.dataMode = DataMode.Checkin;
            this.RefreshInterval = 30000;
            this.templates = new List<Template>();
        }

        public ContentType ContentType
        {
            get { return ContentType.Foursquare; }
        }

        public Priority Priority { get; set; }
        
        public ConcurrentQueue<string> Queue { get; set; }

        public string GetRDSText()
        {
            string output = string.Empty;
            Template template = this.GetTemplate(this.dataMode);
            MethodInfo methodInfo = this.GetType().GetMethod(template.MethodName);
            object[] values = (object[]) methodInfo.Invoke(this, null);
            return string.Format(template.Text, values);
        }

        public object[] GetCheckin()
        {
            string[] output = new string[3];
            ApiResponse response = ApiResponse.GetVenue(this.oauth_token, this.venue_id);
            List<Checkin> checkins = new List<Checkin>();
            checkins.AddRange(response.Venue.HereNow.HereNowGroups.SelectMany(hng => hng.Checkins));
            this.logFile.Append(checkins, "Id");

            if (logFile.LogRecords.Any())
            {
                LogRecord lr = logFile.GetRandomRecord();
                DateTime dt = Convert.ToDateTime(lr.GetValue("CreatedAt"));
                string monthname = dt.ToString("MMMM");
                int day = dt.Day;
                output[0] = lr.GetValue("Name");
                output[1] = monthname;
                output[2] = this.GetOrdinal(day);
            }
            return output;
        }

        public object[] GetMayor()
        {
            string[] output = new string[1];
            ApiResponse response = ApiResponse.GetVenue(this.oauth_token, this.venue_id);
            if (response.Venue.Mayor != null)
            {
                Mayor mayor = response.Venue.Mayor;
                output[0] = mayor.User.FullName;
            }
            return output;
        }

        public int RefreshInterval { get; set; }
        public void Add(Template template)
        {
            this.templates.Add(template);
        }

        private Template GetTemplate(DataMode dataMode)
        {
            foreach(Template template in this.templates)
            {
                if (template.When.ToLower() == dataMode.ToString().ToLower())
                {
                    return template;
                }
            }
            return null;
        }

        private string GetOrdinal(int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString() + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num.ToString() + "st";
                case 2:
                    return num.ToString() + "nd";
                case 3:
                    return num.ToString() + "rd";
                default:
                    return num.ToString() + "th";
            }

        }

    }
}