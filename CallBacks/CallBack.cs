using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Calories.CallBacks
{
    public abstract class CallBack : Class_for_DB
    {
        public abstract string Name { get; }
        public abstract void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id);
        public bool Contains(string callback)
        {
            return callback.Contains(this.Name);
        }
        public async void SendMessageCalories(CallbackQueryEventArgs e, TelegramBotClient client, long id, string goal)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Для {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} калорий каждый день\n" +
                $"из которых:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} белков\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} жиров\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} углеводов\n" +
                $"напиши\n/addfood [название] [вес] - чтобы добавить еду");
            else
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"For {goal}:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Calories} calories per day\n" +
                $"of which:\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Proteins} proteins\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Fats} fats\n" +
                $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t{user.Carbohydrates} carbohydrates\n" +
                $"write\n/addfood [name] [weight] - to add food");
        }
    }
}
