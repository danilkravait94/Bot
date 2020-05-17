using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Calories.QuartzInfo
{
    public class MorningSender : Class_for_DB , IJob
    {
        public static TelegramBotClient client = new TelegramBotClient(Bot.token);
        public async Task Execute(IJobExecutionContext context)
        {
            foreach (User user in DB.Users)
            {
                if (user.Calories != 0)
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(user.ChatId, $"Доброе утро)\n" +
                            $"Самое время внести свой завтрак\n" +
                            $"напиши\n/addfood [название] [вес] - чтобы добавить еду");
                    else
                        await client.SendTextMessageAsync(user.ChatId, $"Good Morning\n" +
                            $"It`s time to enter your breakfast\n" +
                            $"write\n/addfood [name] [weight] - to add food");
                }
                else
                {
                    if (user.Language == "Russian")
                        await client.SendTextMessageAsync(user.ChatId, $"Доброе утро)\n" +
                            $"Самое время внести данные про себя\n" +
                            $"напиши /countcal");
                    else
                        await client.SendTextMessageAsync(user.ChatId, $"Good Morning\n" +
                            $"It`s time to enter your breakfast\n" +
                            $"write /countcal");
                }
            }
            using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
            {
                sw.WriteLine($"{DateTime.Now} - Morning Sending ");
            }
        }
    }
}
