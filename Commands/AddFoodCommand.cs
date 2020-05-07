using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class AddFoodCommand : Command
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
        public override string Name => @"/addfood";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            try
            {
                string[] foodinfo = message.Text.Substring(9).Split(' ');
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

                user.foodname = GetFood($"https://myownapik.azurewebsites.net/translate/{user.foodname}");
                JObject search = JObject.Parse(GetFood($"https://myownapik.azurewebsites.net/food/{user.foodname}"));
                user.food = JsonConvert.DeserializeObject<Food>(search.ToString());
                user.Command = "SELECTFOOD";
                user.Multiply(user.weightoffood);
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                        $"Калорийность - {user.food.nutrients.ENERC_KCAL}\n" +
                        $"из которых:\n" +
                        $"Белки - {user.food.nutrients.PROCNT}\n" +
                        $"Жири - {user.food.nutrients.FAT}\n" +
                        $"Углеводы - {user.food.nutrients.CHOCDF}\n",replyMarkup: SelectOrNoRus);
                else
                    await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                        $"Calories - {user.food.nutrients.ENERC_KCAL}\n" +
                        $"of which:\n" +
                        $"Protein - {user.food.nutrients.PROCNT}\n" +
                        $"Fat - {user.food.nutrients.FAT}\n" +
                        $"Carbohydrates - {user.food.nutrients.CHOCDF}\n", replyMarkup: SelectOrNoEng);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                user.Command = "TextOfFood";
                if (user.Language == "Russian")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Напиши пожалуйста название и вес в граммах продукта");
                }
                else await client.SendTextMessageAsync(message.Chat.Id, "Please, write a name and weight in grams of food");
                return;
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
