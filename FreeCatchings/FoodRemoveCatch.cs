using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{
    class FoodRemoveCatch : FreeCatching
    {
        public  override string Name => "FoodRemove";

        public async override void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            
            if (int.TryParse(message.Text, out int number))
            {
                try
                {
                    user.PlusToDay(user.DayFoodList[number]);
                    user.DayFoodList.RemoveAt(number);
                    user.Command = " ";
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat.Id, "Продукт удален");
                    else
                        await client.SendTextMessageAsync(message.Chat.Id, "The food has been removed");
                }
                catch(Exception e)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
                    {
                        sw.WriteLine($"{e.Message} by {user.Name}({user.ChatId}");
                    }
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat.Id, "Неправильно введен номер продукта");
                    else
                        await client.SendTextMessageAsync(message.Chat.Id, "Error with input of number of food");
                }
            }
            else
            {
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(message.Chat.Id, "Неправильно введен номер продукта");
                else
                    await client.SendTextMessageAsync(message.Chat.Id, "Error with input of number of food");
            }
        }
    }
}
