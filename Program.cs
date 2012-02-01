﻿using System;
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
            foreach (MonitorConfig monitor in config.Monitors)
            {
                var isHealthy = IsHealthy(monitor);
                ProcessNotification(monitor, isHealthy);
            }
        }

        // http://msdn.microsoft.com/en-us/library/system.net.webrequest.aspx
        protected static bool IsHealthy(MonitorConfig config)
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

        protected static void ProcessNotification(MonitorConfig config, bool isHealthy)
        {
            Console.WriteLine("Is {0} healthy? {1}", config.Uri, isHealthy);

            if (!isHealthy)
            {
                Console.WriteLine("I would totally be notifying someone of this right now, but I'm not, really");
            }
        }
    }
}
