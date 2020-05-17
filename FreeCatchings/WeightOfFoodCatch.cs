using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{
    class WeightOfFoodCatch : FreeCatching
    {
        public override string Name => "WeightOFFOOD";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            try
            {
                if (float.TryParse(message.Text.Replace('.', ','), out float weightoffood))
                {
                    weightoffood /= 100;
                    if (weightoffood <= 0)
                    {
                        if (user.Language == "Russian")
                            await client.SendTextMessageAsync(message.Chat, $"Продукт не может весить {weightoffood * 100} лет\n" +
                            $"Пожалуйста, подумай и введи еще раз)");
                        else
                            await client.SendTextMessageAsync(message.Chat, $"The product cannot weigh {weightoffood * 100} grams\n" +
                                $"Please, enter weight of food again");
                    }
                    else
                    {
                        user.weightoffood = weightoffood;

                        user.foodname = await GetFood($"https://myownapik.azurewebsites.net/translate/{user.foodname}");
                        JObject search = JObject.Parse(await GetFood($"https://myownapik.azurewebsites.net/food/{user.foodname}"));
                        user.food = JsonConvert.DeserializeObject<Food>(search.ToString());
                        user.Command = "SELECTFOOD";
                        user.Multiply(user.weightoffood);
                        SendMessageFood(message, client, user.Id);
                    }
                }
                else
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat.Id, "Неправильно введен вес");
                    else
                        await client.SendTextMessageAsync(message.Chat.Id, "Error with input of weight");
                }
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
