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
	public interface ICategoryDomainService
	{

        #region AutoCode
        /// <summary>
        /// 添加一条信息
        /// </summary>		
        Category AddCategory(Category Info);

        /// <summary>
        /// 添加多条信息
        /// </summary>		
        IList<Category> AddCategorys(IList<Category> Info);

        /// <summary>
        /// 编辑一条信息
        /// </summary>		
        void EditCategory(Category Info);

        /// <summary>
        /// 读取一条数据，如数据不存在，返回null
        /// </summary>		
        Category Single(Guid Id);

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
        void Removes(CategoryCriteria CategoryCriteria);

        /// <summary>
        /// 获得所有信息
        /// </summary>		
        IList<Category> All();

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
        bool IsExist(CategoryCriteria CategoryCriteria);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="CategoryCriteria"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        ReturnPaging<Category> GetCategoryPaging(CategoryCriteria CategoryCriteria, int PageNum = 1, int PageSize = 20);

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="WasteCertificateCriteria"></param>
        /// <returns></returns>
        int Count(CategoryCriteria CategoryCriteria);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CategoryCriteria"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        IList<Category> GetList(CategoryCriteria CategoryCriteria, int PageNum = 1, int PageSize = int.MaxValue);
        #endregion

        /// <summary>
        /// 添加一条信息
        /// </summary>		
        Category AddCategory(Category info, Guid? ParentCategory_Id);

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentCategory"></param>
        /// <returns></returns>
        Category AddCategoryWithParent(Category info, Category ParentCategory);

        /// <summary>
        /// 添加多条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentCategory"></param>
        IList<Category> AddCategorysWithParent(IList<Category> infos, Category ParentCategory);

        /// <summary>
        /// 获取指定类型所有分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<Category> GetAllCategorys(string Type);

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<Category> GetRootCategorys(string Type);

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<Category> GetRootCategorys(Guid CategoryTypeId);

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<Category> GetRootCategoryWithCategoryTypeKeycodes(string CategoryTypeKeycode);

        /// <summary>
        /// 获取子分类列表
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        IList<Category> GetChildCategorys(Guid ParentId);

        /// <summary>
        /// 存在子分类
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        bool ExistChildCategory(Guid ParentId);
        /// <summary>
        /// 获取指定Level的分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Indexs"></param>
        /// <returns></returns>
        IList<Category> GetCategorysByLevels(string Type, string IndexHeader, params int[] Levels);

        /// <summary>
        /// 删除某分类类型下的所有分类数据
        /// </summary>
        /// <param name="Type"></param>
        void Removes(string Type);

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        IList<Category> GetCategorys(Guid[] Ids);
        /// <summary>
        /// 获取分类的层级关系
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        string GetCategorys(Guid CategoryId);

        /// <summary>
        /// 获取分类的层级关系
        /// </summary>
        /// <param name="CategoryIndex"></param>
        /// <returns></returns>
        IList<Category> GetCategorys(string CategoryIndex);


        /// <summary>
        /// 获取指定类型、名称的根分类
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        Category GetRootCategory(string Type, string Name);

        /// <summary>
        /// 获取排序值
        /// </summary>
        /// <param name="Type"></param>
        int GetOrder(Guid CategoryTypeId, Guid? ParentCategory_Id);

        /// <summary>
        /// 设置表单项排序
        /// </summary>
        /// <param name="CustomFormId"></param>
        /// <param name="SortIds"></param>
        void SaveOrder(Guid CategoryTypeId, Guid? ParentCategory_Id, Guid[] SortIds);
    }
}
