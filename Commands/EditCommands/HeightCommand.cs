using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class HeightCommand : Command
    {
        public override string Name => @"/height";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat.Id, "Введи свой рост(в см)!");
            else
                await client.SendTextMessageAsync(message.Chat, "Please enter your height in centimeters");
            user.edit = true;
            user.Command = "HEIGHT";
        }
    }
}
