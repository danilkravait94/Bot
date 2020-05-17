using System.Collections.Generic;
using Telegram.Bot;
using Calories.Commands;
using Calories.CallBacks;
using Calories.FreeCatchings;

namespace Calories
{
    public static class Bot
    {
        public static TelegramBotClient client;
        public static string token = "YourToken";
        public static string Name = "CaloriesCalculator_bot";

        private static List<Command> commandslist;
        public static IReadOnlyList<Command> Commands { get => commandslist.AsReadOnly(); }

        private static List<CallBack> callbacklist;
        public static IReadOnlyList<CallBack> CallBacks { get => callbacklist.AsReadOnly(); }

        private static List<FreeCatching> freecatchinglist;
        public static IReadOnlyList<FreeCatching> FreeCatchings { get => freecatchinglist.AsReadOnly(); }
        public static TelegramBotClient Get()
        {
            commandslist = new List<Command>();
            commandslist.Add(new StartCommand());
            commandslist.Add(new CountCalCommand());
            commandslist.Add(new InformationCommand());
            commandslist.Add(new ActivityCommand());
            commandslist.Add(new AgeCommand());
            commandslist.Add(new HeightCommand());
            commandslist.Add(new WeightCommand());
            commandslist.Add(new GenderCommand());
            commandslist.Add(new GoalCommand()); 
            commandslist.Add(new CalculateCalCommand());
            commandslist.Add(new AboutMeCommand());
            commandslist.Add(new DeleteCommand()); 
            commandslist.Add(new MyOpinionCommand());
            commandslist.Add(new AddFoodCommand());
            commandslist.Add(new DayInfoCommand());
            commandslist.Add(new RemoveFoodCommand());


            callbacklist = new List<CallBack>();
            callbacklist.Add(new LanguageCallBack());
            callbacklist.Add(new GenderCallBack());
            callbacklist.Add(new GoalCallBack());
            callbacklist.Add(new ActivityCallBack());
            callbacklist.Add(new LostCallBack());
            callbacklist.Add(new MinusCallBack());
            callbacklist.Add(new PlusCallBack());
            callbacklist.Add(new FoodCallBack());


            freecatchinglist = new List<FreeCatching>();
            freecatchinglist.Add(new AgeCatch());
            freecatchinglist.Add(new WeightCatch());
            freecatchinglist.Add(new HeightCatch());
            freecatchinglist.Add(new NameOfFoodCatch());
            freecatchinglist.Add(new TextOfFoodCatch());
            freecatchinglist.Add(new WeightOfFoodCatch());
            freecatchinglist.Add(new FoodRemoveCatch());

            client = new TelegramBotClient(token);
            return client;
        }

    }
}
