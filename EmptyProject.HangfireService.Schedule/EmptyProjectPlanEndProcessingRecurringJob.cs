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
    public class EmptyProjectPlanEndProcessingRecurringJob : IRecurringJob
    {
        public EmptyProjectPlanEndProcessingRecurringJob(IEmptyProjectPlanDomainService EmptyProjectPlanDomainService) 
        {
            this.TestDomainService = EmptyProjectPlanDomainService;
        }

        private readonly IEmptyProjectPlanDomainService EmptyProjectPlanDomainService;

        [DisplayName("选课计划结束时按学生意愿自动选课")]
        public void Execute(PerformContext context)
        {
            //获取最近1分钟内截止的选课计划
            IList<EmptyProjectPlan> RecentestOneMinuteEndEmptyProjectPlans = EmptyProjectPlanDomainService.RecentestOneMinuteEndEmptyProjectPlans();
            RecentestOneMinuteEndEmptyProjectPlans.ForEach(t => 
            {
                BackgroundJob.Enqueue<IEmptyProjectPlanDomainService>(EmptyProjectPlanService => EmptyProjectPlanService.EndProcessing(t.Id));

                //BackgroundJob.Enqueue(() => EmptyProjectPlanDomainService.EndProcessing(t));
            });
        }
    }
}
