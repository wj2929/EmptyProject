
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BC.Core;
using BC.DDD;
using BC.DDD.EntityFramework;
using System.Data.Entity.Infrastructure;
using EmptyProject.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace EmptyProject.Store.Models
{
    public class EmptyProjectDBContext : BCDBContext
    {
        public EmptyProjectDBContext()
            : this("EmptyProjectDBContext")
        {

        }

        /// <summary>
        /// Class
        /// </summary>
        public IDbSet<Test> Tests { get; set; }


        public EmptyProjectDBContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            Database.SetInitializer<EmptyProjectDBContext>(new CustomCreateDatabaseIfNotExists<EmptyProjectDBContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.AddFromAssembly(typeof(EmptyProjectDBContext).Assembly);
        }
    }
}
