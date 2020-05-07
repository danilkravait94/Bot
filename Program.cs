using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Calories.Commands;
using Calories.CallBacks;
using Calories.FreeCatchings;

namespace Calories
{
    class Program:Class_for_DB
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
            BotClient.StartReceiving();
            Console.ReadKey();
        }
        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;
            Console.WriteLine("The " + e.Message.Chat.FirstName + " Write: " + e.Message.Text);
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
            await DB.SaveChangesAsync();
        }
        private static async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine("The " + e.CallbackQuery.From.FirstName + " Write: " + e.CallbackQuery.Data);
            DB.SaveChanges();
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
            await DB.SaveChangesAsync();
        }
        
    }
}
