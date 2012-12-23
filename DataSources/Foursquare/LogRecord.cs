using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GreenlakeChristmas.FourSquare.Objects;

namespace GreenlakeChristmas.RDSFeed.DataSources.Foursquare
{
    public class LogRecord
    {
        private static string pipe = "|";
        private Dictionary<string, string> properties = new Dictionary<string, string>();

        public LogRecord(Checkin checkin)
        {
            this.Add(checkin);
        }

        //public LogRecord(Mayor mayor)
        //{
        //    this.Add(mayor);
        //}

        public LogRecord(string record)
        {
            string[] arRecord = record.Split(LogRecord.pipe.ToCharArray());
            if (arRecord.Length != 5) return;
            this.properties.Add("Id", arRecord[0]);
            this.properties.Add("CreatedAt", arRecord[1]);
            this.properties.Add("UserId", arRecord[2]);
            this.properties.Add("Name", arRecord[3]);
            this.properties.Add("Shout", arRecord[4]);
            //if (properties["Type"] == "Checkin")
            //{
            //    if (arRecord.Length != 5) return;
            //    this.properties.Add("Id", arRecord[0]);
            //    this.properties.Add("CreatedAt", arRecord[1]);
            //    this.properties.Add("UserId", arRecord[2]);
            //    this.properties.Add("Name", arRecord[3]);
            //    this.properties.Add("Shout", arRecord[4]);
            //} else if (properties["Type"] == "Mayor")
            //{
            //    this.properties.Add("Id", arRecord[0]);
            //    this.properties.Add("UserId", arRecord[1]);
            //    this.properties.Add("Name", arRecord[2]);
            //    this.properties.Add("Place", arRecord[3]);
            //}
        }

        protected void Add(Checkin checkin)
        {
            this.properties.Clear();
            this.properties.Add("Type", "Checkin");
            this.properties.Add("Id", checkin.Id);
            this.properties.Add("CreatedAt", checkin.CreatedAt.ToString(CultureInfo.InvariantCulture));
            this.properties.Add("UserId", checkin.User.Id);
            this.properties.Add("Name", checkin.User.FullName);
            this.properties.Add("Shout", checkin.Shout);
        }

        //protected void Add(Mayor mayor)
        //{
        //    this.properties.Clear();
        //    this.properties.Add("Type", "Mayor");
        //    this.properties.Add("Id", new Guid(mayor.User.Id).ToString());
        //    this.properties.Add("UserId", mayor.User.Id);
        //    this.properties.Add("Name", mayor.User.FullName);
        //    this.properties.Add("Place", "GreenlakeChristmas");
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.properties["Type"] + LogRecord.pipe);
            sb.Append(this.properties["Id"] + LogRecord.pipe);
            sb.Append(this.properties["CreatedAt"] + LogRecord.pipe);
            sb.Append(this.properties["UserId"] + LogRecord.pipe);
            sb.Append(this.properties["Name"] + LogRecord.pipe);
            sb.Append(this.properties["Shout"]);
            return sb.ToString();
        }

        public string GetValue(string key)
        {
            return (this.properties.ContainsKey(key) ? this.properties[key] : string.Empty);
        }
    }
}