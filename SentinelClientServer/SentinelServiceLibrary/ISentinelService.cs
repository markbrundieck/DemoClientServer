using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

    /// <summary>
    /// Created by Mark Brundieck; July 2015
    /// </summary>
namespace SentinelServiceLibrary
{
    [ServiceContract]
    public interface ISentinelService
    {
        [OperationContract]
        string GetData(string value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "default value";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
