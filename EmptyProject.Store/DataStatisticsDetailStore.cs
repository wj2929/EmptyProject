using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class DataStatisticsDetailStore : BaseDBContextRepository<DataStatisticsDetail, Guid>, IDataStatisticsDetailStore
	{ 
		public DataStatisticsDetailStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
