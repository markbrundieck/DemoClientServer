using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

    /// <summary>
    /// Created by Mark Brundieck; July 2015
    /// </summary>
namespace SentinelServiceLibrary
{
    public class SentinelService : ISentinelService
    {

        public string GetData(string value)
        {
            return ProcessData(value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
       
        private string ProcessData(string radiusInput)
        {
            string returnMessage=string.Empty;
            double radiusValue;
            if (double.TryParse(radiusInput, out radiusValue))
            {
                double areaFound = Math.Pow(radiusValue,2) * Math.PI;
                OutputToConsole(radiusInput, areaFound);
                returnMessage = String.Format("Area received from the server is {0}", areaFound.ToString());
            }

            return returnMessage;
        }

        private void OutputToConsole(string clientInput, double areaFound)
        {
            Console.WriteLine("Radius received from client: {0}", clientInput);
            Console.WriteLine("Area found: {0}", areaFound);
        }
    }
}
