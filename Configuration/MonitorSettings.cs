using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace WebMonitor.Configuration
{
    // http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx
    public class MonitorSettings : ConfigurationSection
    {
        private static MonitorSettings settings = ConfigurationManager.GetSection("MonitorSettings") as MonitorSettings;

        public static MonitorSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get { return (string)this["uri"]; }
            set { this["uri"] = value; }
        }

        // http://msdn.microsoft.com/en-us/library/system.net.httpstatuscode.aspx
        [ConfigurationProperty("statusCode", DefaultValue = 200, IsRequired = false)]
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

        [ConfigurationProperty("contains", IsRequired = false)]
        public string Contains
        {
            get { return (string)this["contains"]; }
            set { this["contains"] = value; }
        }

        // TODO - add some notification settings (who to notify?  only on failure?)
    }
}
