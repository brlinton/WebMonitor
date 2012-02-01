using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WebMonitor.Configuration
{
    public class MonitorCollection : ConfigurationElementCollection
    {
        public MonitorCollection()
        {
        }

        public MonitorConfig this[int index]
        {
            get { return (MonitorConfig)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(MonitorConfig MonitorConfig)
        {
            BaseAdd(MonitorConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MonitorConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MonitorConfig)element).Uri;
        }

        public void Remove(MonitorConfig MonitorConfig)
        {
            BaseRemove(MonitorConfig.Uri);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
