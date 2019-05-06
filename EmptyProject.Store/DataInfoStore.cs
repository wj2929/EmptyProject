using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class DataInfoStore : BaseDBContextRepository<DataInfo, Guid>, IDataInfoStore
	{ 
		public DataInfoStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
