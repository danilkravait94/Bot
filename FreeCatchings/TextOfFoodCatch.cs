using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{
    class TextOfFoodCatch : FreeCatching
    {
        public override string Name => "TextOfFood";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            try
            {
                string[] foodinfo = message.Text.Split(' ');
            user.foodname = string.Empty;
            user.weightoffood = 0;
            bool add = false;
            foreach (string s in foodinfo)
            {
                if (double.TryParse(s, out user.weightoffood)) { user.weightoffood /= 100; }
                else
                {
                    if (user.foodname.Length == 0) { user.foodname = s; }
                    else { if (!add) { user.foodname += "&" + s; add = true; } }
                }
            }
            if (user.weightoffood == 0)
            {
                user.Command = "WeightOFFOOD";
                if (user.Language == "Russian")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Напиши пожалуйста вес введенного продукта");
                }
                else await client.SendTextMessageAsync(message.Chat.Id, "Please, write a weight of added food in grams");
                return;
            }
            if (user.foodname == string.Empty)
            {
                user.Command = "nameTOFFOOD";
                if (user.Language == "Russian")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Напиши пожалуйста название введенного продукта");
                }
                else await client.SendTextMessageAsync(message.Chat.Id, "Please, write a name of added food");
                return;
            }

            user.foodname = await GetFood($"https://myownapik.azurewebsites.net/translate/{user.foodname}");
            JObject search = JObject.Parse(await GetFood($"https://myownapik.azurewebsites.net/food/{user.foodname}"));
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
