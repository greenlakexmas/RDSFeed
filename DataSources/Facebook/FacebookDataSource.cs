using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Facebook;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources.Facebook
{
    public class FacebookDataSource : IDataSource
    {
        private FacebookClient facebookClient;
        private string application_id;
        private string application_secret;
        private string object_id;
        private DataKey dataKey;
        private List<Template> templates;

        private enum DataKey
        {
            Checkins,
            Talking_About_Count,
            Were_Here_Count,
            Likes
        }

        public FacebookDataSource(string applicationid, string applicationsecret, string objectid)
        {
            if (string.IsNullOrEmpty(applicationid) ||
                string.IsNullOrEmpty(applicationsecret) ||
                string.IsNullOrEmpty(objectid))
            {
                throw new ConfigurationErrorsException("");
            }
            this.application_id = applicationid;
            this.application_secret = applicationsecret;
            this.object_id = objectid;
            this.RefreshInterval = 30000;
            this.InitializeClient();
            this.dataKey = DataKey.Checkins;
            this.templates = new List<Template>();
        }

        private void InitializeClient()
        {
            if (this.facebookClient == null)
            {
                this.facebookClient = new FacebookClient();
                JsonObject result = (JsonObject)this.facebookClient.Get("oauth/access_token", new
                {
                    client_id = this.application_id,
                    client_secret = this.application_secret,
                    grant_type = "client_credentials"
                });
                this.facebookClient.AccessToken = (string)result["access_token"];
                this.facebookClient.AppId = application_id;
                this.facebookClient.AppSecret = application_secret;
            }
            else
            {
                JsonObject result = (JsonObject) this.facebookClient.Get("oauth/access_token", new
                {
                    client_id = this.application_id,
                    client_secret = this.application_secret,
                    grant_type = "fb_exchange_token",
                    fb_exchange_token = this.facebookClient.AccessToken
                });
                this.facebookClient.AccessToken = (string)result["access_token"];
            }
        }

        public ContentType ContentType
        {
            get { return ContentType.Facebook; }
        }

        public Priority Priority { get; set; }

        public ConcurrentQueue<string> Queue { get; set; }

        private DataKey GetNextDataKey(DataKey dataKey)
        {
            if (dataKey == DataKey.Checkins) return DataKey.Likes;
            if (dataKey == DataKey.Likes) return DataKey.Talking_About_Count;
            if (dataKey == DataKey.Talking_About_Count) return DataKey.Were_Here_Count;
            if (dataKey == DataKey.Were_Here_Count) return DataKey.Checkins;
            return DataKey.Checkins;
        }

        public string GetRDSText()
        {
            string output = string.Empty;
            Template template = this.GetTemplate(this.dataKey);
            MethodInfo methodInfo = this.GetType().GetMethod(template.MethodName);
            object[] values = (object[])methodInfo.Invoke(this, null);
            this.dataKey = this.GetNextDataKey(this.dataKey);
            return string.Format(template.Text, values);

            //string output = string.Empty;
            //JsonObject jsonResult = null;
            //try
            //{
            //    jsonResult = (JsonObject)this.facebookClient.Get(this.object_id);
            //}
            //catch(Exception ex)
            //{
            //    this.InitializeClient();
            //}

            //if (jsonResult != null)
            //{
            //    string keyvalue = jsonResult[this.dataKey.ToString().ToLower()].ToString();
            //    switch(this.dataKey)
            //    {
            //        case DataKey.Checkins:
            //            output = string.Format("{0} people have checked in at Greenlake Christmas on Facebook", keyvalue);
            //            this.dataKey = DataKey.Talking_About_Count;
            //            break;
            //        case DataKey.Talking_About_Count:
            //            output = string.Format("{0} people are talking about Greenlake Christmas on Facebook", keyvalue);
            //            this.dataKey = DataKey.Likes;
            //            break;
            //        case DataKey.Likes:
            //            output = string.Format("{0} people like Greenlake Christmas on Facebook", keyvalue);
            //            this.dataKey = DataKey.Were_Here_Count;
            //            break;
            //        case DataKey.Were_Here_Count:
            //            output = string.Format("{0} people were at Greenlake Christmas on Facebook", keyvalue);
            //            this.dataKey = DataKey.Checkins;
            //            break;
            //    }
            //}
            //return output;
        }

        private string GetValue(DataKey dataKey)
        {
            string value = string.Empty;
            JsonObject jsonResult = null;
            try
            {
                jsonResult = (JsonObject)this.facebookClient.Get(this.object_id);
            }
            catch (Exception ex)
            {
                this.InitializeClient();
            }

            if (jsonResult != null)
            {
                value = jsonResult[dataKey.ToString().ToLower()].ToString();
            }
            return value;
        }

        public string[] GetCheckins()
        {
            string[] values = new string[1];
            values[0] = this.GetValue(DataKey.Checkins);
            return values;
        }

        public string[] GetTalkingAboutCount()
        {
            string[] values = new string[1];
            values[0] = this.GetValue(DataKey.Talking_About_Count);
            return values;
        }

        public string[] GetLikes()
        {
            string[] values = new string[1];
            values[0] = this.GetValue(DataKey.Likes);
            return values;
        }

        public string[] GetWereHereCount()
        {
            string[] values = new string[1];
            values[0] = this.GetValue(DataKey.Were_Here_Count);
            return values;
        }

        public int RefreshInterval { get; set; }

        public void Add(Template template)
        {
            this.templates.Add(template);
        }

        private Template GetTemplate(DataKey dataKey)
        {
            foreach (Template template in this.templates)
            {
                if (template.When.ToLower() == dataKey.ToString().ToLower())
                {
                    return template;
                }
            }
            return null;
        }

    }
}