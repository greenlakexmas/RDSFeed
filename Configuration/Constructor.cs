using System.Collections.Generic;
using System.Xml;

namespace GreenlakeChristmas.RDSFeed.Configuration
{
    //      <Constructor>
    //          <Parameter name="mediafilepath" value="c:\Users\jeffro\song.txt" />
    //      </Constructor>
    public class Constructor
    {
        private Dictionary<int, Parameter> parameters;
        private const int START = 1;

        public Constructor(XmlNode xmlNode)
        {
            this.parameters = new Dictionary<int, Parameter>();
            XmlNodeList xnlParameters = xmlNode.SelectNodes("Parameter");
            if (xnlParameters == null) return;
            int x = START;
            foreach (XmlNode xnParameter in xnlParameters)
            {
                Parameter parameter = new Parameter(xnParameter);
                this.parameters.Add(x, parameter);
                x++;
            }
        }

        public Parameter[] Parameters
        {
            get
            {
                Parameter[] parms = new Parameter[this.parameters.Count];
                int t = 0;
                for (int x = START; x <= this.parameters.Count; x++)
                {
                    parms[t] = this.parameters[x];
                    t++;
                }
                return parms;
            }
        }

        public object[] GetConstructorArgs()
        {
            object[] args = new object[this.Parameters.Length];
            int x = 0;
            foreach(Parameter parameter in this.Parameters)
            {
                args[x] = parameter.Value;
                x++;
            }
            return args;
        }
    }
}