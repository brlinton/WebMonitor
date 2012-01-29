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

        public static bool IsHealthy(MonitorSettings config)
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(config.Uri);
            
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            // Display the status.
            //Console.WriteLine(response.StatusDescription);
            
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            
            // Display the content.
            //  TODO - should we display a snippet of the response
            //Console.WriteLine(responseFromServer);
            
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            var isHealthy = config.HttpStatusCode == response.StatusCode;

            return isHealthy;
        }
    }
}
