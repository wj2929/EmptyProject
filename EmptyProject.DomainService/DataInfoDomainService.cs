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
	internal class DataInfoDomainService : BaseDomainService, IDataInfoDomainService
	{ 
		public DataInfoDomainService(IDataInfoStore DataInfoStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.DataInfoStore = DataInfoStore;
		}
		private readonly IDataInfoStore DataInfoStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public DataInfo AddDataInfo(DataInfo AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.DataInfoStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<DataInfo> AddDataInfos(IList<DataInfo> Infos)
		{
            Infos.ForEach(t => 
            {
                this.DataInfoStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditDataInfo(DataInfo Info)
		{
			this.DataInfoStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public DataInfo Single(Guid Id)
		{
			return this.DataInfoStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
            //this.DataInfoStore.Remove(Id);
            //this.SaveChanage();

            string Index = this.DataInfoStore.Single(Id).Index;
            this.DataInfoStore.Remove(t => t.Index.Contains(Index));
            this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
            //if (Ids.Length > 0)
            //{
            //    this.DataInfoStore.Remove(Ids);
            //    this.SaveChanage();
            //}

            if (Ids.Length > 0)
            {
                string[] Indexs = this.DataInfoStore.Find(t => Ids.Contains(t.Id)).Select(t => t.Index).ToArray();
                Indexs.ForEach(Index => this.DataInfoStore.Remove(t => t.Index.Contains(Index)));
                //this.CategoryStore.Remove(Ids);
                this.SaveChanage();
            }

		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(DataInfoCriteria DataInfoCriteria)
		{
			this.DataInfoStore.Remove(DataInfoCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<DataInfo> All()
		{
			return this.DataInfoStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataInfoCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<DataInfo> GetDataInfoPaging(DataInfoCriteria DataInfoCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(DataInfoCriteria);

			ReturnPaging<DataInfo> returnPaging = new ReturnPaging<DataInfo>();
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
		/// <param name="DataInfoCriteria"></param>
		/// <returns></returns>
		public int Count(DataInfoCriteria DataInfoCriteria)
		{
			return GetQueryable(DataInfoCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataInfoCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<DataInfo> GetList(DataInfoCriteria DataInfoCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(DataInfoCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(DataInfoCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.DataInfoStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(DataInfoCriteria DataInfoCriteria)
		{
			return Count(DataInfoCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<DataInfo> GetQueryable(DataInfoCriteria DataInfoCriteria)
        {
            return this.DataInfoStore.Find(DataInfoCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 获取数据的层级关系
        /// </summary>
        /// <param name="DataInfoId"></param>
        /// <returns></returns>
        public string GetDataInfos(Guid DataInfoId)
        {
            string retStr = string.Empty;

            DataInfo cur = this.Single(DataInfoId);

            return string.Join(",", GetDataInfos(cur.Index).Select(c => c.Name).ToArray());
        }

        /// <summary>
        /// 获取数据的层级关系
        /// </summary>
        /// <param name="DataIndex"></param>
        /// <returns></returns>
        public IList<DataInfo> GetDataInfos(string DataIndex)
        {
            Guid[] DataInfoIds = DataIndex.Trim(',').Split(',').Select(t => t.GuidByString()).ToArray();
            return this.DataInfoStore.Find(c => DataInfoIds.Contains(c.Id)).OrderBy(c => c.Level).ToList();
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>		
        public DataInfo AddDataInfo(DataInfo info, Guid? ParentDataInfo_Id)
        {
            if (ParentDataInfo_Id.HasValue && !ParentDataInfo_Id.Value.IsEmpty())
            {
                DataInfo ParentDataInfo = this.DataInfoStore.Single(ParentDataInfo_Id.Value);
                info.ParentDataInfo_Id = ParentDataInfo_Id.Value;
                info.Index = ParentDataInfo.Index + info.Id + ",";
                info.Level = ParentDataInfo.Level + 1;
            }
            else
            {
                info.Index = "," + info.Id + ",";
                info.Level = 0;
                info.ParentDataInfo_Id = null;
            }
            info = this.DataInfoStore.Add(info);
            this.SaveChanage();
            return info;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentDataInfo"></param>
        /// <returns></returns>
        public DataInfo AddDataInfoWithParent(DataInfo info, DataInfo ParentDataInfo)
        {
            info.ParentDataInfo_Id = ParentDataInfo.Id;
            info.Index = ParentDataInfo.Index + info.Id + ",";
            info.Level = ParentDataInfo.Level + 1;

            info = this.DataInfoStore.Add(info);
            this.SaveChanage();
            return info;
        }

        /// <summary>
        /// 添加多条信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ParentDataInfo"></param>
        public void AddDataInfosWithParent(IList<DataInfo> infos, DataInfo ParentDataInfo)
        {
            if (infos.Count > 0)
            {
                foreach (var info in infos)
                {
                    info.ParentDataInfo_Id = ParentDataInfo.Id;
                    info.Index = ParentDataInfo.Index + info.Id + ",";
                    info.Level = ParentDataInfo.Level + 1;

                    this.DataInfoStore.Add(info);
                }
                this.SaveChanage();
            }
        }
	}
}
