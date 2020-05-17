using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async void SendMessageCalories(Message message, TelegramBotClient client, long id,string goal)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat.Id, $"Для {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} калорий каждый день\n" +
                $"из которых:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} грамм белка\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} грамм жиров\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} грамм углеводов\n" +
                $"напиши\n/addfood [название] [вес] - чтобы добавить еду");
            else
                await client.SendTextMessageAsync(message.Chat.Id, $"For {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} calories per day\n" +
                $"of which:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} grams proteins\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} grams fats\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} grams carbohydrates\n" +
                $"write\n/addfood [name] [weight] - to add food");
        }
    }
}
