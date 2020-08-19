
using System;
using EmptyProject.Domain;
using BC.DDD;
namespace EmptyProject.Store.Interface
{
	public partial interface ICustomFormStepStore : IRepository<CustomFormStep, Guid>
	{ 
	}
}
