using BC.Core;
using EmptyProject.Domain;
using EmptyProject.Domain.Values.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.DataFill
{
    public class BaseData<TContext> where TContext : global::System.Data.Entity.DbContext
    {
        public void Fill(TContext context)
        {
            try
            {
                Test TestInfo = new Test() { Name = "Test" };
                TestInfo = context.Set<Test>().Add(TestInfo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
            }
        }
    }
}
