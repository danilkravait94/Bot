using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.CallBacks
{
    class ActivityCallBack : CallBack
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
        public override string Name => "ACTIVITY";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            var callbackQuery = e.CallbackQuery;
            if (callbackQuery.Data == "BMR") { user.Activity = 1; }
            else if (callbackQuery.Data == "Sedentary") { user.Activity = 1.1; }
            else if (callbackQuery.Data == "Light") { user.Activity = 1.2375; }
            else if (callbackQuery.Data == "Moderate") { user.Activity = 1.375; }
            else if (callbackQuery.Data == "Active") { user.Activity = 1.43; }
            else if (callbackQuery.Data == "VeryActive") { user.Activity = 1.55; }
            else if (callbackQuery.Data == "ExtraActive") { user.Activity = 1.725; }
            if (!user.edit)
            {
                user.Command = "GOAL";
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
                        replyMarkup: keyboardGoalRus);
                else
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
                        replyMarkup: keyboardGoal);
            }
            else
            {
                user.Command = " "; user.edit = false;
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "Активность успешно изменёна!");
                else
                    await client.SendTextMessageAsync(callbackQuery.From.Id, "The level of activity edited successfully!");
            }
        }
    }
}
