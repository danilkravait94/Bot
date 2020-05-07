using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class MyOpinionCommand : Command
    {
        public override string Name => @"/opinion";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, $"Мое мнение по поводу похудения и набора веса будет в ближайшее время");
            else
                await client.SendTextMessageAsync(message.Chat, $"My opinion will be there in the near future");
        }
    }
}
