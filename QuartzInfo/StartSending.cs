using Quartz;
using Quartz.Impl;
using System;

namespace Calories.QuartzInfo
{
    public class StartSending
    {
        static DateTime MorningTime = DateTime.Now
            .AddHours(33-DateTime.Now.Hour)
            .AddMinutes(60-DateTime.Now.Minute);

        static DateTime NightTime = DateTime.Now.AddDays(-1)
            .AddHours(25 - DateTime.Now.Hour)
            .AddMinutes(60 - DateTime.Now.Minute);
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail MorningJob = JobBuilder.Create<MorningSender>().Build();
            IJobDetail NightJob = JobBuilder.Create<NightUpdate>().Build();

            ITrigger triggerForMorning = TriggerBuilder.Create()
                .WithIdentity("morning", "group1")
                .StartAt(new DateTimeOffset(MorningTime))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .Build();

            ITrigger triggerForNight = TriggerBuilder.Create()
                .WithIdentity("night", "group1")
                .StartAt(new DateTimeOffset(NightTime))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(MorningJob, triggerForMorning);
            await scheduler.ScheduleJob(NightJob, triggerForNight);

        }
    }
}
