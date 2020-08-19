
using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class CustomFormStepStore : BaseDBContextRepository<CustomFormStep, Guid>, ICustomFormStepStore
	{ 
		public CustomFormStepStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
