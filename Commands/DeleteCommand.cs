using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Calories.Commands
{

    class DeleteCommand : Command
    {
        public override string Name => @"/delete";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.DeleteUser();
            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, $"Вся информация удалена\n" +
                       $"Напиши команду /countcal что бы ввести новую инфу и посчитать каллории");
            else
                await client.SendTextMessageAsync(message.Chat, $"All information was removed\n" +
                            $"Write a command /countcal to enter new info and count calories");
        }
    }
}
