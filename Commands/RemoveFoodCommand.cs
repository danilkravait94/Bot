using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class RemoveFoodCommand : Command
    {
        public override string Name => @"/removefood";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            string food;
            user.Command = "FoodRemove";
            if (user.Language == "Russian")
                food = "Напиши цыфру, которого хочешь удалить\n";
            else
                food = "Write a number which you want to remove\n";
            if (user.DayFoodList.Count != 0)
                await client.SendTextMessageAsync(message.Chat.Id, ListToString(user.DayFoodList, food));
            else
            {
                user.Command = " ";
                if (user.Language == "Russian")
                    await client.SendTextMessageAsync(message.Chat.Id, "Ты сегодня не добавил ниодного продукта\n" +
                                            $"напиши\n/addfood [название] [вес] - чтобы добавить еду");
                else
                    await client.SendTextMessageAsync(message.Chat.Id,"You haven`t added any food\n" +
                                            $"write\n/addfood [name] [weight] - to add food");
            }

        }
        private static string ListToString(List<Food> foodlist,string foods)
        {
            for (int i =0 ; i<foodlist.Count ; i++)
            {
                foods +=$"{i} "+ foodlist[i].ToString()+"\n";
            }
            return foods;
        }
    }
    
}
