using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Calories.Commands
{
    class AboutMeCommand : Command
    {
        public override string Name => "/about_me";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            string gender, activity, goal;
            if (user.Language == "Russian")
            {
                if (user.Male) gender = "парень";
                else gender = "девушка";
                switch (user.Activity)
                {
                    case 1: activity = "Ты просто сидишь дома)"; break;
                    case 1.1: activity = "Ты не любишь спорт или просто ленивый(ая) :)"; break;
                    case 1.2375: activity = "Ты занимаешься раз в неделю"; break;
                    case 1.375: activity = "Ты занимаешься 2-3 в неделю"; break;
                    case 1.43: activity = "Ты занимаешься 4-5 в неделю"; break;
                    case 1.55: activity = "Ты прям любитель спорта\nЗанимаешь почти каждый день"; break;
                    case 1.725: activity = "Ты прям спортивный псих)"; break;
                    default:
                        activity = "Прости, но ты не указал свою активность";
                        break;
                }
                switch (user.Goal)
                {
                    case "Minus": goal = "похудеть"; break;
                    case "Norm": goal = "остаться таким(ой) как есть"; break;
                    case "Plus": goal = "набрать весу"; break;
                    case "normaly": goal = "худеть с нормальной скоростью"; break;
                    case "quickly": goal = "худеть с быстрой скоростью"; break;
                    case "extremely": goal = "худеть с экстремальной скоростью"; break;
                    default:
                        goal = "not entered"; break;
                }
                await client.SendTextMessageAsync(message.Chat, $"Это вся информация, которую я имею про тебя:\n" +
                    $"Тебя зовут {user.Name} и ты {gender}\n" +
                    $"Тебе {user.Age} лет\n" +
                    $"Твой вес - {user.Weight} kg\n" +
                    $"Твой рост - {user.Height} cm\n" +
                    $"{activity}\n" +
                    $"Твоя цель это {goal}\n\n" +
                    $"Что ж, если ты хочешь что-то изменить - используй команды:\n" +
                    "/age - изменить возраст\n" +
                     "/weight - изменить вес\n" +
                     "/height - изменить рост\n" +
                     "/activity - изменить активность\n" +
                     "/gender - изменить пол\n" +
                     "/goal - изменить цель");
            }
            else
            {
                if (user.Male) gender = "man";
                else gender = "woman";
                switch (user.Activity)
                {
                    case 1: activity = "You always sit at home)))"; break;
                    case 1.1: activity = "You don`t like sport or you are lazy)"; break;
                    case 1.2375: activity = "You do sport but only once at week"; break;
                    case 1.375: activity = "You like sport and do some exercize"; break;
                    case 1.43: activity = "You are extremely active person"; break;
                    case 1.55: activity = "You are really into sport and you love it"; break;
                    case 1.725: activity = "You are psycho which cannot live without sport"; break;
                    default:
                        activity = "Sorry, I haven`t your level of activity";
                        break;
                }
                switch (user.Goal)
                {
                    case "Minus": goal = "weight loss"; break;
                    case "Norm": goal = "keeping fit"; break;
                    case "Plus": goal = "putting on weight"; break;
                    case "normaly": goal = "normaly weight loss"; break;
                    case "quickly": goal = "quickly weight loss"; break;
                    case "extremely": goal = "extremely weight loss"; break;
                    default:
                        goal = "not entered"; break;
                }
                await client.SendTextMessageAsync(message.Chat, $"It`s all information about you, that I know:\n" +
                    $"Your name is {user.Name} and you are {gender}\n" +
                    $"You are {user.Age} years old\n" +
                    $"You weigh {user.Weight} kg\n" +
                    $"Your height is {user.Height} cm\n" +
                    $"{activity}\n" +
                    $"And your goal is {goal}\n\n" +
                    $"So, if you want to edit something you should use commands:\n" +
                    "/age - to edit or enter your age\n" +
                     "/weight - to edit or enter your weight\n" +
                     "/height - to edit or enter your height\n" +
                     "/activity - to edit or enter your level of activity\n" +
                     "/gender - to edit or enter your sex\n" +
                     "/goal - to change goal");
            }
        }
    }
}