using System.Collections.Concurrent;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources
{
    interface IDataSource
    {
        ContentType ContentType { get; }
        Priority Priority { get; set; }
        ConcurrentQueue<string> Queue { get; set; }
        string GetRDSText();
        int RefreshInterval { get; set; }
        void Add(Template template);
    }
}
