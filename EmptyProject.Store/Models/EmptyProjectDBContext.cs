
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
namespace EmptyProject.Store.Models
{
    public class EmptyProjectDBContext : BCDBContext
    {
        public EmptyProjectDBContext()
            : this("EmptyProjectDBContext")
        {

        }

        /// <summary>
        /// Attachment
        /// </summary>
        public IDbSet<Attachment> Attachments { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public IDbSet<Category> Categories { get; set; }

        /// <summary>
        /// CategoryType
        /// </summary>
        public IDbSet<CategoryType> CategoryTypes { get; set; }

        /// <summary>
        /// CustomForm
        /// </summary>
        public IDbSet<CustomForm> CustomForms { get; set; }

        /// <summary>
        /// CustomFormItem
        /// </summary>
        public IDbSet<CustomFormItem> CustomFormItems { get; set; }

        /// <summary>
        /// DataInfo
        /// </summary>
        public IDbSet<DataInfo> DataInfoes { get; set; }

        /// <summary>
        /// Test
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
            modelBuilder.Configurations.AddFromAssembly(typeof(EmptyProjectDBContext).Assembly);
        }
    }
}
