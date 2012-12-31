using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GreenlakeChristmas.FourSquare.Objects;

namespace GreenlakeChristmas.RDSFeed.DataSources.Foursquare
{
    public class FoursquareDataSource : BaseDataSource
    {
        private string oauth_token;
        private string venue_id;
        private string data_file_path;
        private LogFile logFile;

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
            this.RefreshInterval = 30000;
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

        public override ContentType ContentType
        {
            get { return ContentType.Foursquare; }
        }
    }
}