﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Configuration;
using System.Threading;
using System.IO;


namespace ConsoleApplication1
{
    public class RandomCodeReviewJob : JobBase
    {
        protected override void ExecuteJob(IJobExecutionContext context)
        {
            try
            {
                RandomCodeReviewManager manager = new RandomCodeReviewManager();
                manager.WriteData();
                SendEmailJob sendEmail = new SendEmailJob();
                sendEmail.SendEmail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public override void Register(IScheduler scheduler)
        {
            bool isTest = Convert.ToBoolean(ConfigurationSettings.AppSettings["IsTest"]);
            IJobDetail job = JobBuilder.Create(this.GetType())
                .Build();
            ITrigger trigger;
            if (isTest)
            {
                trigger = TriggerBuilder.Create()
               .StartNow()
               .Build();
            }
            else
            {
                trigger = (ICronTrigger)TriggerBuilder.Create()
                     .WithCronSchedule(ConfigManager.GetConfig().RandomCodeReviewJob)
                     .Build();
            }
            scheduler.ScheduleJob(job, trigger);
        }
    }

}

