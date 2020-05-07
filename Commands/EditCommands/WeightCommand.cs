using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class WeightCommand : Command
    {
        public override string Name => @"/weight";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat.Id, "Введи свой вес(в кг)!");
            else
                await client.SendTextMessageAsync(message.Chat, "Please enter your weight in kilograms");
            user.edit = true;
            user.Command = "WEIGHT";
        }
    }
}
