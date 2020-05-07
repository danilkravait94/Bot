using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Calories.CallBacks
{
    class FoodCallBack : CallBack
    {
        public override string Name => "SELECTFOOD";
        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (e.CallbackQuery.Data == "Yes")
            {
                user.Command = " ";
                user.MinusOfDay(user.food);
                user.ToEmptyFood();
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Продукт добавлен");
                else
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "The food was added");
            }
            else if (e.CallbackQuery.Data == "No")
            {
                user.Command = "TextOfFood";
                user.ToEmptyFood();
                if (user.Language == "Russian")
                {
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Напиши пожалуйста название и вес в граммах продукта");
                }
                else await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Please, write a name and weight in grams of food");
            }
        }
    }
}
