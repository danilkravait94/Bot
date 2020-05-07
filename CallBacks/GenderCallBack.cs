using Telegram.Bot;
using Telegram.Bot.Args;

namespace Calories.CallBacks
{
    class GenderCallBack : CallBack
    {

        public override string Name => "GENDER";
        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (e.CallbackQuery.Data == "Male") { user.Male = true; }
            else if (e.CallbackQuery.Data == "Female") { user.Male = false; }

            if (!user.edit)
            {
                 user.Command = "AGEINPUT";
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Введи свой возраст!");
                else
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Please,enter your age!");
            }
            else
            {
                user.Command = " "; user.edit = false;
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Пол успешно изменён!");
                else
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "The gender edited successfully!");
            }
        }
    }
}
