using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class CategoryStore : BaseDBContextRepository<Category, Guid>, ICategoryStore
	{ 
		public CategoryStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{

        }
	}
}
