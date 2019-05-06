
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EmptyProject.Domain;
using BC.Core;
using BC.DDD;
using EmptyProject.Domain.QueryObject;

namespace EmptyProject.DomainService.Interface
{
	public interface IDataInfoDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		DataInfo AddDataInfo(DataInfo Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<DataInfo> AddDataInfos(IList<DataInfo> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditDataInfo(DataInfo Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		DataInfo Single(Guid Id);

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		void Remove(Guid Id);

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		void Removes(Guid[] Ids);

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		void Removes(DataInfoCriteria DataInfoCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<DataInfo> All();

		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		bool IsExist(Guid Id);

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		bool IsExist(DataInfoCriteria DataInfoCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataInfoCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<DataInfo> GetDataInfoPaging(DataInfoCriteria DataInfoCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(DataInfoCriteria DataInfoCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataInfoCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<DataInfo> GetList(DataInfoCriteria DataInfoCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

	}
}
