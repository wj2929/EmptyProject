using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class CategoryTypeStore : BaseDBContextRepository<CategoryType, Guid>, ICategoryTypeStore
	{ 
		public CategoryTypeStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
