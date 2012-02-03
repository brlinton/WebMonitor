using System.IO;
using System.Net;
using log4net;
using WebMonitor.Configuration;
using System;

namespace WebMonitor
{
    class Program
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            SetUpLogging();

            var config = MonitorSettings.Settings;
            var errorCount = 0;

            foreach (MonitorConfig monitor in config.Monitors)
            {
                var response = GetWebsiteHealth(monitor);
                
                if (!response.IsHealthy)
                    errorCount++;
                
                ProcessResponse(response);
            }

            var message = string.Format("Found {0} errors out of {1} monitor checks", errorCount, config.Monitors.Count);

            if (errorCount > 0)
                Logger.WarnFormat(message);
        }

        protected static void SetUpLogging()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("Starting the WebMonitor...");
        }

        // http://msdn.microsoft.com/en-us/library/system.net.webrequest.aspx
        protected static HealthResponse GetWebsiteHealth(MonitorConfig config)
        {
            var responseFromServer = string.Empty;
            var isHealthy = true;
            var statusCode = 0;

            try
            {
                Logger.DebugFormat("Checking {0}...", config.Uri);

                // Create a request for the URL. 		
                var request = WebRequest.Create(config.Uri);
                request.Timeout = 10000;

                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;

                // Get the response.
                var response = (HttpWebResponse)request.GetResponse();
                statusCode = (int)response.StatusCode;

                Logger.DebugFormat("Status code = {0}", statusCode);

                // Get the stream containing content returned by the server.
                var dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                var reader = new StreamReader(dataStream);

                // Read the content.
                responseFromServer = reader.ReadToEnd();

                Logger.DebugFormat("Response from server: {0}", responseFromServer);

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                var responseContainsCorrectValue = true;
                var statusesMatch = statusCode == config.StatusCode;

                // If "Contains" was set in the config, verify that it is in the response
                if (!string.IsNullOrWhiteSpace(config.Contains))
                {
                    responseContainsCorrectValue = responseFromServer.Contains(config.Contains);
                }

                isHealthy = statusesMatch && responseContainsCorrectValue;
            }
            catch (Exception ex)
            {
                isHealthy = false;
                responseFromServer = ex.ToString();
            }

            return new HealthResponse
            {
                Uri = config.Uri,
                IsHealthy = isHealthy,
                Response = responseFromServer,
                StatusCode = statusCode
            };
        }

        protected static void ProcessResponse(HealthResponse response)
        {
            Logger.DebugFormat("Is {0} healthy? {1}", response.Uri, response.IsHealthy);

            if (!response.IsHealthy)
            {
                Logger.ErrorFormat("WEBMONITOR HEALTH ALERT for {0}", response.Uri);
                Logger.Error(response.Response);
            }
        }
    }
}
