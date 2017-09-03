using EmptyProject.DomainService.Interface;
using EmptyProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BC.DDD.ObjectMapper;
using EmptyProject.Domain;
using System.Text;
using BC.Core;
namespace EmptyProject.Service.Controllers
{
    public class TestController : BaseController
    {
        public TestController(ITestDomainService TestDomainService) 
        {
            this.TestDomainService = TestDomainService;
        }
        private readonly ITestDomainService TestDomainService;

        [HttpPost]
        public JsonResult Create(AddTestModel ModelInfo)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    StringBuilder ErrorMessage = new StringBuilder();
                    this.ModelState.ForEach(t =>
                    {
                        if (t.Value.Errors.Count > 0)
                            t.Value.Errors.ForEach(error => ErrorMessage.AppendLine(error.ErrorMessage));
                    });
                    return Json(BaseReturnInfo.Error(ErrorMessage.ToString()));
                }

                return Json(new BaseReturnInfo() { State = true, DataObject = TestDomainService.AddTest(ModelInfo.Map<AddTestModel, Test>()).Map<Test, DisplayTestModel>() });
            }
            catch (Exception ex)
            {
                RecordError(ex);
                return Json(BaseReturnInfo.Error(ex.Message));
            }
        }
    }
}