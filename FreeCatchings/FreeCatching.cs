using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{
    public abstract class FreeCatching : Class_for_DB
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client, long id);
        public bool Contains(string command)
        {
            if (command == null) return false;
            return command.Contains(this.Name);
        }
        public static string GetFood(string linkoffood)
        {
            WebRequest request = WebRequest.Create(linkoffood);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader reader = new StreamReader(s);
            string answer = reader.ReadToEnd();
            response.Close();
            return answer;
        }
    }
}
