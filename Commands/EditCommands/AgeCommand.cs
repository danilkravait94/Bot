using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class AgeCommand : Command
    {
        public override string Name => @"/age";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat.Id, "Введи свой возраст!");
            else
                await client.SendTextMessageAsync(message.Chat.Id, "Please,enter your age!");
            user.edit = true;
            user.Command = "AGEINPUT";
        }
    }
}
