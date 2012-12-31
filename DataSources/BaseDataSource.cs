using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using GreenlakeChristmas.RDSFeed.Configuration;

namespace GreenlakeChristmas.RDSFeed.DataSources
{
    public abstract class BaseDataSource : IDataSource
    {
        private List<Template> templates;
        private int currentTemplateIndex = 0;
        private Random random;

        public BaseDataSource()
        {
            this.templates = new List<Template>();
        }

        public abstract ContentType ContentType { get; }
        public Priority Priority { get; set; }
        public ConcurrentQueue<string> Queue { get; set; }

        public string GetText()
        {
            Template template = this.GetNextTemplate();
            MethodInfo methodInfo = this.GetType().GetMethod(template.MethodName);
            object[] values = (object[])methodInfo.Invoke(this, null);
            if (values == null || values.Length == 0) return string.Empty;
            return string.Format(template.Text, values);
        }

        public int RefreshInterval { get; set; }
        public void Add(Template template)
        {
            this.templates.Add(template);
        }

        public Rotation TemplateRotation { get; set; }
        public Options Options { get; set; }

        protected Template GetNextTemplate()
        {
            Template template = this.templates[this.currentTemplateIndex];
            if (this.templates.Count == 1) return template;
            switch(this.TemplateRotation)
            {
                case Rotation.RoundRobin:
                    this.currentTemplateIndex++;
                    if (this.currentTemplateIndex == this.templates.Count)
                    {
                        this.currentTemplateIndex = 0;
                    }
                    break;
                case Rotation.Random:
                    if (this.random == null)
                    {
                        this.random = new Random();
                    }
                    this.currentTemplateIndex = this.random.Next(0, this.templates.Count - 1);
                    break;
            }
            return template;
        }


    }
}