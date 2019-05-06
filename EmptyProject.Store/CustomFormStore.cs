using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class CustomFormStore : BaseDBContextRepository<CustomForm, Guid>, ICustomFormStore
	{ 
		public CustomFormStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
