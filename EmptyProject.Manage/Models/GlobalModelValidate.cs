using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmptyProject.Manage.Models
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class GuidNotEmptyAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessageString, name);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            Guid valueAsGuid = value.ToString().GuidByString();
            return !valueAsGuid.IsEmpty();
        }

        /// <summary>
        /// 客户端验证
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]{
                new ModelClientValidationRule(){
                    ValidationType="guidnotempty",
                    ErrorMessage=FormatErrorMessage(metadata.GetDisplayName())
                }
            };
        }
    }
}