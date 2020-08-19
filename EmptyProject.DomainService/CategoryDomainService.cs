using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using BC.Core;
using BC.DDD;
using BC.DDD.Specification;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using EmptyProject.DomainService.Interface;
using EmptyProject.Domain.QueryObject;

namespace EmptyProject.DomainService
{
    internal class CategoryDomainService : BaseDomainService, ICategoryDomainService
    {
        public CategoryDomainService(ICategoryStore CategoryStore,
            IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        {
            this.CategoryStore = CategoryStore;
        }
        private readonly ICategoryStore CategoryStore;

        #region AutoCode
        /// <summary>
        /// 添加一条信息
        /// </summary>		
        public Category AddCategory(Category AddInfo)
        {
            if (AddInfo == null)
                throw new ArgumentNullException("AddInfo");

            AddInfo = this.CategoryStore.Add(AddInfo);
            this.SaveChanage();
            return AddInfo;
        }

        /// <summary>
        /// 添加多条信息
        /// </summary>		
        public IList<Category> AddCategorys(IList<Category> Infos)
        {
            Infos.ForEach(t =>
            {
                this.CategoryStore.Add(t);
            });
            this.SaveChanage();
            return Infos;
        }

        /// <summary>
        /// 编辑一条信息
        /// </summary>		
        public void EditCategory(Category Info)
        {
            this.CategoryStore.Edit(Info);
            this.SaveChanage();
        }

        /// <summary>
        /// 读取一条数据，如数据不存在，返回null
        /// </summary>		
        public Category Single(Guid Id)
        {
            return this.CategoryStore.Single(Id);
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>		
        public void Remove(Guid Id)
        {
            string Index = this.CategoryStore.Single(Id).Index;
            this.CategoryStore.Remove(t => t.Index.Contains(Index));
            this.SaveChanage();
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(Guid[] Ids)
        {
            if (Ids.Length > 0)
            {
                string[] Indexs = this.CategoryStore.Find(t => Ids.Contains(t.Id)).Select(t => t.Index).ToArray();
                Indexs.ForEach(Index => this.CategoryStore.Remove(t => t.Index.Contains(Index)));
                //this.CategoryStore.Remove(Ids);
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(CategoryCriteria CategoryCriteria)
        {
            this.CategoryStore.Remove(CategoryCriteria.Query);
            this.SaveChanage();
        }


        /// <summary>
        /// 获得所有信息
        /// </summary>		
        public IList<Category> All()
        {
            return this.CategoryStore.All().ToList();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="CategoryCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public ReturnPaging<Category> GetCategoryPaging(CategoryCriteria CategoryCriteria, int PageNum = 1, int PageSize = 20)
        {
            var q = GetQueryable(CategoryCriteria);

            ReturnPaging<Category> returnPaging = new ReturnPaging<Category>();
            Module_Page _Pages = new Module_Page();
            _Pages.PageNum = PageNum;
            _Pages.AllCount = q.Count();
            _Pages.PageSize = PageSize;
            _Pages.Compute();
            returnPaging.Module_Page = _Pages;
            returnPaging.PageListInfos = q.OrderByDescending(c => c.CreateDate).Skip(_Pages.First).Take(_Pages.Max).ToList();
            return returnPaging;
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="CategoryCriteria"></param>
        /// <returns></returns>
        public int Count(CategoryCriteria CategoryCriteria)
        {
            return GetQueryable(CategoryCriteria).Count();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CategoryCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public IList<Category> GetList(CategoryCriteria CategoryCriteria, int PageNum = 1, int PageSize = int.MaxValue)
        {
            PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(CategoryCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
                return GetQueryable(CategoryCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 检查Id是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(Guid Id)
        {
            return this.CategoryStore.IsExist(t => t.Id == Id);
        }

        /// <summary>
        /// 检查查询表达式是否存在记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(CategoryCriteria CategoryCriteria)
        {
            return Count(CategoryCriteria) > 0;
        }

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<Category> GetQueryable(CategoryCriteria CategoryCriteria)
        {
            return this.CategoryStore.Find(CategoryCriteria.Query);
        }

        #endregion

        ///// <summary>
        ///// 添加一条信息
        ///// </summary>		
        //public Category AddCategory(Category info, Guid ParentId)
        //{
        //    if (!ParentId.IsEmpty())
        //    {
        //        Category ParentCategory = this.CategoryStore.Single(ParentId);
        //        info.ParentCategory_Id = ParentId;
        //        info.Index = ParentCategory.Index + info.Id + ",";
        //        info.Level = ParentCategory.Level + 1;
        //    }
        //    else
        //    {
        //        info.Index = "," + info.Id + ",";
        //        info.Level = 0;
        //    }
        //    info = this.CategoryStore.Add(info);
        //    this.SaveChanage();
        //    return info;
        //}


        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentCategory"></param>
        /// <returns></returns>
        public Category AddCategoryWithParent(Category info, Category ParentCategory)
        {
            info.ParentCategory_Id = ParentCategory.Id;
            info.Index = ParentCategory.Index + info.Id + ",";
            info.Level = ParentCategory.Level + 1;

            info.OrderBy = GetOrder(info.CategoryType_Id, ParentCategory.Id);

            info = this.CategoryStore.Add(info);
            this.SaveChanage();
            return info;
        }

        /// <summary>
        /// 添加多条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentCategory"></param>
        public IList<Category> AddCategorysWithParent(IList<Category> infos, Category ParentCategory)
        {
            if (infos.Count > 0)
            {
                Guid? ParentCategoryId = null;
                if (ParentCategory != null)
                    ParentCategoryId = ParentCategory.Id;

                int Order = GetOrder(infos.Select(t => t.CategoryType_Id).First(), ParentCategoryId);

                foreach (var info in infos)
                {
                    info.OrderBy = Order++;
                    info.ParentCategory_Id = ParentCategoryId;
                    info.Index = (ParentCategoryId.HasValue ? ParentCategory.Index : ",") + info.Id + ",";
                    info.Level = (ParentCategoryId.HasValue ? ParentCategory.Level : 0) + 1;

                    this.CategoryStore.Add(info);
                }
                this.SaveChanage();
            }

            return infos;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>		
        public Category AddCategory(Category info, Guid? ParentCategory_Id)
        {
            if (ParentCategory_Id.HasValue && !ParentCategory_Id.Value.IsEmpty())
            {
                Category ParentCategory = this.CategoryStore.Single(ParentCategory_Id.Value);
                info.ParentCategory_Id = ParentCategory_Id.Value;
                info.Index = ParentCategory.Index + info.Id + ",";
                info.Level = ParentCategory.Level + 1;
            }
            else
            {
                info.Index = "," + info.Id + ",";
                info.Level = 0;
                info.ParentCategory_Id = null;
            }
            info.OrderBy = GetOrder(info.CategoryType_Id, ParentCategory_Id);
            info = this.CategoryStore.Add(info);
            this.SaveChanage();
            return info;
        }

        /// <summary>
        /// 获取指定类型所有分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<Category> GetAllCategorys(string Type)
        {
            return this.CategoryStore.Find(c => c.Type == Type).ToList();
        }

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<Category> GetRootCategorys(string Type)
        {
            return this.CategoryStore.Find(c => c.Type == Type && (c.ParentCategory_Id == null || c.ParentCategory_Id == Guid.Empty)).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
        }

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<Category> GetRootCategorys(Guid CategoryTypeId)
        {
            return this.CategoryStore.Find(c => c.CategoryType_Id == CategoryTypeId && (c.ParentCategory_Id == null || c.ParentCategory_Id == Guid.Empty)).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
        }

        /// <summary>
        /// 获取指定类型根分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<Category> GetRootCategoryWithCategoryTypeKeycodes(string CategoryTypeKeycode)
        {
            return this.CategoryStore.Find(c => c.CategoryType.Keycode == CategoryTypeKeycode && (c.ParentCategory_Id == null || c.ParentCategory_Id == Guid.Empty)).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
        }

        /// <summary>
        /// 获取子分类列表
        /// </summary>
        /// <param name="ParentCategory_Id"></param>
        /// <returns></returns>
        public IList<Category> GetChildCategorys(Guid ParentCategory_Id)
        {
            return this.CategoryStore.Find(c => c.ParentCategory_Id != null && c.ParentCategory_Id.Value == ParentCategory_Id).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
        }

        /// <summary>
        /// 存在子分类
        /// </summary>
        /// <param name="ParentCategory_Id"></param>
        /// <returns></returns>
        public bool ExistChildCategory(Guid ParentCategory_Id)
        {
            return this.CategoryStore.IsExist(c => c.ParentCategory_Id != null && c.ParentCategory_Id.Value == ParentCategory_Id);
        }


        /// <summary>
        /// 删除某分类类型下的所有分类数据
        /// </summary>
        /// <param name="Type"></param>
        public void Removes(string Type)
        {
            this.CategoryStore.Remove(c => c.Type == Type);
            this.SaveChanage();
        }

        /// <summary>
        /// 获取指定Level的分类列表
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Levels"></param>
        /// <returns></returns>
        public IList<Category> GetCategorysByLevels(string Type, string IndexHeader, params int[] Levels)
        {
            ISpecification<Category> Specification = new Specification<Category>(c => c.Type == Type);

            if (!IndexHeader.IsEmpty())
            {
                Specification = Specification.And(c => c.Index.Contains(IndexHeader));
            }

            if (Levels.Length > 0)
            {
                int Level = Levels[0];
                ISpecification<Category> SpecificationOr = new Specification<Category>(c => c.Level == Level);
                for (int i = 1; i < Levels.Length; i++)
                {
                    int level = Levels[i];
                    SpecificationOr = SpecificationOr.Or(c => c.Level == level);
                }
                Specification = Specification.And(SpecificationOr);
            }



            return this.CategoryStore.Specification(Specification).ToList();
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public IList<Category> GetCategorys(Guid[] Ids)
        {
            return this.CategoryStore.Find(c => Ids.Contains(c.Id)).ToList();
        }
        /// <summary>
        /// 获取分类的层级关系
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public string GetCategorys(Guid CategoryId)
        {
            string retStr = string.Empty;

            Category cur = this.Single(CategoryId);

            return string.Join(",", GetCategorys(cur.Index).Select(c => c.Name).ToArray());
        }

        /// <summary>
        /// 获取分类的层级关系
        /// </summary>
        /// <param name="CategoryIndex"></param>
        /// <returns></returns>
        public IList<Category> GetCategorys(string CategoryIndex)
        {
            Guid[] CategoryIds = CategoryIndex.Trim(',').Split(',').Select(t => t.GuidByString()).ToArray();
            return this.CategoryStore.Find(c => CategoryIds.Contains(c.Id)).OrderBy(c => c.Level).ToList();
        }


        /// <summary>
        /// 获取指定类型、名称的根分类
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Category GetRootCategory(string Type, string Name)
        {
            return this.CategoryStore.Single(c => c.Type == Type && c.Name == Name && c.Level == 0);
        }

        /// <summary>
        /// 获取排序值
        /// </summary>
        /// <param name="Type"></param>
        public int GetOrder(Guid CategoryTypeId, Guid? ParentCategory_Id)
        {
            var q = this.CategoryStore.Find(t => t.CategoryType_Id == CategoryTypeId && t.ParentCategory_Id == ParentCategory_Id)
                .OrderByDescending(t => t.OrderBy)
                .Select(t => t.OrderBy);
            return q.Count() == 0 ? 0 : q.First() + 1;
        }

        /// <summary>
        /// 设置表单项排序
        /// </summary>
        /// <param name="CustomFormId"></param>
        /// <param name="SortIds"></param>
        public void SaveOrder(Guid CategoryTypeId, Guid? ParentCategory_Id, Guid[] SortIds)
        {
            if (SortIds.Length > 0)
            {
                IList<Category> Categorys = GetList(new CategoryCriteria() { CategoryTypeId = CategoryTypeId, ParentCategory_Id = ParentCategory_Id });
                if (Categorys.Where(t => SortIds.Contains(t.Id)).Count() == SortIds.Length)
                {
                    IDictionary<Guid, Category> CategoryDic = Categorys.ToDictionary(t => t.Id, t => t);
                    int i = 0;
                    SortIds.ForEach(t =>
                    {
                        CategoryDic[t].OrderBy = i++;
                        this.CategoryStore.Edit(CategoryDic[t]);
                    });
                    this.SaveChanage();
                }
            }
        }
    }
}
