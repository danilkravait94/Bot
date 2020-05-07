using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    public abstract class Command : Class_for_DB
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client, long id);
        public bool Contains(string command)
        {
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
        public async void SendMessageCalories(Message message, TelegramBotClient client, long id,string goal)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, $"Для {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} калорий каждый день\n" +
                $"из которых:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} белков\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} жиров\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} углеводов\n" +
                $"напиши\n/addfood [название] [вес] - чтобы добавить еду");
            else
                await client.SendTextMessageAsync(message.Chat, $"For {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} calories per day\n" +
                $"of which:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} proteins\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} fats\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} carbohydrates\n" +
                $"write\n/addfood [name] [weight] - to add food");
        }
    }
}
