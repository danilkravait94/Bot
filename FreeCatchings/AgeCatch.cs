using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.FreeCatchings
{

    public class AgeCatch : FreeCatching
    {
        public override string Name => "AGEINPUT";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            if (float.TryParse(message.Text, out float agefloat))
            {
                int age = (int)agefloat;
                if (age <= 5 || age > 95)
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(message.Chat, $"Тебе не может быть {age} лет\n" +
                        $"Пожалуйста, подумай и введи еще раз)");
                    else
                        await client.SendTextMessageAsync(message.Chat, $"You cannot be {age} years old\n" +
                            $"Please, enter your age again");
                }
                else
                {
                    user.Age = age;
                    if (!user.edit)
                    {
                        user.Command = "WEIGHT";
                        if (user.Language == "Russian")
                            await client.SendTextMessageAsync(message.Chat.Id, "Введи свой вес(в кг)!");
                        else
                            await client.SendTextMessageAsync(message.Chat, "Please enter your weight in kilograms");
                    }
                    else
                    {
                        user.Command = " "; user.edit = false;
                        if (user.Language == "Russian")
                            await client.SendTextMessageAsync(message.Chat.Id, "Возраст успешно изменён!");
                        else
                            await client.SendTextMessageAsync(message.Chat.Id, "The age edited successfully!");
                    }
                }
            }
            else
            {
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(message.Chat.Id, "Неправильно введен возраст");
                else
                    await client.SendTextMessageAsync(message.Chat.Id, "Error with input of age");
            }
        }
    }
}
