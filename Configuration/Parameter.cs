using System.Configuration;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{

    public class Parameter
    {
         
        public Parameter(XmlNode xmlNode)
        {
            XmlAttributeCollection attributes = xmlNode.Attributes;
            if (attributes == null || attributes.Count == 0)
            {
                throw new ConfigurationErrorsException(
                    "A Parameter node in a Constructor section does not contain Name or Value attributes.");
            }
            foreach (XmlAttribute attribute in attributes)
            {
                switch (attribute.Name.ToLower())
                {
                    case "name":
                        this.Name = attribute.InnerText;
                        break;
                    case "value":
                        this.Value = attribute.InnerText;
                        break;
                }
            }
            if (this.Name == string.Empty)
            {
                throw new ConfigurationErrorsException(
                    "A Parameter node in a Constructor section does not have Name defined.");
            }
            if (this.Value == string.Empty)
            {
                throw new ConfigurationErrorsException(
                    "A Parameter node in a Constructor section does not have Value defined.");
            }

        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}