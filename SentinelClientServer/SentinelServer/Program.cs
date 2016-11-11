using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using SentinelServiceLibrary;

    /// <summary>
    /// Created by Mark Brundieck; July 2015
    /// </summary>
namespace SentinelServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer();
        }

        private static void RunServer()
        {
            //get URI of service from config
            Uri serviceAddress = new Uri(GetUriSetting());

            //new instance of service host
            ServiceHost selfHostServer = new ServiceHost(typeof(SentinelService), serviceAddress);
            try
            {
                //adding service endpoint
                selfHostServer.AddServiceEndpoint(typeof(ISentinelService), new WSHttpBinding(), "SentinelServer");

                //enable metadata exchange
                ServiceMetadataBehavior serverBehavior = new ServiceMetadataBehavior();
                serverBehavior.HttpGetEnabled = true;
                selfHostServer.Description.Behaviors.Add(serverBehavior);

                //running
                selfHostServer.Open();
                Console.WriteLine("Server started at {0}. Press <ENTER> to terminate service.", DateTime.Now);
                Console.WriteLine();
                Console.ReadLine();
                Console.WriteLine("Connection Closed.");
                //close for termination
                selfHostServer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following exception occurred when running the Sentinel Server. {0}", ex.Message);
                selfHostServer.Abort();
            }
        }

        /// <summary>
        /// Obtain the Uri that the service will use.
        /// </summary>
        /// <returns>URI for this service.</returns>
        private static string GetUriSetting()
        {
            var uriValue = ConfigurationManager.AppSettings["ServiceUri"];
            if (uriValue.Trim().Length == 0) throw new ConfigurationErrorsException("ServiceUri not set for Sentinel server program.");
            return uriValue;
        }
    }
}
