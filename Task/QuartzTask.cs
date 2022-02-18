using IceSync.Controllers;
using Quartz;
using Quartz.Impl;
using System;

namespace IceSync.Task
{
    public static class QuartzTask
    {
        public static async void CallingTask()
        {
            IScheduler zamanlayici = await StdSchedulerFactory.GetDefaultScheduler();

            if (!zamanlayici.IsStarted)
                await zamanlayici.Start();
            // STarting 
            IJobDetail jobs = JobBuilder.Create<WorkflowsController>().Build();
            ITrigger trigger = TriggerBuilder.Create()
     .StartNow()
     .WithSimpleSchedule(x => x
         .WithIntervalInMinutes(1)
         .RepeatForever())
     .Build();
            await zamanlayici.ScheduleJob(jobs, trigger);
        }
     
       

    }
}
