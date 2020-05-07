using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class DayInfoCommand : Command
    {
        static string aboutprotein;
        static string aboutfat;
        static string aboutcalories;
        static string aboutcarborates;
        public override string Name => @"/dayinfo";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            if (user.Language == "Russian")
            {
                if (user.DayProteins > 0) aboutprotein = $"Осталось {user.DayProteins} грамм белка";
                else if(user.DayProteins == 0) aboutprotein = $"Ты достиг цели по белкам";
                else aboutprotein = $"Ты переел цель по белка на {Math.Abs(user.DayProteins)} грамм";

                if (user.DayFats > 0) aboutfat = $"Осталось {user.DayFats} грамм жиров";
                else if (user.DayFats == 0) aboutfat = $"Ты достиг цели по жирам";
                else aboutfat = $"Ты переел цель по жирам на {Math.Abs(user.DayFats)} грамм";

                if (user.DayCalories > 0) aboutcalories = $"Осталось {user.DayCalories} калорий";
                else if (user.DayCalories == 0) aboutcalories = $"Ты достиг цели по калориям";
                else aboutcalories = $"Ты переел на {Math.Abs(user.DayCalories)} калорий";

                if (user.DayCarbohydrates > 0) aboutcarborates = $"Осталось {user.DayCarbohydrates} грамм углеводов";
                else if (user.DayCarbohydrates == 0) aboutcarborates = $"Ты достиг цели по углеводам";
                else aboutcarborates = $"Ты переел цель по углеводов на {Math.Abs(user.DayCarbohydrates)} грамм";
            }
            else 
            {
                if (user.DayProteins > 0) aboutprotein = $"You have to eat {user.DayProteins} grams of protein";
                else if (user.DayProteins == 0) aboutprotein = $"You reach your protein goal";
                else aboutprotein = $"You have ate {Math.Abs(user.DayProteins)} grams of protein more than you need";

                if (user.DayFats > 0) aboutfat = $"You have to eat {user.DayFats} grams of fat";
                else if (user.DayFats == 0) aboutfat = $"You reach your fat goal";
                else aboutfat = $"You have ate {Math.Abs(user.DayFats)} grams more than you need";

                if (user.DayCalories > 0) aboutcalories = $"You have to eat {user.DayCalories} calories";
                else if (user.DayCalories == 0) aboutcalories = $"You reach your calories goal";
                else aboutcalories = $"You have ate {Math.Abs(user.DayCalories)} calories more than you need";

                if (user.DayCarbohydrates > 0) aboutcarborates = $"You have to eat {user.DayCarbohydrates} grams of carbohydrates";
                else if (user.DayCarbohydrates == 0) aboutcarborates = $"You reach your carbohydrate goal";
                else aboutcarborates = $"You have ate {Math.Abs(user.DayCarbohydrates)} grams of carbohydrates more than you need";
            }
            if (StartFind("Осталось"))
            {
                await client.SendTextMessageAsync(message.Chat, $"Осталось:\n" +
                    $"{aboutcalories.Substring(8)} из которых:\n" +
                    $"\t\t\t\t\t\t{aboutprotein.Substring(8)}\n" +
                    $"\t\t\t\t\t\t{aboutfat.Substring(8)}\n" +
                    $"\t\t\t\t\t\t{aboutcarborates.Substring(8)}");
                return;
            }
            if (StartFind("You have to eat"))
            {
                await client.SendTextMessageAsync(message.Chat, $"You have to eat:\n" +
                    $"{aboutcalories.Substring(15)} of which:\n" +
                    $"\t\t\t\t\t\t{aboutprotein.Substring(15)}\n" +
                    $"\t\t\t\t\t\t{aboutfat.Substring(15)}\n" +
                    $"\t\t\t\t\t\t{aboutcarborates.Substring(15)}");
                return;
            }
            await client.SendTextMessageAsync(message.Chat, $"{aboutcalories}\n" +
                    $"\t\t\t\t\t\t{aboutprotein}\n" +
                    $"\t\t\t\t\t\t{aboutfat}\n" +
                    $"\t\t\t\t\t\t{aboutcarborates}");
        }
        private static bool StartFind(string sentence)
        {
            return (aboutcalories.StartsWith(sentence) &
                aboutcarborates.StartsWith(sentence) &
                aboutfat.StartsWith(sentence) &
                aboutprotein.StartsWith(sentence));
        }
    }
}