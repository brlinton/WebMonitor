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
            // TODO - make this a collection to iterate through (allowing nested elements in the config)
            var config = MonitorSettings.Settings;
            var isHealthy = IsHealthy(config);
            Console.WriteLine("Is {0} healthy? {1}", config.Uri, isHealthy);
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
            var statusCode = (int)response.StatusCode;

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

            var responseContainsCorrectValue = true;
            var statusesMatch = statusCode == config.StatusCode;

            // If "Contains" was set in the config, verify that it is in the response
            if (!string.IsNullOrWhiteSpace(config.Contains))
            {
                responseContainsCorrectValue = responseFromServer.Contains(config.Contains);
            }

            return statusesMatch && responseContainsCorrectValue;
        }
    }
}
