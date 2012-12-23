using System.Configuration;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{
    /// <summary>
    /// <Case when="Checkin" method="GetCheckin" template="{0} checked in on 4square on {1} {2}" />
    /// </summary>
    public class Template
    {
        
        public Template(XmlNode xmlNode)
        {
            XmlAttributeCollection attributes = xmlNode.Attributes;
            if (attributes == null || attributes.Count == 0)
            {
                throw new ConfigurationErrorsException(
                    "A Templates section for a Source does not contain When, Method or Template attributes for a Case node.");
            }
            foreach(XmlAttribute attribute in attributes)
            {
                switch (attribute.Name.ToLower())
                {
                    case "when":
                        this.When = attribute.InnerText;
                        break;
                    case "method":
                        this.MethodName = attribute.InnerText;
                        break;
                    case "template":
                        this.Text = attribute.InnerText;
                        break;
                }
            }
            if (this.MethodName == string.Empty)
            {
                throw new ConfigurationErrorsException(
                    "A Case node in a Templates section does not have MethodName defined.");
            }
        }

        public string When { get; private set; }
        public string MethodName { get; private set; }
        public string Text { get; private set; }
    }
}