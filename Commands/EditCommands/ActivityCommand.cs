using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class ActivityCommand : Command
    {
        public static InlineKeyboardMarkup keyboardAct = new InlineKeyboardMarkup(new[]
                               {
                        new [] { InlineKeyboardButton.WithCallbackData("Basal Metabolic Rate (BMR)", "BMR"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Sedentary: little or no exercise", "Sedentary"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Light: exercise 1 times/week", "Light"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Moderate: exercise 2-3 times/week", "Moderate"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Active: exercise 4-5 times/week", "Active"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Very Active: intense exercise 6-7 times/week", "VeryActive"),},
                        new []{InlineKeyboardButton.WithCallbackData("Extra Active: very intense exercise daily, or physical job", "ExtraActive"),}
                                });
        public static InlineKeyboardMarkup keyboardActRus = new InlineKeyboardMarkup(new[]
                                       {
                        new [] { InlineKeyboardButton.WithCallbackData("Базовый Метаболизм", "BMR"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Отсутствие физической нагрузки ", "Sedentary"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Тренировка 1 раз в неделю", "Light"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Тренировка 2-3 раз в неделю", "Moderate"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Тренировка 4-5 раз в неделю", "Active"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Интенсивная тренировка 5-6 раз в неделю", "VeryActive"),},
                        new []{InlineKeyboardButton.WithCallbackData("Интенсивная тренировка каждый день \nили физическая работа", "ExtraActive"),}
                                });
        public override string Name => @"/activity";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, "Как часто ты занимаешься спортом?",
                        replyMarkup: keyboardActRus);
            else
                await client.SendTextMessageAsync(message.Chat, "How often do you training?",
       replyMarkup: keyboardAct);
            user.edit = true;
            user.Command = "ACTIVITY";
        }
    }
}
