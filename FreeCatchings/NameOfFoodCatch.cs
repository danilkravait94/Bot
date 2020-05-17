using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{
    class NameOfFoodCatch : FreeCatching
    {
        public override string Name => "nameTOFFOOD";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            try
            {
                user.foodname = string.Empty;
            bool add=false;
            foreach (string s in message.Text.Split(' '))
            {
                    if (user.foodname.Length == 0) { user.foodname = s; }
                    else { if (!add) { user.foodname += "&" + s; add = true; } }
            }

            user.foodname = GetFood($"https://myownapik.azurewebsites.net/translate/{user.foodname}").ToString();
            JObject search = JObject.Parse(GetFood($"https://myownapik.azurewebsites.net/food/{user.foodname}").ToString());
            user.food = JsonConvert.DeserializeObject<Food>(search.ToString());
            user.Command = "SELECTFOOD";
            user.Multiply(user.weightoffood);
            SendMessageFood(message, client, user.Id);
            }
            catch (WebException e)
            {
                using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
                {
                    sw.WriteLine($"{e.Message} by {user.Name}({user.ChatId}");
                }
                user.Command = "TextOfFood";
                if (user.Language == "Russian")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Прости,но я пробежался по всем базам данных и \nЯ не смог найти этот продукт");
                    await client.SendTextMessageAsync(message.Chat.Id, "Напиши пожалуйста название и вес в граммах другого продукта");
                }
                else
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Sorry, i looked at all data bases \nbut I cannot find this food");
                    await client.SendTextMessageAsync(message.Chat.Id, "Please, write a name and weight in grams of another food");
                }
                return;
            }
        }
    }
}
