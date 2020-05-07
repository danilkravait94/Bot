using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.CallBacks
{
    class MinusCallBack : CallBack
    {
        public static InlineKeyboardMarkup keyboardGoal = new InlineKeyboardMarkup(new[]
                        {
                        new [] { InlineKeyboardButton.WithCallbackData("Weight loss", "Minus"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Keeping fit", "Norm"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Putting on weight", "Plus"),}
                        });
        public static InlineKeyboardMarkup keyboardGoalRus = new InlineKeyboardMarkup(new[]
                       {
                        new [] { InlineKeyboardButton.WithCallbackData("Похудение", "Minus"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Поддержка формы", "Norm"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Набор веса", "Plus"),}
                        });
        public override string Name => "MINUS";
        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            var callbackQuery = e.CallbackQuery;

            if (callbackQuery.Data == "Yes")
            {
                user.Command = "GOAL";
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
                        replyMarkup: keyboardGoalRus);
                else
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
                        replyMarkup: keyboardGoal);
            }
            else if (callbackQuery.Data == "No")
            {
                user.Calories = (int)(user.Calories * 0.90);
                user.Proteins = (int)((user.Calories * 0.3) / 4 + 1);
                user.Fats = (int)((user.Calories * 0.3) / 9 + 1);
                user.Carbohydrates = (int)((user.Calories * 0.4) / 4 + 1); user.Command = " ";
                user.SetCalories();
                if (user.Language == "Russian")
                    SendMessageCalories(e, client, id, "безопасной потери веса");
                else
                    SendMessageCalories(e, client, id, $"weight loss {user.Goal}");
            }
        }
    }
}
