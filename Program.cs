using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Calories.Commands;
using Calories.CallBacks;
using Calories.FreeCatchings;
using Calories.QuartzInfo;
using System.IO;

namespace Calories
{
    class Program : Class_for_DB
    {
        private static TelegramBotClient BotClient;
        public static IReadOnlyList<Command> commands;
        public static IReadOnlyList<CallBack> callbacks;
        public static IReadOnlyList<FreeCatching> freeCatchings;
        private static bool here = false, did;
        //public static UserContext DB = new UserContext();
        static void Main()
        {

            BotClient = Bot.Get();
            commands = Bot.Commands;
            callbacks = Bot.CallBacks;
            freeCatchings = Bot.FreeCatchings;

            BotClient.OnMessage += BotClient_OnMessage;
            BotClient.OnMessageEdited += BotClient_OnMessage;
            BotClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            StartSending.Start();
            BotClient.StartReceiving();
            Console.ReadKey();
        }
        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;
            //return;
            //if (e.Message.Chat.Id != 386219611) return;

            var users = DB.Users;
            var message = e.Message;

            foreach (User u in users) { if (u.ChatId == message.Chat.Id.ToString()) here = true; }

            if (!here)
            {
                DB.Users.Add(new User { ChatId = message.Chat.Id.ToString(), Name = message.Chat.FirstName, Command="LANGUAGE" });
                
                await DB.SaveChangesAsync();
                users = DB.Users;
            }
            did = false;
            foreach (User u in users)
            {
                if (u.ChatId == message.Chat.Id.ToString())
                {
                    foreach (var command in commands)
                    {
                        if (command.Contains(message.Text))
                        {
                            did = true;
                            command.Execute(message, BotClient, u.Id);
                            break;
                        }
                    }
                    foreach (var freecatch in freeCatchings)
                    {
                        if (!did & freecatch.Contains(u.Command))
                        {
                            did = true;
                            freecatch.Execute(message, BotClient, u.Id);
                            break;
                        }
                    }
                    if (!did)
                    {
                        if (u.Language == "Russian")
                            await BotClient.SendTextMessageAsync(message.Chat.Id, "Я тебя не понимаю!");
                        else
                            await BotClient.SendTextMessageAsync(message.Chat.Id, "I don`t understand you!");
                    }
                }
            }
            using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
            {
                sw.WriteLine($"{DateTime.Now} the {message.Chat.FirstName}({message.Chat.Id}) Write: {e.Message.Text}");
            }
            await DB.SaveChangesAsync();

        }
        private static async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            foreach (User u in users)
            {
                if (u.ChatId == e.CallbackQuery.From.Id.ToString())
                {
                    foreach (var callback in callbacks)
                    {
                        if (callback.Contains(u.Command))
                        {
                            callback.Execute(e, BotClient,u.Id);
                            break;
                        }
                    }
                }
            }
            using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
            {
                sw.WriteLine($"{DateTime.Now} the {e.CallbackQuery.From.FirstName}({e.CallbackQuery.From.Id}) Select: {e.CallbackQuery.Data}");
            }
            await DB.SaveChangesAsync();
        }
        
    }
}
