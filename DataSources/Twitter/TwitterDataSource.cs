using System.Collections.Generic;
using System.Linq;
using GreenlakeChristmas.RDSFeed.Configuration;
using Twitterizer;

namespace GreenlakeChristmas.RDSFeed.DataSources.Twitter
{
    public class TwitterDataSource : BaseDataSource
    {
        private OAuthTokens oAuthTokens;
        private decimal lastMentionId = 0;
        private decimal lastRetweetId = 0;
        private decimal lastTweetId = 0;
        private decimal lastHashtagId = 0;
        
        public TwitterDataSource(string accesstoken, string accesstokensecret, string consumerkey, string consumersecret)
        {
            this.oAuthTokens = new OAuthTokens
                                   {
                                       AccessToken = accesstoken,
                                       AccessTokenSecret = accesstokensecret,
                                       ConsumerKey = consumerkey,
                                       ConsumerSecret = consumersecret
                                   };
            this.RefreshInterval = 30000;
        }

        public string[] GetMention()
        {
            TimelineOptions timelineOptions = new TimelineOptions {SinceStatusId = lastMentionId};
            TwitterResponse<TwitterStatusCollection> response = 
                TwitterTimeline.Mentions(this.oAuthTokens, timelineOptions);
            string[] values = new string[] {};
            if (response.Result == RequestResult.Success)
            {
                TwitterStatusCollection statuses = response.ResponseObject;
                List<TwitterStatus> listStatuses = new List<TwitterStatus>(statuses);
                this.lastMentionId = listStatuses.Min(t => t.Id);
                TwitterStatus status = listStatuses.Find(t => t.Id == this.lastMentionId);
                values = new string[] {status.CreatedDate.ToShortDateString(), status.User.Name, status.Text};
            }
            return values;
        }

        public string[] GetRetweet()
        {
            RetweetsOfMeOptions options = new RetweetsOfMeOptions {SinceStatusId = lastRetweetId};
            TwitterResponse<TwitterStatusCollection> response = TwitterTimeline.RetweetsOfMe(oAuthTokens, options);
            string[] values = new string[] { };
            if (response.Result == RequestResult.Success)
            {
                TwitterStatusCollection statuses = response.ResponseObject;
                List<TwitterStatus> listStatuses = new List<TwitterStatus>(statuses);
                this.lastRetweetId = listStatuses.Min(t => t.Id);
                TwitterStatus status = listStatuses.Find(t => t.Id == this.lastRetweetId);
                values = new string[] { status.CreatedDate.ToShortDateString(), status.User.Name, status.Text };
            }
            return values;
        }

        public string[] GetTweet()
        {
            UserTimelineOptions options = new UserTimelineOptions {IncludeRetweets = false, SinceStatusId = lastTweetId};
            TwitterResponse<TwitterStatusCollection> response = TwitterTimeline.UserTimeline(oAuthTokens, options);
            string[] values = new string[] { };
            if (response.Result == RequestResult.Success)
            {
                TwitterStatusCollection statuses = response.ResponseObject;
                List<TwitterStatus> listStatuses = new List<TwitterStatus>(statuses);
                this.lastTweetId = listStatuses.Min(t => t.Id);
                TwitterStatus status = listStatuses.Find(t => t.Id == this.lastTweetId);
                values = new string[] { status.CreatedDate.ToShortDateString(), status.User.Name, status.Text };
            }
            return values;
        }

        public string[] GetHashTweet()
        {
            string[] values = new string[]{};
            if (this.Options == null || this.Options.GetParameter("hashtag") == null)
            {
                return values;
            }
            Parameter hashtagParameter = this.Options.GetParameter("hashtag");
            SearchOptions searchOptions = new SearchOptions
                                              {
                                                  ResultType = SearchOptionsResultType.Recent,
                                                  IncludeEntities = false,
                                                  SinceId = lastHashtagId
                                              };
            TwitterResponse<TwitterSearchResultCollection> response = TwitterSearch.Search(hashtagParameter.Value, searchOptions);
            if (response.Result == RequestResult.Success)
            {
                TwitterSearchResultCollection results = response.ResponseObject;
                List<TwitterSearchResult> listResults = new List<TwitterSearchResult>(results);
                this.lastHashtagId = listResults.Min(t => t.Id);
                TwitterSearchResult searchResult = listResults.Find(t => t.Id == this.lastHashtagId);
                values = new string[] { searchResult.CreatedDate.ToShortDateString(), searchResult.FromUserDisplayName, searchResult.Text };
            }
            return values;
        }

        public override ContentType ContentType
        {
            get { return ContentType.Twitter; }
        }
    }
}