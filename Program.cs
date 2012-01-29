using System;
using System.IO;
using System.Net;
using WebMonitor.Configuration;

namespace WebMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = MonitorSettings.Settings;
            var isHealthy = IsHealthy(config);

            Console.WriteLine("Is {0} healthy? {1}", config.Uri, isHealthy);
            // TODO - read configuration, building up things to do
            // TODO - spin through the things to do
            // TODO - on failure of the check, execute its settings for how to notify
        }

        // http://msdn.microsoft.com/en-us/library/system.net.webrequest.aspx
        public static bool IsHealthy(MonitorSettings config)
        {
            // Create a request for the URL. 		
            var request = WebRequest.Create(config.Uri);
            
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            
            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();
            
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            
            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(dataStream);
            
            // Read the content.
            var responseFromServer = reader.ReadToEnd();
            
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            var statusCode = (int)response.StatusCode;
            return statusCode == config.StatusCode;
        }
    }
}
