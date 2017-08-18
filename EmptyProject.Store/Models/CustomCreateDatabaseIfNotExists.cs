using EmptyProject.Store.Models.DataFill;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models
{
    public class CustomCreateDatabaseIfNotExists<TContext> : CreateDatabaseIfNotExists<TContext>
        where TContext : System.Data.Entity.DbContext
    {
        protected override void Seed(TContext context)
        {
            new BaseData<TContext>().Fill(context);
        }
    }
}
