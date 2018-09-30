$(function () {

    var TermId = "CED62372-EBB6-447D-920F-F14063A8ACF5"
    var param = {};

    //***************************工具******************************************//
    //获取设置
    param = {};
    buildTestContent("Tools_Config", param);

    //***************************机构******************************************//
    param = {};
    buildTestContent("Organization_List", param);

    //***************************学期******************************************//
    param = {};
    buildTestContent("Term_List", param);

    //***************************年级分组******************************************//
    param = {  };
    buildTestContent("GradeGroup_List", param);

    param = { Name: "Test" };
    buildTestContent("GradeGroup_Create", param);

    param = { EditId: "369f08a1-a389-4ac9-95de-e8092f2b4e0d", Name: "Test1" };
    buildTestContent("GradeGroup_Edit", param);

    param = { RemoveId: "" };
    buildTestContent("GradeGroup_Remove", param);

    //***************************年级******************************************//
    param = {  };
    buildTestContent("Grade_List", param);

    param = { GradeGroupId: "34dd9346-7067-428d-b492-37620e4151cc", Name: "test" };
    buildTestContent("Grade_Create", param);

    param = { EditId: "", Name: "Test1" };
    buildTestContent("Grade_Edit", param);

    param = { RemoveId: "" };
    buildTestContent("Grade_Remove", param);

    //***************************选课规则******************************************//
    param = { TermId: TermId };
    buildTestContent("SCRule_List", param);

    //param = {
    //    
    //    TermId: TermId,
    //    Name: "test",
    //    'SCRuleSetting.StudentRules[0].Key': "Sex", 'SCRuleSetting.StudentRules[0].Value': "All",
    //    'SCRuleSetting.StudentRules[1].Key': "Grade", 'SCRuleSetting.StudentRules[1].Value': "All",
    //    'SCRuleSetting.OptionalCourseIds': "cf8e6203-071e-4826-b58b-e2f9caf5e0be"
    //};
    //param = {
    //    
    //    TermId: TermId,
    //    Name: "test",
    //    'SCRuleSetting.StudentRules[0].Key': "Sex", 'SCRuleSetting.StudentRules[0].Value': "All",
    //    'SCRuleSetting.StudentRules[1].Key': "Grade", 'SCRuleSetting.StudentRules[1].Value': "All",
    //    'SCRuleSetting.OptionalCourseRules[0].Key': "OptionalCourse", 'SCRuleSetting.OptionalCourseRules[0].Value': "cf8e6203-071e-4826-b58b-e2f9caf5e0be"
    //};
    //buildTestContent("SCRule_Create", param);


    param = {

        TermId: TermId,
        Name: "test",
        SexRule: "All",
        GradeRule: "All",
        OptionalCourseRule: "All",
        EditId: ""
        //'SCRuleSetting.StudentRules[0].Key': "Sex", 'SCRuleSetting.StudentRules[0].Value': "All",
        //'SCRuleSetting.StudentRules[1].Key': "Grade", 'SCRuleSetting.StudentRules[1].Value': "All",
        //'SCRuleSetting.OptionalCourseRules[0].Key': "OptionalCourse", 'SCRuleSetting.OptionalCourseRules[0].Value': "cf8e6203-071e-4826-b58b-e2f9caf5e0be"
    };
    buildTestContent("SCRule_CreateOrEdit", param);

    param = { RemoveId: "10fefb35-08df-4d68-bc9f-4f401e71ea9c" };
    buildTestContent("SCRule_Remove", param);


    //***************************选课计划******************************************//
    param = { TermId: TermId, PageNum: 1, PageSize: 10 };
    buildTestContent("SCPlan_Paging", param);

    param = { TermId: TermId, StartDate: "2017-06-27 09:00", EndDate: "2017-06-27 10:00", SCRuleIds: "10fefb35-08df-4d68-bc9f-4f401e71ea9c" };
    buildTestContent("SCPlan_Create", param);

    param = { Id: "bee9e3c2-0510-494c-9770-083f15023f81" };
    buildTestContent("SCPlan_Single", param);

    param = { Id: "bee9e3c2-0510-494c-9770-083f15023f81" };
    buildTestContent("SCPlan_Suspend", param);

    param = { Id: "bee9e3c2-0510-494c-9770-083f15023f81" };
    buildTestContent("SCPlan_End", param);

    param = { RemoveId: "" };
    buildTestContent("SCPlan_Remove", param);

    param = { };
    buildTestContent("SCPlan_StudentAvailableSCPlanList", param);

    //***************************课程******************************************//
    param = {  };
    buildTestContent("Course_List", param);

    param = { Name: "Test" };
    buildTestContent("Course_Create", param);

    param = { EditId: "c912a48b-271a-4789-a225-05eb2970a04c", Name: "体育" };
    buildTestContent("Course_Edit", param);

    param = { RemoveId: "" };
    buildTestContent("Course_Remove", param);


    //***************************选修班******************************************//
    param = { TermId: TermId, IsOptional:true,PageNum: 1, PageSize: 10 };
    buildTestContent("OptionalCourse_Paging", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34" };
    buildTestContent("OptionalCourse_ListBySCPlanId", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34" };
    buildTestContent("OptionalCourse_CourseGroupListBySCPlanId", param);

    param = { };
    buildTestContent("OptionalCourse_TermGroupListByTeacher", param);

    //***************************学生******************************************//
    param = { IsAllSex: false, Sexs: '0,1', IsAllGrade: true, Grades: "", PageNum: 1, PageSize: 10 };
    buildTestContent("Student_Paging", param);

    param = {};
    buildTestContent("Student_EnrolledCount", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", OptionalCourseId: "9a850480-3d3a-41e8-9608-08f4d8c03544", UserName: "20161238", UseWillingPoint: 50 };
    buildTestContent("Student_SignUpOptionalCourse", param);

    param = { UserName: "20161240" };
    buildTestContent("Student_AvailableSCPlanList", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", Sex: -1, PageNum: 1, PageSize: 10 };
    buildTestContent("Student_SignUpSCPlanPaging", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", OptionalCourseId: "9a850480-3d3a-41e8-9608-08f4d8c03544", HighPriority: "" };
    buildTestContent("Student_SignUpOptionalCourseList", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", OptionalCourseId: "9a850480-3d3a-41e8-9608-08f4d8c03544"};
    buildTestContent("Student_OptionalCourseSelectedList", param);

    //***************************教师******************************************//
    param = { Sex: -1, PageNum: 1, PageSize: 10 };
    buildTestContent("Teacher_Paging", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", OptionalCourseId: "9a850480-3d3a-41e8-9608-08f4d8c03544", "StudentId": "96c2785f-48d1-41bc-905d-454211d37335", TeacherUserName: "" };
    buildTestContent("Teacher_HighPriorityStudent", param);

    param = { SCPlanId: "0FF0DF50-F0D3-4B2F-8322-D58EBB94FF34", OptionalCourseId: "9a850480-3d3a-41e8-9608-08f4d8c03544", "StudentId": "96c2785f-48d1-41bc-905d-454211d37335", TeacherUserName: "" };
    buildTestContent("Teacher_CancelHighPriorityStudent", param);

});

