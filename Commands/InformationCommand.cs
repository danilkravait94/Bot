using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class InformationCommand : Command
    {
        public static InlineKeyboardMarkup keyboardInfo = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {new [] {
                                                    InlineKeyboardButton.WithUrl("Telegram","https://t.me/agent_d"),
                                                    InlineKeyboardButton.WithUrl ("Instagram","https://www.instagram.com/danillll228/"),
                                          },
                                         });
        public override string Name => @"/information";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, "Nice to meet you)\n" +
                "My name is Danil\n" +
                "I`m the student of KPI and " +
                "it`s my first project\n" +
                "If you have complains or advice, you can write me there:", replyMarkup: keyboardInfo);
            else await client.SendTextMessageAsync(message.Chat, "Привет)\n" +
                "Меня зовут Данил\n" +
                "Я учусь в КПИ и " +
                "это мой первый проект\n" +
                "Если у тебя есть предложение или ты заметил какой-то баг,то напиши мне:", replyMarkup: keyboardInfo);
        }
    }
}
