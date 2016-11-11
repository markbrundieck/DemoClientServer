using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Created by Mark Brundieck; July 2015
/// </summary>
namespace SentinelMainStartup
{
	class Program
	{
		static void Main(string[] args)
		{
			//support for command line startup, process args
            if (args.Length > 0)
			{
				ProcessInput(args);
			}
			else
			{
				OutputExpectedInput();
				Console.WriteLine();
				Converse();
			}
		}

		/// <summary>
		/// recursively handle input from user and provide output as appropriate.
		/// </summary>
        private static void Converse()
		{
			string inputLine = Console.ReadLine();
			if (inputLine.Trim().Length > 0)
			{
				if (inputLine.ToLower().CompareTo("end") == 0)
				{
					EndThisProgram();
				}

				string[] inputArray = inputLine.Split(' ');
				ProcessInput(inputArray);
			}

			// the only way out of this is to type "end"
            Converse();
		}

		/// <summary>
		/// process for ending this main startup program
		/// </summary>
        private static void EndThisProgram()
		{
			Console.WriteLine("Ending Main application. Client and server need to be closed independently.");
			Console.WriteLine();
			System.Environment.Exit(-1);
		}

		/// <summary>
		/// default instructions
		/// </summary>
        private static void OutputExpectedInput()
		{
			Console.WriteLine("Enter 'start server', 'start client', 'start both' or 'end'");
		}

		/// <summary>
		/// Main routine for processing instructions
		/// </summary>
		/// <param name="args">array of arguments</param>
        private static void ProcessInput(string[] args)
		{
			switch (args.Length)
			{
				case 1:
                    if (args[0].ToLower().CompareTo("end") == 0)
				{
					// Future enhancement: close client & server by finding the process and killing it.
                        EndThisProgram();
				}
				else
				{
					OutputExpectedInput();
				}

				break;
				case 2:
                    ProcessStart(args[1]);
					Console.WriteLine("Started " + args[1]);
					Console.WriteLine();
                    break;
                default:
                    OutputExpectedInput();
                    break;
            }
        }
        /// <summary>
        /// routine for starting either client or server
        /// </summary>
        /// <param name="whichProcess">determines whether it is client or server</param>
        private static void ProcessStart(string whichProcess)
        {
            switch (whichProcess)
            {
                case "client":
                    if (!FoundProcessRunning( whichProcess))
                    {
                        Process.Start(GetClientPath());
                    } 
                    else
                    {
                        Console.WriteLine("Client already running.");
                    }
                    break;
                case "server":
                    if (!FoundProcessRunning(whichProcess))
                    {
                        Process.Start(GetServerPath());
                    }
                    else
                    {
                        Console.WriteLine("Server already running.");
                    }
                    break;
                case "both":
                    ProcessStart("server");
                    ProcessStart("client");
                    break;
                default:
                    OutputExpectedInput();
                    break;
            }
        }
       /// <summary>
       /// checks to see whether process is already running
       /// </summary>
       /// <param name="whichProcess">expects: client or server</param>
       /// <returns>whether process was found</returns>
        private static bool FoundProcessRunning(string whichProcess)
        {
            var processName = string.Empty;
           
            switch (whichProcess)
            {
                case "client":
                     FileInfo infoClient = new FileInfo(GetClientPath());
                     processName = infoClient.Name;
                    break;
                case "server":
                     FileInfo infoServer = new FileInfo(GetServerPath());
                     processName = infoServer.Name.Remove(infoServer.Name.Length-4, 4);
                    break;
            }

            Process[] names = Process.GetProcessesByName(processName);
            return names.Length > 0;
        }
        /// <summary>
        /// getting client path from config
        /// </summary>
        /// <returns></returns>
        private static string GetClientPath()
        {
            string appPath = ConfigurationManager.AppSettings["ClientAppPath"];
            if (appPath.Trim().Length == 0) throw new ConfigurationErrorsException("ClientAppPath not set for Sentinel client program.");
            return appPath;
        }
        /// <summary>
        /// getting server path from config
        /// </summary>
        /// <returns></returns>
        private static string GetServerPath()
        {
            string appPath = ConfigurationManager.AppSettings["ServerAppPath"];
            if (appPath.Trim().Length == 0) throw new ConfigurationErrorsException("ServerAppPath not set for Sentinel server program.");
            return appPath;
        }
    }
}
