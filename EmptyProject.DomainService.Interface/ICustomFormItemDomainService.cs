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
	public interface ICustomFormItemDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		CustomFormItem AddCustomFormItem(CustomFormItem Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<CustomFormItem> AddCustomFormItems(IList<CustomFormItem> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditCustomFormItem(CustomFormItem Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		CustomFormItem Single(Guid Id);

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
		void Removes(CustomFormItemCriteria CustomFormItemCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<CustomFormItem> All();

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
		bool IsExist(CustomFormItemCriteria CustomFormItemCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="CustomFormItemCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<CustomFormItem> GetCustomFormItemPaging(CustomFormItemCriteria CustomFormItemCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(CustomFormItemCriteria CustomFormItemCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="CustomFormItemCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<CustomFormItem> GetList(CustomFormItemCriteria CustomFormItemCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

        /// <summary>
        /// 获取类目下的自定义表单列表
        /// </summary>
        /// <param name="CustomFormId"></param>
        /// <returns></returns>
        IList<CustomFormItem> GetCustomFormItems(Guid CustomFormId);

        ///// <summary>
        ///// 获取类目下的自定义表单列表
        ///// </summary>
        ///// <param name="CustomFormId"></param>
        ///// <returns></returns>
        //IList<CustomFormItem> GetCustomFormItemsByKey(string CustomFormKey);

        ///// <summary>
        ///// 根据Keycode获取商品类目
        ///// </summary>
        ///// <param name="Keycode"></param>
        ///// <returns></returns>
        //CustomForm SingleByKeycode(string Keycode);

        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="SortId"></param>
        /// <param name="PreviousId"></param>
        /// <returns></returns>
        void SaveOrder(Guid CustomFormId, Guid SortId, Guid PreviousId);

        /// <summary>
        /// 设置表单项排序
        /// </summary>
        /// <param name="CustomFormId"></param>
        /// <param name="SortIds"></param>
        void SaveItemOrder(Guid CustomFormId,Guid[] SortIds);

        /// <summary>
        /// 获取排序值
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        int GetOrder(Guid FormId);

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="Id"></param>
        void Lock(Guid Id);

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="Id"></param>
        void Unlock(Guid Id);

	}
}
