using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;


namespace OPClient.REST
{
    class OPClient
    {
        static string baseuri = "http://localhost:51913/OPService.svc/";

        static HttpClient client = new HttpClient();
        static async Task<String> GetStringAsync(string path)
        {
            string c = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                c = await response.Content.ReadAsAsync<String>();
            }
            return c;

        }

      

        static async Task<String> PurchaseAsync(string path, string cid, List<string> od)
        {

            //Though not the best solution, w/o relying on the JSON library we will Stringify our order first.
            // and convert to a JSOO object with one property called od
            string order = cid ;
            foreach (string s in od)
                order = String.Format("{0};{1}",order,s);
            string mjs = String.Format("{0}\"od\":\"{1}\"{2}", "{", order, "}");

            HttpResponseMessage response = await client.PostAsync(
                path, new StringContent(mjs, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            string x = await response.Content.ReadAsStringAsync();
            return x;
        }

        static async Task<List<String>> GetListAsync(string path)
        {
            List<string> lst = null;
            HttpResponseMessage response =  await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                lst = await response.Content.ReadAsAsync<List<String>>();
            }
            return lst;
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(baseuri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                string c = await GetStringAsync("Customer/1");
                Console.WriteLine(c);

                List<string> pis = await GetListAsync("Product");
                foreach (string s in pis)
                    Console.WriteLine(s);

                List<string> od = new List<string> { "P1,NA,1.99,1", "P3,NA,2.99,2" };
                string h = await PurchaseAsync("Order", "2", od);
                Console.WriteLine(h);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
