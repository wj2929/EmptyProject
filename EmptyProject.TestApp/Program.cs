using EmptyProject.TestApp.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmptyProject.DomainService.Interface;
using System.IO;
using Excel;
using System.Data;
using EmptyProject.Domain.Values.Settings;
using EmptyProject.Domain;
using EmptyProject.Domain.QueryObject;

namespace EmptyProject.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalController.InjectCustomMap();
            IStudentDomainService StudentDomainService = GlobalController.IoC.Resolve<IStudentDomainService>();
            ITeacherDomainService TeacherDomainService = GlobalController.IoC.Resolve<ITeacherDomainService>();
            IClassRoomDomainService ClassRoomDomainService = GlobalController.IoC.Resolve<IClassRoomDomainService>();
            ITermDomainService TermDomainService = GlobalController.IoC.Resolve<ITermDomainService>();
            IOptionalCourseDomainService OptionalCourseDomainService = GlobalController.IoC.Resolve<IOptionalCourseDomainService>();
            IEmptyProjectRuleDomainService EmptyProjectRuleDomainService = GlobalController.IoC.Resolve<IEmptyProjectRuleDomainService>();
            IEmptyProjectPlanDomainService EmptyProjectPlanDomainService = GlobalController.IoC.Resolve<IEmptyProjectPlanDomainService>();
            IOptionalCourseWillingStudentDomainService OptionalCourseWillingStudentDomainService = GlobalController.IoC.Resolve<IOptionalCourseWillingStudentDomainService>();
            StudentDomainService.PrepareImportStudentData(@"F:\快盘\工作文档\智能排课\学生导入模版.xlsx", "Test");
            StudentDomainService.BatchImportStudentData(1, 100);

            TeacherDomainService.PrepareImportTeacherData(@"F:\快盘\工作文档\智能排课\教师导入模版.xlsx", "Test");
            TeacherDomainService.BatchImportTeacherData(1, 100);

            ClassRoomDomainService.PrepareImportClassRoomData(@"F:\快盘\工作文档\智能排课\示例数据-专业教室.xlsx", "Test");
            ClassRoomDomainService.BatchImportClassRoomData(1, 100);

            IList<Term> AllTerms = TermDomainService.GetList(new TermCriteria() { TenantId = "Test" });

            OptionalCourseDomainService.PrepareImportOptionalCourseData(@"F:\快盘\工作文档\智能排课\课程数据模版.xlsx", "Test", AllTerms.First().Id);
            OptionalCourseDomainService.BatchImportOptionalCourseData(1, 100);

            EmptyProjectRule EmptyProjectRuleInfo = EmptyProjectRuleDomainService.GetList(new EmptyProjectRuleCriteria() { TenantId = "Test" }).First();
            EmptyProjectRuleSetting EmptyProjectRuleSetting = EmptyProjectRuleInfo.EmptyProjectRuleSetting;
            EmptyProjectRuleSetting.OptionalCourseIds = OptionalCourseDomainService.GetList(new OptionalCourseCriteria() { TenantId = "Test" }).Select(t => t.Id).ToList();
            EmptyProjectRuleInfo.Setting = EmptyProjectRuleSetting.ToConfig();
            EmptyProjectRuleDomainService.EditEmptyProjectRule(EmptyProjectRuleInfo);

            EmptyProjectPlan EmptyProjectPlanInfo = EmptyProjectPlanDomainService.GetList(new EmptyProjectPlanCriteria() { TenantId = "Test" }).First();
            EmptyProjectPlanInfo.EmptyProjectRuleSettings = EmptyProjectRuleSetting.ToConfig();
            EmptyProjectPlanDomainService.EditEmptyProjectPlan(EmptyProjectPlanInfo);

            IList<OptionalCourse> AllOptionalCourses = OptionalCourseDomainService.GetList(new OptionalCourseCriteria() { TenantId = "Test" });
            OptionalCourseDomainService.AddStudents(AllOptionalCourses.First().Id, StudentDomainService.GetList(new StudentCriteria() { TenantId = "Test" }).Select(t => t.Id).ToList());

            OptionalCourseDomainService.RemoveStudents(AllOptionalCourses.First().Id, StudentDomainService.GetList(new StudentCriteria() { TenantId = "Test" }).Take(2).Select(t => t.Id).ToArray());

            OptionalCourseWillingStudentDomainService.SignUp(EmptyProjectPlanInfo.Id, AllOptionalCourses.First().Id, "20161238", 100);
        }
    }
}
