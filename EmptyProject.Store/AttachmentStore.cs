using System;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using BC.DDD.EntityFramework;
using BC.DDD;
namespace EmptyProject.Store
{
	internal partial class AttachmentStore : BaseDBContextRepository<Attachment, Guid>, IAttachmentStore
	{ 
		public AttachmentStore(IDbContextFactory DatabaseFactory) : base(DatabaseFactory) 
		{
 
		}
	}
}
