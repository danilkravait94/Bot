using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Calories.QuartzInfo
{

    class NightUpdate : Class_for_DB, IJob
    {
        public static TelegramBotClient client = new TelegramBotClient(Bot.token);
        public async Task Execute(IJobExecutionContext context)
        {
            foreach (User user in DB.Users)
            {
                user.UpdateUser();
            }
            await DB.SaveChangesAsync();
            using (StreamWriter sw = System.IO.File.AppendText(@"C:\Users\Олег\source\repos\CaloriesConsole\Message.txt"))
            {
                sw.WriteLine($"{DateTime.Now} - Updating ");
            }
        }
    }
}
