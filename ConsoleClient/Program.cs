using System;
using Newtonsoft.Json;

namespace MyMessanger_Stepik
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Message msg = new Message("Vadim", "Hello!", DateTime.UtcNow);
            string output = JsonConvert.SerializeObject(msg);
            Console.WriteLine(output);
            Message deserializeMsg = JsonConvert.DeserializeObject<Message>(output);
            Console.WriteLine(deserializeMsg);
            //{ "UserName":"Vadim","MessageText":"Hello!","TimeStamp":"2021-12-30T19:57:41.5287026Z"}
            //Vadim < 30.12.2021 19:57:41 >: Hello!
        }
    }
}