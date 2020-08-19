
using System;
using EmptyProject.Domain;
using BC.DDD;
namespace EmptyProject.Store.Interface
{
	public partial interface IUserThirdLoginHistoryStore : IRepository<UserThirdLoginHistory, Guid>
	{ 
	}
}
