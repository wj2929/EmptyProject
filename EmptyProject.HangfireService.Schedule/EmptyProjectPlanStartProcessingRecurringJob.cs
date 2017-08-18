using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC.Core;
using EmptyProject.DomainService.Interface;

namespace EmptyProject.HangfireService.Schedule
{
    [AutomaticRetry(Attempts = 0)]
    [DisableConcurrentExecution(90)]
    public class EmptyProjectPlanStartProcessingRecurringJob : IRecurringJob
    {
        public EmptyProjectPlanStartProcessingRecurringJob(IEmptyProjectPlanDomainService EmptyProjectPlanDomainService) 
        {
            this.TestDomainService = EmptyProjectPlanDomainService;
        }

        private readonly IEmptyProjectPlanDomainService EmptyProjectPlanDomainService;

        [DisplayName("选课计划自动开始")]
        public void Execute(PerformContext context)
        {
            //获取最近1分钟内开始的选课计划
            IList<EmptyProjectPlan> RecentestOneMinuteStartEmptyProjectPlans = EmptyProjectPlanDomainService.RecentestOneMinuteStartEmptyProjectPlans();
            RecentestOneMinuteStartEmptyProjectPlans.ForEach(t => 
            {
                BackgroundJob.Enqueue<IEmptyProjectPlanDomainService>(EmptyProjectPlanService => EmptyProjectPlanService.StartProcessing(t.Id));
                //BackgroundJob.Enqueue(() => EmptyProjectPlanDomainService.StartProcessing(t));
            });
        }
    }
}
