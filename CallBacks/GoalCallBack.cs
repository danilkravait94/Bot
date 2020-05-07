using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.CallBacks
{
    class GoalCallBack : CallBack
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
        public static InlineKeyboardMarkup keyboardMinusDaNet = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
         {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Да","Yes"),
                                                    InlineKeyboardButton.WithCallbackData("Нет","No"),
                                          },
        });
        public static InlineKeyboardMarkup keyboardMinusYesNo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Yes","Yes"),
                                                    InlineKeyboardButton.WithCallbackData("No","No"),
                                          },
               });
        public static InlineKeyboardMarkup keyboardLoss = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                     {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Normaly","normaly"),
                                      },
                                      new [] {
                                                    InlineKeyboardButton.WithCallbackData("Quickly","quickly"),
                                      },
                                      new [] {
                                                    InlineKeyboardButton.WithCallbackData("Extremely","extremely"),
                                      },
                                    });
        public static InlineKeyboardMarkup keyboardLossRus = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Безопасно","normaly"),
                                      },
                                      new [] {
                                                    InlineKeyboardButton.WithCallbackData("Быстро","quickly"),
                                      },
                                      new [] {
                                                    InlineKeyboardButton.WithCallbackData("Екстремально быстро","extremely"),
                                      },
                                     });
        public override string Name => "GOAL";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            var callbackQuery = e.CallbackQuery;
            user.BodyMassIndex = (10000 * user.Weight) / (user.Height * user.Height);
            if (user.Male)
            { user.Calories =(int)( 88.363 + (13.397 * user.Weight) + (4.799 * user.Height) - (5.677 * user.Age)); }
            else
            { user.Calories = (int)(447.593 + (9.247 * user.Weight) + (3.098 * user.Height) - (4.330 * user.Age)); }
            user.Calories = (int)(user.Calories*user.Activity);
            user.Proteins = (int)((user.Calories * 0.2) / 4 + 1);
            user.Fats = (int)((user.Calories * 0.3) / 9 + 1);
            user.Carbohydrates = (int)((user.Calories * 0.5) / 4 + 1);
            if (callbackQuery.Data == "Minus")
            {
                if (user.BodyMassIndex < 18)
                {
                    user.Command = "GOAL";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "Ты и так худой(ая), куда еще худеть?\n" +
                            "Я советую тебе набрать пару кг",
                            replyMarkup: keyboardGoalRus);
                    else
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "You are slender\n" +
                            "I advise you to put on a few pounds!",
                            replyMarkup: keyboardGoal);
                }
                else if (user.BodyMassIndex >= 18 && user.BodyMassIndex < 24)
                {
                    user.Command = "MINUS";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе худеть," +
                            " ведь твое тело в норме)\n" +
                            "Хочешь ли ты поменять свою цель?",
                            replyMarkup: keyboardMinusDaNet);
                    else
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against weight loss," +
                            " your body is just fine\n" +
                            "Do you want to change your goal?",
                            replyMarkup: keyboardMinusYesNo);
                }
                else if (user.BodyMassIndex >= 24)
                {
                    user.Goal = "Minus";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(callbackQuery.From.Id, $"Выбери режим похудения\n" +
                                                        $"Безопасный(0.25 кг/нд)\n" +
                                                        $"Быстрый(0.5 кг/нд)\n" +
                                                        $"Екстремально быстрый(1 кг/нд)", replyMarkup: keyboardLossRus);
                    else
                        await client.SendTextMessageAsync(callbackQuery.From.Id, $"How you want to lose weight:\n" +
                                                        $"normaly(0.25 kg/week)\n" +
                                                        $"quickly(0.5 kg/week)\n" +
                                                        $"extremely(1 kg/week)", replyMarkup: keyboardLoss);
                    user.Command = "LOST";
                }
            }
            else if (callbackQuery.Data == "Norm")
            {
                user.Goal = "Norm"; user.Command = " ";
                if (user.Language == "Russian")
                    SendMessageCalories(e, client, id, "поддержки веса");
                else
                    SendMessageCalories(e, client, id, $"keeping fit");
            }
            else if (callbackQuery.Data == "Plus")
            {
                if (user.BodyMassIndex < 24)
                {
                    user.Goal = "Plus";
                    user.Calories = (int)(user.Calories * 1.2);
                    user.Proteins = (int)((user.Calories * 0.2) / 4 + 1);
                    user.Fats = (int)((user.Calories * 0.3) / 9 + 1);
                    user.Carbohydrates = (int)((user.Calories * 0.5) / 4 + 1);
                    user.Command = " ";
                    if (user.Language == "Russian")
                        SendMessageCalories(e, client, id, "набора веса");
                    else
                        SendMessageCalories(e, client, id, $"putting weight");
                }
                else if (user.BodyMassIndex >= 24 && user.BodyMassIndex <= 30)
                {
                    user.Command = "PLUS";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе набирать вес," +
                            " ведь у тебя и так присутствует лишний вес\n" +
                            "Хочешь ли ты поменять свою цель?",
                            replyMarkup: keyboardMinusDaNet);
                    else
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against putting on weight," +
                            " because you are already overweight\n" +
                            "Do you want to change your goal?",
                            replyMarkup: keyboardMinusYesNo);
                }
                else if (user.BodyMassIndex > 30)
                {
                    user.Command = "GOAL";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "У тебя и так достаточно лишнего веса\n" +
                            "Я советую тебе похудеть",
                            replyMarkup: keyboardGoalRus);
                    else
                        await client.SendTextMessageAsync(callbackQuery.From.Id, "You are overweight\n" +
                            "I advise you to lose weight!",
                            replyMarkup: keyboardGoal);
                }
            }
            user.SetCalories();
        }
    }
}

