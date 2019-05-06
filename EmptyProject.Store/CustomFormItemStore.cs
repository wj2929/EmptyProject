using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class CustomFormItemStore : BaseDBContextRepository<CustomFormItem, Guid>, ICustomFormItemStore
	{ 
		public CustomFormItemStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
