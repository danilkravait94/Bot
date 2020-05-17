using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.FreeCatchings
{
    public abstract class FreeCatching : Class_for_DB
    {
        public static InlineKeyboardMarkup SelectOrNoRus = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
       {
            new [] {InlineKeyboardButton.WithCallbackData("Добавить продукт","Yes"),},
            new [] {InlineKeyboardButton.WithCallbackData("Перезадать продукт","No"), }
       });
        public static InlineKeyboardMarkup SelectOrNoEng = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
         {
             new [] { InlineKeyboardButton.WithCallbackData("Add product","Yes"), },
             new [] { InlineKeyboardButton.WithCallbackData("Reset info about product","No") }
        });
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client, long id);
        public bool Contains(string command)
        {
            if (command == null) return false;
            return command.Contains(this.Name);
        }
        public async void SendMessageFood(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            if (user.Language == "Russian")
                await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                    $"Калорийность - {user.food.nutrients.ENERC_KCAL:0.0}\n" +
                    $"Белки - {user.food.nutrients.PROCNT:0.0}\n" +
                    $"Жири - {user.food.nutrients.FAT:0.0}\n" +
                    $"Углеводы - {user.food.nutrients.CHOCDF:0.0}\n", replyMarkup: SelectOrNoRus);
            else
                await client.SendTextMessageAsync(message.Chat, $"{user.food.label}\n" +
                    $"Calories - {user.food.nutrients.ENERC_KCAL:0.0}\n" +
                    $"Protein - {user.food.nutrients.PROCNT:0.0}\n" +
                    $"Fat - {user.food.nutrients.FAT:0.0}\n" +
                    $"Carbohydrates - {user.food.nutrients.CHOCDF:0.0}\n", replyMarkup: SelectOrNoEng);
        }
    }
}
