using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{
    public class Source
    {
        private List<Template> templates;
 
        public Source(XmlNode xmlNode)
        {
            XmlNode xnode = xmlNode.SelectSingleNode("RefreshInterval");
            if (xnode == null)
            {
                throw new ConfigurationErrorsException("Could not find RefreshInterval for a Source.");
            }
            this.RefreshInterval = Convert.ToInt32(xnode.InnerText) * 1000;

            xnode = xmlNode.SelectSingleNode("Priority");
            if (xnode == null)
            {
                throw new ConfigurationErrorsException("Could not find Priority for a Source.");
            }
            Priority priority = Priority.General;
            if (Enum.TryParse(xnode.InnerText, true, out priority))
            {
                this.Priority = priority;
            }

            this.templates = new List<Template>();
            XmlAttributeCollection attributes = xmlNode.Attributes;
            if (attributes == null || attributes.Count == 0)
            {
                throw new ConfigurationErrorsException("Name and Type attributes not found for a Source: " + xmlNode.InnerXml);
            }
            foreach(XmlAttribute attribute in attributes)
            {
                switch (attribute.Name.ToLower())
                {
                    case "name":
                        this.Name = attribute.InnerText;
                        break;
                    case "type":
                        try
                        {
                            this.SourceType = Type.GetType(attribute.InnerText);
                        } catch(Exception e)
                        {
                            throw new ConfigurationErrorsException(
                                string.Format("The source type {0} cannot be referenced.", attribute.InnerText));
                        }
                        break;
                }
            }
            if (this.Name == string.Empty || this.SourceType == null)
            {
                throw new ConfigurationErrorsException("The Source node must have a 'name' and 'type' attribute.");
            }
            XmlNode xnConstructor = xmlNode.SelectSingleNode("Constructor");
            this.Constructor = new Constructor(xnConstructor);
            XmlNodeList xnlTemplates = xmlNode.SelectNodes("Templates/Case");
            if (xnlTemplates == null)
            {
                throw new ConfigurationErrorsException(string.Format("The Source {0} has no templates assigned.",
                                                                     this.Name));
            }
            foreach(XmlNode xnTemplate in xnlTemplates)
            {
                Template template = new Template(xnTemplate);
                this.templates.Add(template);
            }
        }

        public string Name { get; private set; }
        public Type SourceType { get; private set; }
        public int RefreshInterval { get; private set; }
        public Constructor Constructor { get; private set; }
        public Template[] Templates
        {
            get { return this.templates.ToArray(); }
        }

        public Priority Priority { get; private set; }
    }
}