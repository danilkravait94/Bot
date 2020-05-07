using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.CallBacks
{
    class LanguageCallBack : CallBack
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
        public override string Name => "LANGUAGE";
        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (e.CallbackQuery.Data == "Russian")
            {
                user.Language = "Russian";
                await client.SendTextMessageAsync(
                     e.CallbackQuery.From.Id, $"Привет,я создан что бы помочь" +
                     $" тебе считать твои калории, " +
                     "терять или набирать вес)\n\n" +
                     "Отправь команду:\n" +
                     "/countcal - для подсчета количества калорий, которое нужно тебе каждый день для достяжение твоих целей\n");
            }
            else if (e.CallbackQuery.Data == "English")
            {
                user.Language = "English";
                await client.SendTextMessageAsync(
                     e.CallbackQuery.From.Id, $"Hi," +
                     "I can help you calculate your calories, " +
                     "lose and put on weight)\n\n" +
                     "Send me a command:\n" +
                     "/countcal - to estimate the number of calories that you need to consume each day\n");
            }
            user.Command = " ";
        }
    }
}
