using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class UserAccessTokenStore : BaseDBContextRepository<UserAccessToken, Guid>, IUserAccessTokenStore
	{ 
		public UserAccessTokenStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
