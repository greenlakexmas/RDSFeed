using System;
using System.Configuration;
using Facebook;

namespace GreenlakeChristmas.RDSFeed.DataSources.Facebook
{
    public class FacebookDataSource : BaseDataSource
    {
        private FacebookClient facebookClient;
        private string application_id;
        private string application_secret;
        private string object_id;
        private DataKey dataKey;
        //private List<Template> templates;

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
                throw new ConfigurationErrorsException("Facebook requires applicationId, applicationsecret and objectid parameters.");
            }
            this.application_id = applicationid;
            this.application_secret = applicationsecret;
            this.object_id = objectid;
            this.RefreshInterval = 30000;
            this.InitializeClient();
            this.dataKey = DataKey.Checkins;
            //this.templates = new List<Template>();
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

        //public ContentType ContentType
        //{
        //    get { return ContentType.Facebook; }
        //}

        //public Priority Priority { get; set; }

        //public ConcurrentQueue<string> Queue { get; set; }

        //private static DataKey GetNextDataKey(DataKey dataKey)
        //{
        //    if (dataKey == DataKey.Checkins) return DataKey.Likes;
        //    if (dataKey == DataKey.Likes) return DataKey.Talking_About_Count;
        //    if (dataKey == DataKey.Talking_About_Count) return DataKey.Were_Here_Count;
        //    if (dataKey == DataKey.Were_Here_Count) return DataKey.Checkins;
        //    return DataKey.Checkins;
        //}

        //public string GetText()
        //{
        //    string output = string.Empty;
        //    Template template = this.GetTemplate(this.dataKey);
        //    MethodInfo methodInfo = this.GetType().GetMethod(template.MethodName);
        //    object[] values = (object[])methodInfo.Invoke(this, null);
        //    this.dataKey = GetNextDataKey(this.dataKey);
        //    return string.Format(template.Text, values);

        //}

        private string GetValue(DataKey datakey)
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
                value = jsonResult[datakey.ToString().ToLower()].ToString();
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

        //public int RefreshInterval { get; set; }

        //public void Add(Template template)
        //{
        //    this.templates.Add(template);
        //}

        //public Rotation TemplateRotation { get; set; }

        //public Options Options { get; set; }

        //private Template GetTemplate(DataKey datakey)
        //{
        //    foreach (Template template in this.templates)
        //    {
        //        if (template.When.ToLower() == datakey.ToString().ToLower())
        //        {
        //            return template;
        //        }
        //    }
        //    return null;
        //}

        public override ContentType ContentType
        {
            get { return ContentType.Facebook; }
        }
    }
}