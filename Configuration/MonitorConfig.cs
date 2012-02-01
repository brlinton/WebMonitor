using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WebMonitor.Configuration
{
    public class MonitorConfig : ConfigurationElement
    {
        public MonitorConfig() {}

        public MonitorConfig(string uri, int statusCode, string contains)
        {
            Uri = uri;
            StatusCode = statusCode;
            Contains = contains;
        }

        [ConfigurationProperty("uri", IsRequired = true, IsKey = true)]
        public string Uri
        {
            get { return (string)this["uri"]; }
            set { this["uri"] = value; }
        }

        [ConfigurationProperty("statusCode", DefaultValue = 200, IsRequired = false, IsKey = false)]
        public int StatusCode
        {
            get
            {
                return (int)this["statusCode"];
            }
            set
            {
                this["statusCode"] = value;
            }
        }

        [ConfigurationProperty("contains", IsRequired = false, IsKey = false)]
        public string Contains
        {
            get { return (string)this["contains"]; }
            set { this["contains"] = value; }
        }
    }
}
