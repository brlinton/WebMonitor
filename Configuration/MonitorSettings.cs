using System.Configuration;

namespace WebMonitor.Configuration
{
    // http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx
    public class MonitorSettings : ConfigurationSection
    {
        private static readonly MonitorSettings settings = ConfigurationManager.GetSection("MonitorSettings") as MonitorSettings;

        public static MonitorSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("Monitors", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MonitorCollection),
            AddItemName = "Monitor",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MonitorCollection Monitors
        {
            get
            {
                return (MonitorCollection)base["Monitors"];
            }
        }
    }
}
