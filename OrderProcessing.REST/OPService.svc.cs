using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using opdblib_ado;

namespace OrderProcessing.REST
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OPService
    {
        private Order db;
        public OPService()
        {
           db = new Order("ism6236", "ism6236bo");
        }
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

        [OperationContract]
        [WebGet(UriTemplate = "/Customer/{Cid}",ResponseFormat = WebMessageFormat.Json)]
        public String GetCustomer(String Cid)
        {
            return db.getCustomer(Cid);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/Product")]
        public List<String> getProductIds()
        {
            return db.getProductIds();
        }

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
           UriTemplate = "/Order", ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped)]
        public int Purchase(String od)
        {
            string[] vals = od.Split(';');
            string cid = vals[0];
            List<string> ods = new List<string>();
            for (int j = 1; j < vals.Length; j++)
                ods.Add(vals[j]);
            return db.Purchase(cid, ods);
        }
        // Add more operations here and mark them with [OperationContract]
    }
}
