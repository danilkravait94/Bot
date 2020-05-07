using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.FreeCatchings
{
    class NameOfFoodCatch : FreeCatching
    {
        public static InlineKeyboardMarkup SelectOrNoRus = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        {
            new [] {InlineKeyboardButton.WithCallbackData("Добавить продукт","Yes"),},
            new [] {InlineKeyboardButton.WithCallbackData("Перезадать продукт","No"), }
        });
        public static InlineKeyboardMarkup SelectOrNoEng = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
         {
             new [] { InlineKeyboardButton.WithCallbackData("Add product","Yes"), },
             new [] { InlineKeyboardButton.WithCallbackData("Reset info about product","No") }
        });
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

            user.foodname = GetFood($"https://myownapik.azurewebsites.net/translate/{user.foodname}");
            JObject search = JObject.Parse(GetFood($"https://myownapik.azurewebsites.net/food/{user.foodname}"));
            user.food = JsonConvert.DeserializeObject<Food>(search.ToString());
            user.Command = "SELECTFOOD";
            user.Multiply(user.weightoffood);
            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                    $"Калорийность - {user.food.nutrients.ENERC_KCAL}\n" +
                    $"Белки - {user.food.nutrients.PROCNT}\n" +
                    $"Жири - {user.food.nutrients.FAT}\n" +
                    $"Углеводы - {user.food.nutrients.CHOCDF}\n", replyMarkup: SelectOrNoRus);
            else
                await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                    $"Calories - {user.food.nutrients.ENERC_KCAL}\n" +
                    $"Protein - {user.food.nutrients.PROCNT}\n" +
                    $"Fat - {user.food.nutrients.FAT}\n" +
                    $"Carbohydrates - {user.food.nutrients.CHOCDF}\n", replyMarkup: SelectOrNoEng);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
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
