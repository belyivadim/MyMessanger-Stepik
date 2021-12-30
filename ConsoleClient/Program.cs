using System;
using Newtonsoft.Json;

namespace MyMessanger_Stepik
{
    public static class Program
    {
        private static int MessageId;
        private static string UserName;
        private static MessangerClientAPI API = new MessangerClientAPI();

        private static void GetNewMessage()
        {
            Message msg = API.GetMessage(MessageId);
            while (msg != null)
            {
                Console.WriteLine(msg);
                MessageId++;
                msg = API.GetMessage(MessageId);
            }
        }
        static void Main(string[] args)
        {
            //Message msg = new Message("Vadim", "Hello!", DateTime.UtcNow);
            //string output = JsonConvert.SerializeObject(msg);
            //Console.WriteLine(output);
            //Message deserializeMsg = JsonConvert.DeserializeObject<Message>(output);
            //Console.WriteLine(deserializeMsg);
            //{ "UserName":"Vadim","MessageText":"Hello!","TimeStamp":"2021-12-30T19:57:41.5287026Z"}
            //Vadim < 30.12.2021 19:57:41 >: Hello!
            MessageId = 1;
            Console.WriteLine("Введите Ваше имя: ");
            UserName = Console.ReadLine();
            string MessageText = "";
            while (MessageText != "exit")
            {
                GetNewMessage();
                Console.WriteLine("Введите текст сообщения: ");
                MessageText = Console.ReadLine();
                if (MessageText.Length > 1)
                {
                    Message Sendmsg = new Message(UserName, MessageText, DateTime.Now);
                    API.SendMessage(Sendmsg);
                }
            }
        }
    }
}