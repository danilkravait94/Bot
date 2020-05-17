using Telegram.Bot;
using Telegram.Bot.Types;

namespace Calories.Commands
{
    class CalculateCalCommand : Command
    {
        public override string Name => "/calculate_cal";

        public override void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            double calories;
            if (user.Male)
            { calories = 88.363 + (13.397 * user.Weight) + (4.799 * user.Height) - (5.677 * user.Age); }
            else
            { calories = 447.593 + (9.247 * user.Weight) + (3.098 * user.Height) - (4.330 * user.Age); }
            calories *= user.Activity;
            user.Proteins = (int)((calories * 0.3) / 4 + 1);
            user.Fats = (int)((calories * 0.3) / 9 + 1);
            user.Carbohydrates = (int)((calories * 0.3) / 4 + 1);
            if (user.Goal == "normaly")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(message, client, id, "безопасной потери веса");
                else
                    SendMessageCalories(message, client, id, $"weight loss {user.Goal}");
            }
            else if (user.Goal == "quickly")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(message, client, id, "быстрой потери веса");
                else
                    SendMessageCalories(message, client, id, $"weight loss {user.Goal}");
            }
            else if (user.Goal == "extremely")
            {
                if (user.Language == "Russian")
                    SendMessageCalories(message, client, id, "потери веса екстремально быстро");
                else
                    SendMessageCalories(message, client, id, $"weight loss {user.Goal}");
            }
            else if (user.Goal == "Norm")
            {
                user.Command = " ";
                if (user.Language == "Russian")
                    SendMessageCalories(message, client, id, "поддержки веса");
                else
                    SendMessageCalories(message, client, id, $"keeping fit");
            }
            else if (user.Goal == "Plus")
            {
                calories *= 1.2;
                user.Proteins = (int)((calories * 0.2) / 4 + 1);
                user.Fats = (int)((calories * 0.3) / 9 + 1);
                user.Carbohydrates = (int)((calories * 0.5) / 4 + 1);
                user.Command = " ";
                if (user.Language == "Russian")
                    SendMessageCalories(message, client, id, "набора веса");
                else
                    SendMessageCalories(message, client, id, $"putting weight");
            }
            user.Calories = (int)calories;
            user.SetCalories();
        }
    }
}