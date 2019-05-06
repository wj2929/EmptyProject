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
	public interface ICustomFormDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		CustomForm AddCustomForm(CustomForm Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<CustomForm> AddCustomForms(IList<CustomForm> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditCustomForm(CustomForm Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		CustomForm Single(Guid Id);

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
		void Removes(CustomFormCriteria CustomFormCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<CustomForm> All();

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
		bool IsExist(CustomFormCriteria CustomFormCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="CustomFormCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<CustomForm> GetCustomFormPaging(CustomFormCriteria CustomFormCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(CustomFormCriteria CustomFormCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="CustomFormCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<CustomForm> GetList(CustomFormCriteria CustomFormCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

        /// <summary>
        /// 根据Keycode获取商品类目
        /// </summary>
        /// <param name="Keycode"></param>
        /// <returns></returns>
        CustomForm SingleByKeycode(string Keycode);
	}
}
