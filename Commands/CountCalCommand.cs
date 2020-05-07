using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class CountCalCommand : Command
    {
        public static InlineKeyboardMarkup keyboardSex = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Male","Male"),
                                                    InlineKeyboardButton.WithCallbackData("Female","Female"),
                                          },
                                     });
        public static InlineKeyboardMarkup keyboardSexRus = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                     {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Мужчина","Male"),
                                                    InlineKeyboardButton.WithCallbackData("Женщина","Female"),
                                          },
                                    });
        public override string Name => "/countcal";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.Command = "GENDER"; 

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, "Выбери свой пол", replyMarkup: keyboardSexRus);
            else
                await client.SendTextMessageAsync(message.Chat, "Choose your gender", replyMarkup: keyboardSex);
        }
    }
}