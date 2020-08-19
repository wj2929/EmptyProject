using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class DataStatisticsStore : BaseDBContextRepository<DataStatistics, Guid>, IDataStatisticsStore
	{ 
		public DataStatisticsStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
