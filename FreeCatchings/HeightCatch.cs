using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.FreeCatchings
{

    public class HeightCatch : FreeCatching
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
        public override string Name => "HEIGHT";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            if (float.TryParse(message.Text.Replace('.', ','), out float height))
            {
                if (height <= 30 || height > 250)
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat, $"Твой рост не может быть {height} см\n" +
                        $"Пожалуйста, подумай и введи еще раз)");
                    else
                        await client.SendTextMessageAsync(message.Chat, $"Your height cannot be {height} cm\n" +
                        $"Please, enter your height again");
                }
                else
                {
                    user.Height = height;
                    user.BodyMassIndex = (10000 * user.Weight) / (user.Height * user.Height);
                    if (user.BodyMassIndex < 13 || user.BodyMassIndex > 50)
                    {
                        if (!user.edit)
                        {
                            user.Command = "WEIGHT";
                            if (user.Language == "Russian")
                                await client.SendTextMessageAsync(message.Chat.Id, "Твой индекс массы тела невозможен\n" +
                                    "Введи свой вес(в кг) снова!");
                            else
                                await client.SendTextMessageAsync(message.Chat, "Your body mass index is impossible \n" +
                                    "Please enter your weight (in kilograms) again!");
                        }
                        else
                        {
                            if (user.Language == "Russian")
                                await client.SendTextMessageAsync(message.Chat, $"Твой рост не может быть {height} см," +
                                    $" потому что твой индекс массы тела невозможен\n" +
                                $"Пожалуйста, подумай и введи еще раз)");
                            else
                                await client.SendTextMessageAsync(message.Chat, $"Your height cannot be {height} cm," +
                                    $"because your body mass index is impossible\n" +
                                $"Please, enter your height again");
                            user.Command = "HEIGHT";
                        }
                    }
                    else
                    {
                        if (!user.edit)
                        {
                            user.Command = "ACTIVITY";
                            if (user.Language == "Russian")
                                await client.SendTextMessageAsync(message.Chat, "Как часто ты занимаешься спортом?",
                                    replyMarkup: keyboardActRus);
                            else
                                await client.SendTextMessageAsync(message.Chat, "How often do you training?",
                                    replyMarkup: keyboardAct); 
                        }
                        else
                        {
                            user.Command = " "; user.edit = false;
                            if (user.Language == "Russian")
                                await client.SendTextMessageAsync(message.Chat.Id, "Рост успешно изменён!");
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, "The height edited successfully!");
                        }
                    }
                }
            }
            else
            {
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(message.Chat.Id, "Неправильно введен рост");
                else
                    await client.SendTextMessageAsync(message.Chat.Id, "Error with input of height");
            }
        }
    }
}
