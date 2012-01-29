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

        [ConfigurationProperty("httpStatusCode", IsRequired = false)]
        public HttpStatusCode HttpStatusCode
        {
            get 
            {
                var code = (HttpStatusCode)this["httpStatusCode"];
                
                if (code == 0)
                {
                    code = HttpStatusCode.OK;
                }

                return code; 
            }
            set 
            { 
                this["httpStatusCode"] = value; 
            }
        }
    }
}
