using BC.DDD;
using BC.DDD.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models
{
    public class DatabaseFactory : Disposable, IDbContextFactory
    {
        private EmptyProjectDBContext mDBContext;
        public DbContext Get()
        {
            return this.mDBContext ?? (this.mDBContext = new EmptyProjectDBContext());
        }

        public void Dispose()
        {
            if (this.mDBContext != null)
                this.mDBContext.Dispose();
        }


        public DbContext Instance()
        {
            return new EmptyProjectDBContext();
        }
    }
}
