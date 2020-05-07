using Telegram.Bot;
using Telegram.Bot.Args;

namespace Calories.CallBacks
{
    class LostCallBack : CallBack
    {
        public override string Name => "LOST";
        public override void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            var callbackQuery = e.CallbackQuery;

            if (callbackQuery.Data == "normaly")
            {
                user.Calories = (int)(user.Calories * 0.90);
                user.Goal = "normaly";
            }
            else if (callbackQuery.Data == "quickly")
            {
                user.Calories = (int)(user.Calories * 0.75);
                user.Goal = "quickly";
            }
            else if (callbackQuery.Data == "extremely")
            {
                user.Calories = (int)(user.Calories * 0.59);
                user.Goal = "extremely";
            }
            user.Proteins = (int)((user.Calories * 0.3) / 4 + 1);
            user.Fats = (int)((user.Calories * 0.3) / 9 + 1);
            user.Carbohydrates = (int)((user.Calories * 0.4) / 4 + 1);
            user.Command = " ";
            user.SetCalories();
            if (callbackQuery.Data == "normaly")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(e, client, id, "безопасной потери веса");
                else
                    SendMessageCalories(e, client, id, $"weight loss {user.Goal}");
            }
            else if (callbackQuery.Data == "quickly")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(e, client, id, "быстрой потери веса");
                else
                    SendMessageCalories(e, client, id, $"weight loss {user.Goal}");
            }
            else if (callbackQuery.Data == "extremely")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(e, client, id, "потери веса екстремально быстро");
                else
                    SendMessageCalories(e, client, id, $"weight loss {user.Goal}");
            }
        }
    }
}
