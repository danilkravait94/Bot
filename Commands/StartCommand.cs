using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Calories.Commands
{

    class StartCommand : Command
    {
        InlineKeyboardMarkup keyboardHi = new InlineKeyboardMarkup(new[]
                {
                        new [] { InlineKeyboardButton.WithCallbackData("English", "English"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Русский", "Russian"),}
                        });
        public override string Name => @"/start";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.Command = "LANGUAGE";

            await client.SendTextMessageAsync(message.Chat, $"Hi, {message.Chat.FirstName}, select the language you would like to use:\n" +
                        "Привет, выбери язык, который хотел бы использовать:", replyMarkup: keyboardHi);
        }
    }
}
