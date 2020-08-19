
using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class UserThirdLoginHistoryStore : BaseDBContextRepository<UserThirdLoginHistory, Guid>, IUserThirdLoginHistoryStore
	{ 
		public UserThirdLoginHistoryStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
