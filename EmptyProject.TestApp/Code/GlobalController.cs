using BC.DDD.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BC.IoC.WinFormOnly.BaseCache;
using AutoMapper;
using EmptyProject.Domain.Values.ImportModes;
using EmptyProject.Domain;

namespace EmptyProject.TestApp.Code
{
    public class GlobalController
    {
        private static IDependencyResolver _IoC;
        /// <summary>
        /// 依赖注入
        /// </summary>
        public static IDependencyResolver IoC
        {
            get
            {
                if (_IoC == null)
                    _IoC = IoCManage.Current;

                return _IoC;
            }
        }

        /// <summary>
        /// 缓存管理
        /// </summary>
        private static ICacheManager<string, object> _CacheManager;
        static public ICacheManager<string, object> CacheManager
        {
            get
            {
                if (_CacheManager == null)
                    _CacheManager = new FullCache();

                return _CacheManager;
            }
        }

        public static void InjectCustomMap()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;

                cfg.CreateMap<ImportStudentModel, Student>()
                    .ForMember(t => t.UserName, dto => dto.MapFrom(t => t.XH))
                    .ForMember(t => t.FullName, dto => dto.MapFrom(t => t.Name))
                    .ForMember(t => t.Sex, dto => dto.MapFrom(t => t.Sex == "男" ? 0 : 1));

                cfg.CreateMap<ImportTeacherModel, Teacher>()
                    .ForMember(t => t.UserName, dto => dto.MapFrom(t => t.JobNumber))
                    .ForMember(t => t.FullName, dto => dto.MapFrom(t => t.Name))
                    .ForMember(t => t.Sex, dto => dto.MapFrom(t => t.Sex == "男" ? 0 : 1));

            });
        }
    }
}
