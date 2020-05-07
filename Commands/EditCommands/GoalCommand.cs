using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class GoalCommand : Command
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
        public override string Name => @"/goal";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, "Какая у тебя цель?", replyMarkup: keyboardGoalRus);
            else
                await client.SendTextMessageAsync(message.Chat, "What is your goal?",
         replyMarkup: keyboardGoal);
            user.edit = true;
            user.Command = "GOAL";
        }
    }
}
