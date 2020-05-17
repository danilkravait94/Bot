using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{

    public class WeightCatch : FreeCatching
    {
        public override string Name => "WEIGHT";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            if (float.TryParse(message.Text.Replace('.',','), out float weight))
            {
                if (weight <= 5 || weight > 200)
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat, $"Ты не можешь весить {weight} кг\n" +
                        $"Пожалуйста, подумай и введи еще раз)");
                    else
                        await client.SendTextMessageAsync(message.Chat, $"You cannot weigh {weight} kg\n" +
                        $"Please, enter your weight again");
                }
                else
                {
                    if (!user.edit)
                    {
                        user.Weight = weight;
                        user.Command = "HEIGHT";
                        if (user.Language == "Russian")
                            await client.SendTextMessageAsync(message.Chat.Id, "Введи свой рост(в см)!");
                        else
                            await client.SendTextMessageAsync(message.Chat, "Please enter your height in centimeters");
                    }
                    else
                    {
                        if (weight >= user.Weight)
                        {
                            if (user.Weight == 0 || weight == user.Weight)
                            {
                                if (user.Language == "Russian")
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Вес успешно изменён! ");
                                else
                                    await client.SendTextMessageAsync(message.Chat.Id, $"The weight edited successfully! ");
                            }
                            else
                                if (user.Goal == "Minus" ||
                                user.Goal == "Norm" ||   user.Goal == "normaly" ||
                                user.Goal == "quickly" || user.Goal == "extremely")
                            {
                                if (user.Language == "Russian")
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Ох, ты набрал(а) {(user.Weight - weight):0.00} кг ");
                                else
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Oy, you put on {(user.Weight - weight):0.00} kg ");
                            }
                            else if (user.Goal == "Plus")
                            {
                                if (user.Language == "Russian")
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Оу, ты набрал(а) {(user.Weight - weight):0.00} кг\n" +
                                        $"Поздравляю, продолжай в этом же духе)");
                                else
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Oy, you put on {(user.Weight - weight):0.00} kg \n" +
                                        $"Good, keep it up!)");
                            }
                        }
                        else if (weight < user.Weight)
                        {
                            if (user.Goal == "Minus" || user.Goal == "normaly" ||
                                user.Goal == "quickly" || user.Goal == "extremely")
                            {
                                if (user.Language == "Russian")
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Оy, ты похудел(а) на {(user.Weight - weight):0.00} кг \n" +
                                        $"Поздравляю, продолжай в этом же духе)");
                                else
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Good, you lost {(user.Weight - weight):0.00} kg \n" +
                                        $"Good, keep it up!)");
                            }
                            else if (user.Goal == "Plus" || user.Goal == "Norm")
                            {
                                if (user.Language == "Russian")
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Оу, ты похудел(а) на {(user.Weight - weight):0.00} кг");
                                else
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Oy, you lost {(user.Weight - weight):0.00} kg ");
                            }
                        }
                        user.Command = " "; user.edit = false;
                        user.Weight = weight;
                    }
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
    }
}
