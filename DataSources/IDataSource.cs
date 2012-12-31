using System.Collections.Concurrent;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources
{
    interface IDataSource
    {
        ContentType ContentType { get; }
        Priority Priority { get; set; }
        ConcurrentQueue<string> Queue { get; set; }
        string GetText();
        int RefreshInterval { get; set; }
        void Add(Template template);
        Rotation TemplateRotation { get; set; }
        Options Options { get; set; }
    }
}
