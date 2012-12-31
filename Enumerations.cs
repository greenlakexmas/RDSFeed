namespace GreenlakeChristmas.RDSFeed
{
    public enum ContentType
    {
        Unknown = 0,
        Foursquare = 1,
        Facebook = 2,
        TextFile = 3,
        Twitter = 4
    }

    public enum Priority
    {
        Immediate,
        General
    }

    public enum Rotation
    {
        RoundRobin,
        Random
    }


}
