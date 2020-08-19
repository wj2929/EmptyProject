
using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class UserThirdLoginExtendStore : BaseDBContextRepository<UserThirdLoginExtend, Guid>, IUserThirdLoginExtendStore
	{ 
		public UserThirdLoginExtendStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
