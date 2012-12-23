using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{
    public class RDSConfig
    {
        private List<Source> sources; 

        public RDSConfig()
        {
            string filename = "rds.config";
            if (!File.Exists(filename))
            {
                throw new ConfigurationErrorsException("The file rds.config could not be found.");
            }
            this.sources = new List<Source>();
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(filename);
            this.unpack(xdoc);
        }

        private void unpack(XmlDocument xdoc)
        {
            XmlNode xnode = xdoc.SelectSingleNode("RDSFeed/RefreshInterval");
            if (xnode == null)
            {
                throw new ConfigurationErrorsException("Could not find RefreshInterval in configuration.");
            }
            this.RefreshInterval = Convert.ToInt32(xnode.InnerText) * 1000;

            xnode = xdoc.SelectSingleNode("RDSFeed/MergeFile");
            if (xnode == null)
            {
                throw new ConfigurationErrorsException("Could not find MergeFile in configuration.");
            }
            this.MergeFile = xnode.InnerText;
            XmlNodeList nodeList = xdoc.SelectNodes("RDSFeed/Sources/Source");
            if (nodeList == null)
            {
                throw new ConfigurationErrorsException("Could not find any Source in configuration.");
            }
            foreach(XmlNode xmlNode in nodeList)
            {
                Source source = new Source(xmlNode);
                this.sources.Add(source);
            }
        }

        public int RefreshInterval { get; private set; }
        public string MergeFile { get; private set; }
        public Source[] Sources
        {
            get { return this.sources.ToArray(); }
        }

    }
}