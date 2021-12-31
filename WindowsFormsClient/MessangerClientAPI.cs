using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using RestSharp;

namespace MyMessanger_Stepik
{
    class MessangerClientAPI
    {
        private static readonly HttpClient client = new HttpClient();
        public void TestNewtonsoftJson()
        {
            Message msg = new Message("Vadim", "Hello!", DateTime.UtcNow);
            string output = JsonConvert.SerializeObject(msg);
            Console.WriteLine(output);
            Message deserializeMsg = JsonConvert.DeserializeObject<Message>(output);
            Console.WriteLine(deserializeMsg);
        }

        public Message GetMessage(int MessageId)
        {
            WebRequest request = WebRequest.Create("http://localhost:5155/api/Messanger/" + MessageId.ToString());
            request.Method = "Get";
            WebResponse response = request.GetResponse();
            string status = ((HttpWebResponse)response).StatusDescription;
            //Console.WriteLine(status);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            if((status.ToLower() == "ok") && (responseFromServer != "Not found"))
            {
                Message deserializeMsg = JsonConvert.DeserializeObject<Message>(responseFromServer);
                //Console.WriteLine(deserializeMsg);
                return deserializeMsg;
            }
            return null;
        }

        public async Task<Message> GetMessageHTTPAsync(int MessageId)
        {
            var responseString = await client.GetStringAsync("http://localhost:5155/api/Messanger/" + MessageId.ToString());
            if(responseString != null)
            {
                Message deserializeMsg = JsonConvert.DeserializeObject<Message>(responseString);
                return deserializeMsg;
            }
            return null;
        }

        public Message GetMessageRestSharp(int MessageId)
        {
            string ServiceUrl = "http://localhost:5155/api/Messanger/";
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest("/api/Messanger/" + MessageId.ToString(), Method.GET);
            IRestResponse<Message> Response = client.Execute<Message>(request);
            string ResponseContent = Response.Content;
            Message deserializedMsg = JsonConvert.DeserializeObject<Message>(ResponseContent);
            return deserializedMsg;
        }

        public bool SendMessageRestSharp(Message msg)
        {
            string ServiceUrl = "http://localhost:5155";
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest("/api/Messanger", Method.POST);
            // Json to post.
            string jsonToSend = JsonConvert.SerializeObject(msg);
            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            bool ExitIsTrue = false;
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ExitIsTrue = true;
                    }
                    else
                    {
                        ExitIsTrue = false;
                    }
                });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            return ExitIsTrue;
        }

        public bool SendMessage(Message msg)
        {
            WebRequest request = WebRequest.Create("http://localhost:5155/api/Messanger");
            request.Method = "POST";
            string postData = JsonConvert.SerializeObject(msg);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return true;
        }
    }
}
