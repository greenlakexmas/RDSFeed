using System.Collections.Generic;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{
    public class Options
    {
        private Dictionary<string, Parameter> parameters;

        public Options(XmlNode xmlNode)
        {
            this.parameters = new Dictionary<string, Parameter>();
            XmlNodeList xnlParameters = xmlNode.SelectNodes("Parameter");
            if (xnlParameters == null) return;
            foreach (XmlNode xnParameter in xnlParameters)
            {
                Parameter parameter = new Parameter(xnParameter);
                this.parameters.Add(parameter.Name, parameter);
            }
        }

        public Parameter GetParameter(string name)
        {
            return (parameters.ContainsKey(name) ? parameters[name] : null);
        }
    }
}