using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;


namespace tekeg
{
    class Program
    {
        private static TelegramBotClient BotClient;
        private static bool here = false, edit = false;
        public static UserContext DB = new UserContext();
        public static InlineKeyboardMarkup keyboardGoal = new InlineKeyboardMarkup(new[]
                        {
                        new [] { InlineKeyboardButton.WithCallbackData("Weight loss", "Minus"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Keeping fit", "Norm"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Putting on weight", "Plus"),}
                        });
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
        public static InlineKeyboardMarkup keyboardSex = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Male","Male"),
                                                    InlineKeyboardButton.WithCallbackData("Female","Female"),
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
        public static InlineKeyboardMarkup keyboardGoalRus = new InlineKeyboardMarkup(new[]
                       {
                        new [] { InlineKeyboardButton.WithCallbackData("Похудение", "Minus"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Поддержка формы", "Norm"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Набор веса", "Plus"),}
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
        public static InlineKeyboardMarkup keyboardSexRus = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Мужчина","Male"),
                                                    InlineKeyboardButton.WithCallbackData("Женщина","Female"),
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
        static void Main()
        {
            BotClient = new TelegramBotClient("884830807:AAEkTEJFBM-Tt8RQFuYLL3d6r10r0T3iaxk");


            BotClient.OnMessage += BotClient_OnMessage;
            //BotClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            BotClient.OnCallbackQuery += OnCallbackQuery_Lang;
            BotClient.OnCallbackQuery += OnCallbackQuery_Gender;
            BotClient.OnCallbackQuery += OnCallbackQuery_Activity;
            BotClient.OnCallbackQuery += OnCallbackQuery_Goal;
            BotClient.OnCallbackQuery += OnCallbackQuery_Lost;
            BotClient.OnCallbackQuery += OnCallbackQuery_MinPlus;



            BotClient.StartReceiving();

            Console.ReadKey();
        }

        private static async void OnCallbackQuery_Lang(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if (u.ChatId == e.CallbackQuery.From.Id.ToString())
                {
                    if ((callbackQuery.Data == "Russian" || callbackQuery.Data == "English") && u.Command == "/Language")
                    {
                        if (callbackQuery.Data == "Russian")
                        {
                            u.Language = "Russian"; u.Command = " ";
                            await BotClient.SendTextMessageAsync(
                                 callbackQuery.From.Id, $"Привет,я создан что бы помочь" +
                                 $" тебе считать твои калории, " +
                                 "терять или набирать вес)\n\n" +
                                 "Отправь команду:\n" +
                                 "/countcal - для подсчета количества калорий, которое нужно тебе каждый день для достяжение твоих целей\n");
                        }
                        else if (callbackQuery.Data == "English")
                        {
                            u.Language = "English"; u.Command = " ";
                            await BotClient.SendTextMessageAsync(
                                 callbackQuery.From.Id, $"Hi," +
                                 ", I can help you calculate your calories, " +
                                 "lose and put on weight)\n\n" +
                                 "Send me a command:\n" +
                                 "/countcal - to estimate the number of calories that you need to consume each day\n");
                        }
                    }
                }
            }
        }
        private static async void OnCallbackQuery_Gender(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if ((callbackQuery.Data == "Male" || callbackQuery.Data == "Female") && u.Command == "/Gender")
                {
                    if (callbackQuery.Data == "Male") { u.Male = true; }
                    else if (callbackQuery.Data == "Female") { u.Male = false; }
                    if (!edit)
                    {
                        u.Command = "/Age";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Введи свой возраст!");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Please,enter your age!");
                    }
                    else
                    {
                        u.Command = " "; edit = false;
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Пол успешно изменён!");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "The gender edited successfully!");
                    }
                }
            }
        }
        private static async void OnCallbackQuery_Goal(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if ((callbackQuery.Data == "Minus" || callbackQuery.Data == "Norm" || callbackQuery.Data == "Plus") && u.Command == "/Goal")
                {
                    double calories;
                    if (u.Male)
                    { calories = 88.363 + (13.397 * u.Weight) + (4.799 * u.Height) - (5.677 * u.Age); }
                    else
                    { calories = 447.593 + (9.247 * u.Weight) + (3.098 * u.Height) - (4.330 * u.Age); }
                    calories *= u.Activity;
                    if (callbackQuery.Data == "Minus")
                    {
                        if (u.BodyMassIndex < 17)
                        {
                            u.Command = "/Goal";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Ты и так худой(ая), куда еще худеть?\n" +
                                    "Я советую тебе набрать пару кг",
                                    replyMarkup: keyboardGoalRus);
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "You are slender\n" +
                                    "I advise you to put on a few pounds!",
                                    replyMarkup: keyboardGoal);
                        }
                        else if (u.BodyMassIndex >= 17 || u.BodyMassIndex < 24)
                        {
                            u.Command = "/Minus";
                            InlineKeyboardMarkup keyboardMinusDaNet = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                     {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Да","YesPlus"),
                                                    InlineKeyboardButton.WithCallbackData("Нет","NoMinus"),
                                          },
                                    });
                            InlineKeyboardMarkup keyboardMinusYesNo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                    {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Yes","YesPlus"),
                                                    InlineKeyboardButton.WithCallbackData("No","NoMinus"),
                                          },
                                   });
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе худеть," +
                                    " ведь твое тело в норме)\n" +
                                    "Хочешь ли ты поменять свою цель?",
                                    replyMarkup: keyboardMinusDaNet);
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against weight loss," +
                                    " your body is just fine\n" +
                                    "Do you want to change your goal?",
                                    replyMarkup: keyboardMinusYesNo);
                        }
                        else if (u.BodyMassIndex >= 24)
                        {
                            u.Goal = "Minus";
                            u.Command = "/Lost";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Выбери режим похудения\n" +
                                                                $"Безопасный(0.25 кг/нд)\n" +
                                                                $"Быстрый(0.5 кг/нд)\n" +
                                                                $"Екстремально быстрый(1 кг/нд)", replyMarkup: keyboardLossRus);
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"How you want to lose weight:\n" +
                                                                $"normaly(0.25 kg/week)\n" +
                                                                $"quickly(0.5 kg/week)\n" +
                                                                $"extremely(1 kg/week)", replyMarkup: keyboardLoss);
                        }
                    }
                    else if (callbackQuery.Data == "Norm")
                    {
                        u.Goal = "Norm"; u.Command = " ";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для поддержки веса тебе нужно " +
                                $"{(int)calories} калорий каждый день");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh, you need {(int)calories} calories per day");
                    }
                    else if (callbackQuery.Data == "Plus")
                    {
                        if (u.BodyMassIndex < 25)
                        {
                            u.Goal = "Plus";
                            calories *= 1.1;
                            u.Command = " ";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для набора веса тебе нужно " +
                                    $"{(int)calories} калорий каждый день");
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh,if you want to put weight, you should eat" +
                                    $" {(int)calories} calories per day");
                        }
                        else if (u.BodyMassIndex >= 25 && u.BodyMassIndex <= 35)
                        {
                            u.Command = "/Plus";
                            var keyboardDaNet = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                     {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Да","YesPlus"),
                                                    InlineKeyboardButton.WithCallbackData("Нет","NoPlus"),
                                          },
                                    });
                            var keyboardYesNo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                    {new [] {
                                                    InlineKeyboardButton.WithCallbackData("Yes","YesPlus"),
                                                    InlineKeyboardButton.WithCallbackData("No","NoPlus"),
                                          },
                                   });
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе набирать вес," +
                                    " ведь у тебя и так присутствует лишний вес\n" +
                                    "Хочешь ли ты поменять свою цель?",
                                    replyMarkup: keyboardDaNet);
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against putting on weight," +
                                    " because you are already overweight\n" +
                                    "Do you want to change your goal?",
                                    replyMarkup: keyboardYesNo);
                        }
                        else if (u.BodyMassIndex > 35)
                        {
                            u.Command = "/Goal";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "У тебя и так достаточно лишнего веса\n" +
                                    "Я советую тебе похудеть",
                                    replyMarkup: keyboardGoalRus);
                            else
                                await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "You are overweight\n" +
                                    "I advise you to lose weight!",
                                    replyMarkup: keyboardGoal);
                        }
                    }
                    u.Calories = (int)calories;
                    //   }
                }
            }
        }
        private static async void OnCallbackQuery_Activity(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if (u.Command == "/Activity")
                {
                    if (callbackQuery.Data == "BMR") { u.Activity = 1; }
                    else if (callbackQuery.Data == "Sedentary") { u.Activity = 1.1; }
                    else if (callbackQuery.Data == "Light") { u.Activity = 1.2375; }
                    else if (callbackQuery.Data == "Moderate") { u.Activity = 1.375; }
                    else if (callbackQuery.Data == "Active") { u.Activity = 1.43; }
                    else if (callbackQuery.Data == "VeryActive") { u.Activity = 1.55; }
                    else if (callbackQuery.Data == "ExtraActive") { u.Activity = 1.725; }
                    if (!edit)
                    {
                        u.Command = "/Goal";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
                                replyMarkup: keyboardGoalRus);
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
                                replyMarkup: keyboardGoal);
                    }
                    else
                    {
                        u.Command = " "; edit = false;
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Активность успешно изменёна!");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "The level of activity edited successfully!");
                    }
                }
            }
        }
        private static async void OnCallbackQuery_Lost(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if (u.Command == "/Lost")
                {
                    if (callbackQuery.Data == "normaly")
                    { u.Calories = (int)(u.Calories * 0.90); }
                    else if (callbackQuery.Data == "quickly")
                    { u.Calories = (int)(u.Calories * 0.75); }
                    else if (callbackQuery.Data == "extremely")
                    { u.Calories = (int)(u.Calories * 0.59); }
                    u.Command = " ";
                    if (u.Language == "Russian")
                    {
                        if (callbackQuery.Data == "normaly")
                        {
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для безопасной потери веса ты" +
                            $" должен(а) есть {u.Calories} калорий каждый день\n ");
                        }
                        else if (callbackQuery.Data == "quickly")
                        {
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для быстрой потери веса ты" +
                             $" должен(а) есть {u.Calories} калорий каждый день\n ");
                        }
                        else if (callbackQuery.Data == "extremely")
                        {
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для потери веса екстремально быстро ты" +
                            $" должен(а) есть {u.Calories} калорий каждый день\n ");
                        }
                    }
                    else
                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"For weight loss {callbackQuery.Data} you" +
                            $" should eat {u.Calories} calories per day\n ");
                }
            }
        }
        private static async void OnCallbackQuery_MinPlus(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            var callbackQuery = e.CallbackQuery;
            foreach (User u in users)
            {
                if (u.Command == "/Plus" || u.Command == "/Minus")
                {
                    if (callbackQuery.Data == "YesPlus")
                    {
                        u.Command = "/Goal";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
                                replyMarkup: keyboardGoalRus);
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
                                replyMarkup: keyboardGoal);
                    }
                    else if (callbackQuery.Data == "NoPlus")
                    {
                        u.Goal = "Plus";
                        u.Calories = (int)(u.Calories * 1.1);
                        u.Command = " ";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для набора веса тебе нужно " +
                                $"{u.Calories} калорий каждый день");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh,if you want to put weight, you should eat" +
                                $" {u.Calories} calories per day");
                    }
                    else if (callbackQuery.Data == "NoMinus")
                    {
                        u.Calories = (int)(u.Calories * 0.90); u.Command = " ";
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для безопасной потери веса ты" +
                            $" должен(а) есть {u.Calories} калорий каждый день\n ");
                        else
                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"For weight loss normaly you" +
                            $" should eat {u.Calories} calories per day\n ");
                    }
                }
            }
        }
        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;
            //Console.WriteLine("The " + e.Message.Chat.FirstName + " Write: " + e.Message.Text);
            //return;
            var users = DB.Users;
            var message = e.Message;
            foreach (User u in users)
            {
                if (u.ChatId == message.Chat.Id.ToString()) here = true;

            }
            if (!here)
            {
                DB.Users.Add(new User { ChatId = message.Chat.Id.ToString(), Name = message.Chat.FirstName });
                await DB.SaveChangesAsync();
            }
            users = DB.Users;
            foreach (User u in users)
            {
                if (u.ChatId == message.Chat.Id.ToString())
                {
                    switch (e.Message.Text)
                    {
                        case "/start":
                            InlineKeyboardMarkup keyboardHi = new InlineKeyboardMarkup(new[]
                        {
                        new [] { InlineKeyboardButton.WithCallbackData("English", "English"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Русский", "Russian"),}
                        });
                            u.Command = "/Language";
                            await BotClient.SendTextMessageAsync(message.Chat, $"Hi, {message.Chat.FirstName}, select the language you would like to use:\n" +
                                "Привет, выбери язык, который хотел бы использовать:", replyMarkup: keyboardHi); ;
                            break;
                        case "/countcal":
                            u.Command = "/Gender"; edit = false;
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, "Выбери свой пол", replyMarkup: keyboardSexRus);
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "Choose your gender", replyMarkup: keyboardSex);
                            break;
                        case "/information":
                            var keyboardInfo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {new [] {
                                                    InlineKeyboardButton.WithUrl("Telegram","https://t.me/agent_d"),
                                                    InlineKeyboardButton.WithUrl ("Instagram","https://www.instagram.com/danillll228/"),
                                          },
                                         });
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, "Nice to meet you)\n" +
                                "My name is Danil\n" +
                                "I`m the student of KPI and " +
                                "it`s my first project\n" +
                                "If you have complains or advice, you can write me there:", replyMarkup: keyboardInfo);
                            else await BotClient.SendTextMessageAsync(message.Chat, "Привет)\n" +
                                "Меня зовут Данил\n" +
                                "Я учусь в КПИ и " +
                                "это мой первый проект\n" +
                                "Если у тебя есть предложение или ты заметил какой-то баг,то напиши мне:", replyMarkup: keyboardInfo);
                            break;
                        case "/age":
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Введи свой возраст!");
                            else
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Please,enter your age!");
                            edit = true;
                            u.Command = "/Age"; break;
                        case "/weight":
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Введи свой вес(в кг)!");
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "Please enter your weight in kilograms");
                            edit = true;
                            u.Command = "/Weight"; break;
                        case "/height":
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Введи свой рост(в см)!");
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "Please enter your height in centimeters");
                            edit = true;
                            u.Command = "/Height"; break;
                        case "/activity":
                            edit = true; u.Command = "/Activity";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, "Как часто ты занимаешься спортом?",
                       replyMarkup: keyboardActRus);
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "How often do you training?",
                       replyMarkup: keyboardAct); break;
                        case "/gender":
                            edit = true; u.Command = "/Gender";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, "Выбери свой пол", replyMarkup: keyboardSexRus);
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "Choose your gender", replyMarkup: keyboardSex);
                            break;
                        case "/goal":
                            edit = true; u.Command = "/Goal";
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, "Какая у тебя цель?", replyMarkup: keyboardGoalRus);
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, "What is your goal?",
                         replyMarkup: keyboardGoal);
                            break;
                        case "/calculate_cal":
                            double calories;
                            if (u.Male)
                            { calories = 88.363 + (13.397 * u.Weight) + (4.799 * u.Height) - (5.677 * u.Age); }
                            else
                            { calories = 447.593 + (9.247 * u.Weight) + (3.098 * u.Height) - (4.330 * u.Age); }
                            calories *= u.Activity;
                            if (u.Goal == "Minus")
                            {
                                u.Command = "/Lost";
                                if (u.Language == "Russian")
                                    await BotClient.SendTextMessageAsync(message.Chat, $"Выбери режим похудения\n" +
                                                                    $"Безопасный(0.25 кг/нд)\n" +
                                                                    $"Быстрый(0.5 кг/нд)\n" +
                                                                    $"Екстремально быстрый(1 кг/нд)", replyMarkup: keyboardLossRus);
                                else
                                    await BotClient.SendTextMessageAsync(message.Chat, $"How you want to lose weight:\n" +
                                $"normaly(0.25 kg/week)\n" +
                                $"quickly(0.5 kg/week)\n" +
                                $"extremely(1 kg/week)", replyMarkup: keyboardLoss);
                            }
                            else if (u.Goal == "Norm")
                            {
                                if (u.Language == "Russian")
                                    await BotClient.SendTextMessageAsync(message.Chat, $"Что ж, для поддержки веса тебе нужно " +
                                        $"{(int)calories} калорий каждый день");
                                else
                                    await BotClient.SendTextMessageAsync(message.Chat, $"Oh, you need {(int)calories} calories per day");
                            }
                            else if (u.Goal == "Plus")
                            {
                                calories *= 1.1;
                                if (u.Language == "Russian")
                                    await BotClient.SendTextMessageAsync(message.Chat, $"Что ж, для набора веса тебе нужно " +
                                        $"{(int)calories} калорий каждый день");
                                else
                                    await BotClient.SendTextMessageAsync(message.Chat, $"Oh,if you want to put weight, you should eat" +
                                        $" {(int)calories} calories per day");
                            }
                            u.Calories = (int)calories;
                            break;
                        case "/about_me":
                            string gender, activity, goal;
                            if (u.Language == "Russian")
                            {
                                if (u.Male) gender = "парень";
                                else gender = "девушка";
                                switch (u.Activity)
                                {
                                    case 1: activity = "Ты просто сидишь дома)"; break;
                                    case 1.1: activity = "Ты не любишь спорт или просто ленивый(ая) :)"; break;
                                    case 1.2375: activity = "Ты занимаешься раз в неделю"; break;
                                    case 1.375: activity = "Ты занимаешься 2-3 в неделю"; break;
                                    case 1.43: activity = "Ты занимаешься 4-5 в неделю"; break;
                                    case 1.55: activity = "Ты прям любитель спорта\nЗанимаешь почти каждый день"; break;
                                    case 1.725: activity = "Ты прям спортивный псих)"; break;
                                    default:
                                        activity = "Прости, но ты не указал свою активность";
                                        break;
                                }
                                switch (u.Goal)
                                {
                                    case "Minus": goal = "похудеть"; break;
                                    case "Norm": goal = "остаться таким(ой) как есть"; break;
                                    case "Plus": goal = "набрать весу"; break;
                                    default:
                                        goal = "not entered"; break;
                                }
                                await BotClient.SendTextMessageAsync(message.Chat, $"Это вся информация, которую я имею про тебя:\n" +
                                    $"Тебя зовут {u.Name} и ты {gender}\n" +
                                    $"Тебе {u.Age} лет\n" +
                                    $"Твой вес - {u.Weight} kg\n" +
                                    $"Твой рост - {u.Height} cm\n" +
                                    $"{activity}\n" +
                                    $"Твоя цель это {goal}\n\n" +
                                    $"Что ж, если ты хочешь что-то изменить - используй команды:\n" +
                                    "/age - изменить возраст\n" +
                                     "/weight - изменить вес\n" +
                                     "/height - изменить рост\n" +
                                     "/activity - изменить активность\n" +
                                     "/gender - изменить пол\n" +
                                     "/goal - изменить цель");
                            }
                            else
                            {
                                if (u.Male) gender = "man";
                                else gender = "woman";
                                switch (u.Activity)
                                {
                                    case 1: activity = "You always sit at home)))"; break;
                                    case 1.1: activity = "You don`t like sport or you are lazy)"; break;
                                    case 1.2375: activity = "You do sport but only once at week"; break;
                                    case 1.375: activity = "You like sport and do some exercize"; break;
                                    case 1.43: activity = "You are extremely active person"; break;
                                    case 1.55: activity = "You are really into sport and you love it"; break;
                                    case 1.725: activity = "You are psycho which cannot live without sport"; break;
                                    default:
                                        activity = "Sorry, I haven`t your level of activity";
                                        break;
                                }
                                switch (u.Goal)
                                {
                                    case "Minus": goal = "weight loss"; break;
                                    case "Norm": goal = "keeping fit"; break;
                                    case "Plus": goal = "putting on weight"; break;
                                    default:
                                        goal = "not entered"; break;
                                }
                                await BotClient.SendTextMessageAsync(message.Chat, $"It`s all information about you, that I know:\n" +
                                    $"Your name is {u.Name} and you are {gender}\n" +
                                    $"You are {u.Age} years old\n" +
                                    $"You weigh {u.Weight} kg\n" +
                                    $"Your height is {u.Height} cm\n" +
                                    $"{activity}\n" +
                                    $"And your goal is {goal}\n\n" +
                                    $"So, if you want to edit something you should use commands:\n" +
                                    "/age - to edit or enter your age\n" +
                                     "/weight - to edit or enter your weight\n" +
                                     "/height - to edit or enter your height\n" +
                                     "/activity - to edit or enter your level of activity\n" +
                                     "/gender - to edit or enter your sex\n" +
                                     "/goal - to change goal");
                            }
                            break;
                        case "/delete":
                            u.Activity = 0; u.Age = 0; u.Calories = 0; u.Command = null; u.Goal = null; u.Height = 0; u.Weight = 0;
                            if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat, $"Вся информация удалена\n" +
                                       $"Напиши команду /countcal что бы ввести новую инфу и посчитать каллории");
                            else
                                await BotClient.SendTextMessageAsync(message.Chat, $"All information was removed\n" +
                                            $"Write a command /countcal to enter new info and count calories"); break;
                        default:
                            int age;
                            float weight, height;
                            message.Text = message.Text.Replace(".", ",");
                            if (u.Command == "/Age" && int.TryParse(message.Text, out age))
                            {
                                if (age <= 5 || age > 95)
                                {
                                    if (u.Language == "Russian")
                                        await BotClient.SendTextMessageAsync(message.Chat, $"Тебе не может быть {age} лет\n" +
                                        $"Пожалуйста, подумай и введи еще раз)");
                                    else
                                        await BotClient.SendTextMessageAsync(message.Chat, $"You cannot be {age} years old\n" +
                                            $"Please, enter your age again");
                                }
                                else
                                {
                                    u.Age = age;
                                    if (!edit)
                                    {
                                        u.Command = "/Weight";
                                        if (u.Language == "Russian")
                                            await BotClient.SendTextMessageAsync(message.Chat.Id, "Введи свой вес(в кг)!");
                                        else
                                            await BotClient.SendTextMessageAsync(message.Chat, "Please enter your weight in kilograms");
                                    }
                                    else
                                    {
                                        u.Command = " "; edit = false;
                                        if (u.Language == "Russian")
                                            await BotClient.SendTextMessageAsync(message.Chat.Id, "Возраст успешно изменён!");
                                        else
                                            await BotClient.SendTextMessageAsync(message.Chat.Id, "The age edited successfully!");
                                    }
                                }
                            }
                            else if (u.Command == "/Weight" && float.TryParse(message.Text, out weight))
                            {
                                if (weight <= 5 || weight > 200)
                                {
                                    if (u.Language == "Russian")
                                        await BotClient.SendTextMessageAsync(message.Chat, $"Ты не можешь весить {weight} кг\n" +
                                        $"Пожалуйста, подумай и введи еще раз)");
                                    else
                                        await BotClient.SendTextMessageAsync(message.Chat, $"You cannot weigh {weight} kg\n" +
                                        $"Please, enter your weight again");
                                }
                                else
                                {
                                    if (!edit)
                                    {
                                        u.Weight = weight;
                                        u.Command = "/Height";
                                        if (u.Language == "Russian")
                                            await BotClient.SendTextMessageAsync(message.Chat.Id, "Введи свой рост(в см)!");
                                        else
                                            await BotClient.SendTextMessageAsync(message.Chat, "Please enter your height in centimeters");
                                    }
                                    else
                                    {
                                        u.Command = " "; edit = false;
                                        if (weight >= u.Weight)
                                        {
                                            if (u.Weight == 0 || weight == u.Weight)
                                            {
                                                if (u.Language == "Russian")
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Вес успешно изменён! ");
                                                else
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"The weight edited successfully! ");
                                            }
                                            else
                                                if (u.Goal == "Minus" || u.Goal == "Norm")
                                            {
                                                if (u.Language == "Russian")
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Ох, ты набрал(а) {weight - u.Weight} кг ");
                                                else
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Oy, you put on {weight - u.Weight} kg ");
                                            }
                                            else if (u.Goal == "Plus")
                                            {
                                                if (u.Language == "Russian")
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Оу, ты набрал(а) {weight - u.Weight} кг\n" +
                                                        $"Поздравляю, продолжай в этом же духе)");
                                                else
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Oy, you put on {weight - u.Weight} kg \n" +
                                                        $"Good, keep it up!)");
                                            }
                                        }
                                        else if (weight < u.Weight)
                                        {
                                            if (u.Goal == "Minus")
                                            {
                                                if (u.Language == "Russian")
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Оy, ты похудел(а) на {u.Weight - weight} кг \n" +
                                                        $"Поздравляю, продолжай в этом же духе)");
                                                else
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Good, you lost {u.Weight - weight} kg \n" +
                                                        $"Good, keep it up!)");
                                            }
                                            else if (u.Goal == "Plus" || u.Goal == "Norm")
                                            {
                                                if (u.Language == "Russian")
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Оу, ты похудел(а) на {u.Weight - weight} кг");
                                                else
                                                    await BotClient.SendTextMessageAsync(message.Chat.Id, $"Oy, you lost {u.Weight - weight} kg ");
                                            }
                                        }
                                        u.Weight = weight;
                                    }
                                }
                            }
                            else if (u.Command == "/Height" && float.TryParse(message.Text, out height))
                            {
                                if (height <= 30 || height > 250)
                                {
                                    if (u.Language == "Russian")
                                        await BotClient.SendTextMessageAsync(message.Chat, $"Твой рост не может быть {height} см\n" +
                                        $"Пожалуйста, подумай и введи еще раз)");
                                    else
                                        await BotClient.SendTextMessageAsync(message.Chat, $"Your height cannot be {height} cm\n" +
                                        $"Please, enter your height again");
                                }
                                else
                                {
                                    u.Height = height;
                                    u.BodyMassIndex = (10000 * u.Weight) / (u.Height * u.Height);
                                    if (u.BodyMassIndex < 13 || u.BodyMassIndex > 50)
                                    {
                                        if (!edit)
                                        {
                                            u.Command = "/Weight";
                                            if (u.Language == "Russian")
                                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Твой индекс массы тела невозможен\n" +
                                                    "Введи свой вес(в кг) снова!");
                                            else
                                                await BotClient.SendTextMessageAsync(message.Chat, "Your body mass index is impossible \n" +
                                                    "Please enter your weight (in kilograms) again!");
                                        }
                                        else
                                        {
                                            if (u.Language == "Russian")
                                                await BotClient.SendTextMessageAsync(message.Chat, $"Твой рост не может быть {height} см," +
                                                    $" потому что твой индекс массы тела невозможен\n" +
                                                $"Пожалуйста, подумай и введи еще раз)");
                                            else
                                                await BotClient.SendTextMessageAsync(message.Chat, $"Your height cannot be {height} cm," +
                                                    $"because your body mass index is impossible\n" +
                                                $"Please, enter your height again");
                                            u.Command = "/Height";
                                        }
                                    }
                                    else
                                    {
                                        if (!edit)
                                        {
                                            u.Command = "/Activity";
                                            if (u.Language == "Russian")
                                                await BotClient.SendTextMessageAsync(message.Chat, "Как часто ты занимаешься спортом?",
                                                    replyMarkup: keyboardActRus);
                                            else
                                                await BotClient.SendTextMessageAsync(message.Chat, "How often do you training?",
                                                    replyMarkup: keyboardAct); break;
                                        }
                                        else
                                        {
                                            u.Command = " "; edit = false;
                                            if (u.Language == "Russian")
                                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Рост успешно изменён!");
                                            else
                                                await BotClient.SendTextMessageAsync(message.Chat.Id, "The height edited successfully!");
                                        }
                                    }
                                }
                            }
                            else
                                if (u.Language == "Russian")
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "Я тебя не понимаю!");
                            else
                                await BotClient.SendTextMessageAsync(message.Chat.Id, "I don`t understand you!");
                            break;
                    }
                    Console.WriteLine("The " + message.Chat.FirstName + " Write: " + message.Text);
                }
            }
            DB.SaveChanges();
        }
        //private static async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        //{
        //    var users = DB.Users;
        //    var callbackQuery = e.CallbackQuery;
        //    foreach (User u in users)
        //    {
        //        if (u.ChatId == e.CallbackQuery.From.Id.ToString())
        //        {
        //            if ((callbackQuery.Data == "Russian" || callbackQuery.Data == "English") && u.Command == "/Language")
        //            {
        //                if (callbackQuery.Data == "Russian")
        //                {
        //                    u.Language = "Russian"; u.Command = " ";
        //                    await BotClient.SendTextMessageAsync(
        //                         callbackQuery.From.Id, $"Привет,я создан что бы помочь" +
        //                         $" тебе считать твои калории, " +
        //                         "терять или набирать вес)\n\n" +
        //                         "Отправь команду:\n" +
        //                         "/countcal - для подсчета количества калорий, которое нужно тебе каждый день для достяжение твоих целей\n");
        //                }
        //                else if (callbackQuery.Data == "English")
        //                {
        //                    u.Language = "English"; u.Command = " ";
        //                    await BotClient.SendTextMessageAsync(
        //                         callbackQuery.From.Id, $"Hi," +
        //                         ", I can help you calculate your calories, " +
        //                         "lose and put on weight)\n\n" +
        //                         "Send me a command:\n" +
        //                         "/countcal - to estimate the number of calories that you need to consume each day\n");
        //                }
        //            }
        //            else if ((callbackQuery.Data == "Male" || callbackQuery.Data == "Female") && u.Command == "/Gender")
        //            {
        //                if (callbackQuery.Data == "Male") { u.Male = true; }
        //                else if (callbackQuery.Data == "Female") { u.Male = false; }
        //                if (!edit)
        //                {
        //                    u.Command = "/Age";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Введи свой возраст!");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Please,enter your age!");
        //                }
        //                else
        //                {
        //                    u.Command = " "; edit = false;
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Пол успешно изменён!");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "The gender edited successfully!");
        //                }
        //            }
        //            else if ((callbackQuery.Data == "Minus" || callbackQuery.Data == "Norm" || callbackQuery.Data == "Plus") && u.Command == "/Goal")
        //            {
        //                double calories;
        //                if (u.Male)
        //                { calories = 88.363 + (13.397 * u.Weight) + (4.799 * u.Height) - (5.677 * u.Age); }
        //                else
        //                { calories = 447.593 + (9.247 * u.Weight) + (3.098 * u.Height) - (4.330 * u.Age); }
        //                calories *= u.Activity;
        //                if (callbackQuery.Data == "Minus")
        //                {
        //                    if (u.BodyMassIndex < 17)
        //                    {
        //                        u.Command = "/Goal";
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Ты и так худой(ая), куда еще худеть?\n" +
        //                                "Я советую тебе набрать пару кг",
        //                                replyMarkup: keyboardGoalRus);
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "You are slender\n" +
        //                                "I advise you to put on a few pounds!",
        //                                replyMarkup: keyboardGoal);
        //                    }
        //                    else if (u.BodyMassIndex >= 17 || u.BodyMassIndex < 24)
        //                    {
        //                        u.Command = "/Minus";
        //                        InlineKeyboardMarkup keyboardMinusDaNet = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        //                                 {new [] {
        //                                            InlineKeyboardButton.WithCallbackData("Да","YesPlus"),
        //                                            InlineKeyboardButton.WithCallbackData("Нет","NoMinus"),
        //                                  },
        //                                });
        //                        InlineKeyboardMarkup keyboardMinusYesNo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        //                                {new [] {
        //                                            InlineKeyboardButton.WithCallbackData("Yes","YesPlus"),
        //                                            InlineKeyboardButton.WithCallbackData("No","NoMinus"),
        //                                  },
        //                               });
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе худеть," +
        //                                " ведь твое тело в норме)\n" +
        //                                "Хочешь ли ты поменять свою цель?",
        //                                replyMarkup: keyboardMinusDaNet);
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against weight loss," +
        //                                " your body is just fine\n" +
        //                                "Do you want to change your goal?",
        //                                replyMarkup: keyboardMinusYesNo);
        //                    }
        //                    else if (u.BodyMassIndex >= 24)
        //                    {
        //                        u.Goal = "Minus";
        //                        u.Command = "/Lost";
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Выбери режим похудения\n" +
        //                                                            $"Безопасный(0.25 кг/нд)\n" +
        //                                                            $"Быстрый(0.5 кг/нд)\n" +
        //                                                            $"Екстремально быстрый(1 кг/нд)", replyMarkup: keyboardLossRus);
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"How you want to lose weight:\n" +
        //                                                            $"normaly(0.25 kg/week)\n" +
        //                                                            $"quickly(0.5 kg/week)\n" +
        //                                                            $"extremely(1 kg/week)", replyMarkup: keyboardLoss);
        //                    }
        //                }
        //                else if (callbackQuery.Data == "Norm")
        //                {
        //                    u.Goal = "Norm"; u.Command = " ";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для поддержки веса тебе нужно " +
        //                            $"{(int)calories} калорий каждый день");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh, you need {(int)calories} calories per day");
        //                }
        //                else if (callbackQuery.Data == "Plus")
        //                {
        //                    if (u.BodyMassIndex < 25)
        //                    {
        //                        u.Goal = "Plus";
        //                        calories *= 1.1;
        //                        u.Command = " ";
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для набора веса тебе нужно " +
        //                                $"{(int)calories} калорий каждый день");
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh,if you want to put weight, you should eat" +
        //                                $" {(int)calories} calories per day");
        //                    }
        //                    else if (u.BodyMassIndex >= 25 && u.BodyMassIndex <= 35)
        //                    {
        //                        u.Command = "/Plus";
        //                        var keyboardDaNet = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        //                                 {new [] {
        //                                            InlineKeyboardButton.WithCallbackData("Да","YesPlus"),
        //                                            InlineKeyboardButton.WithCallbackData("Нет","NoPlus"),
        //                                  },
        //                                });
        //                        var keyboardYesNo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        //                                {new [] {
        //                                            InlineKeyboardButton.WithCallbackData("Yes","YesPlus"),
        //                                            InlineKeyboardButton.WithCallbackData("No","NoPlus"),
        //                                  },
        //                               });
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Я бы не советовал тебе набирать вес," +
        //                                " ведь у тебя и так присутствует лишний вес\n" +
        //                                "Хочешь ли ты поменять свою цель?",
        //                                replyMarkup: keyboardDaNet);
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "I would advise against putting on weight," +
        //                                " because you are already overweight\n" +
        //                                "Do you want to change your goal?",
        //                                replyMarkup: keyboardYesNo);
        //                    }
        //                    else if (u.BodyMassIndex > 35)
        //                    {
        //                        u.Command = "/Goal";
        //                        if (u.Language == "Russian")
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "У тебя и так достаточно лишнего веса\n" +
        //                                "Я советую тебе похудеть",
        //                                replyMarkup: keyboardGoalRus);
        //                        else
        //                            await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "You are overweight\n" +
        //                                "I advise you to lose weight!",
        //                                replyMarkup: keyboardGoal);
        //                    }
        //                }
        //                u.Calories = (int)calories;
        //                //   }
        //            }
        //            else if (u.Command == "/Activity")
        //            {
        //                if (callbackQuery.Data == "BMR") { u.Activity = 1; }
        //                else if (callbackQuery.Data == "Sedentary") { u.Activity = 1.1; }
        //                else if (callbackQuery.Data == "Light") { u.Activity = 1.2375; }
        //                else if (callbackQuery.Data == "Moderate") { u.Activity = 1.375; }
        //                else if (callbackQuery.Data == "Active") { u.Activity = 1.43; }
        //                else if (callbackQuery.Data == "VeryActive") { u.Activity = 1.55; }
        //                else if (callbackQuery.Data == "ExtraActive") { u.Activity = 1.725; }
        //                if (!edit)
        //                {
        //                    u.Command = "/Goal";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
        //                            replyMarkup: keyboardGoalRus);
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
        //                            replyMarkup: keyboardGoal);
        //                }
        //                else
        //                {
        //                    u.Command = " "; edit = false;
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Активность успешно изменёна!");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "The level of activity edited successfully!");
        //                }
        //            }
        //            else if (u.Command == "/Lost")
        //            {
        //                if (callbackQuery.Data == "normaly")
        //                { u.Calories = (int)(u.Calories * 0.90); }
        //                else if (callbackQuery.Data == "quickly")
        //                { u.Calories = (int)(u.Calories * 0.75); }
        //                else if (callbackQuery.Data == "extremely")
        //                { u.Calories = (int)(u.Calories * 0.59); }
        //                u.Command = " ";
        //                if (u.Language == "Russian")
        //                {
        //                    if (callbackQuery.Data == "normaly")
        //                    {
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для безопасной потери веса ты" +
        //                        $" должен(а) есть {u.Calories} калорий каждый день\n ");
        //                    }
        //                    else if (callbackQuery.Data == "quickly")
        //                    {
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для быстрой потери веса ты" +
        //                         $" должен(а) есть {u.Calories} калорий каждый день\n ");
        //                    }
        //                    else if (callbackQuery.Data == "extremely")
        //                    {
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для потери веса екстремально быстро ты" +
        //                        $" должен(а) есть {u.Calories} калорий каждый день\n ");
        //                    }
        //                }
        //                else
        //                    await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"For weight loss {callbackQuery.Data} you" +
        //                        $" should eat {u.Calories} calories per day\n ");
        //            }
        //            else if (u.Command == "/Plus" || u.Command == "/Minus")
        //            {
        //                if (callbackQuery.Data == "YesPlus")
        //                {
        //                    u.Command = "/Goal";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "Какая у тебя цель?",
        //                            replyMarkup: keyboardGoalRus);
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, "What is your goal?",
        //                            replyMarkup: keyboardGoal);
        //                }
        //                else if (callbackQuery.Data == "NoPlus")
        //                {
        //                    u.Goal = "Plus";
        //                    u.Calories = (int)(u.Calories * 1.1);
        //                    u.Command = " ";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Что ж, для набора веса тебе нужно " +
        //                            $"{u.Calories} калорий каждый день");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Oh,if you want to put weight, you should eat" +
        //                            $" {u.Calories} calories per day");
        //                }
        //                else if (callbackQuery.Data == "NoMinus")
        //                {
        //                    u.Calories = (int)(u.Calories * 0.90); u.Command = " ";
        //                    if (u.Language == "Russian")
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Для безопасной потери веса ты" +
        //                        $" должен(а) есть {u.Calories} калорий каждый день\n ");
        //                    else
        //                        await BotClient.SendTextMessageAsync(callbackQuery.From.Id, $"For weight loss normaly you" +
        //                        $" should eat {u.Calories} calories per day\n ");
        //                }
        //            }
        //        }
        //    }
        //    DB.SaveChanges();
        //}
    }
}
