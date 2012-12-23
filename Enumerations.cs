using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenlakeChristmas.RDSFeed
{
    public enum ContentType
    {
        Unknown = 0,
        Foursquare = 1,
        Facebook = 2,
        MediaFile = 3
    }

    public enum Priority
    {
        Immediate,
        General
    }

}
