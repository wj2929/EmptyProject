using BC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain.Values.Configs
{
    public class GlobalConfig : IConfigBase<GlobalConfig>
    {
        [Display(Name = "前站地址", Order =1)]
        [Required]
        public string WebUrl { get; set; }

        [Display(Name = "测试模式", Order = 2)]
        public bool DebugMode { get; set; }

        [Display(Name = "每天每Ip最大发送验证码数", Order = 3)]
        public int MaxSendSMSCodeCountPerDayPerIp { get; set; }

        [Display(Name = "验证码过期时间（秒）", Order = 4)]
        public int VerificationCodeExpiresSecond { get; set; }

        [Display(Name = "首页分享描述", Order = 5)]
        [Required]
        public string HomeShareDescription { get; set; }

        public string ToConfig()
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("WebUrl", WebUrl);
            dic.Add("DebugMode", DebugMode.ToString());
            dic.Add("MaxSendSMSCodeCountPerDayPerIp", MaxSendSMSCodeCountPerDayPerIp.ToString());
            dic.Add("VerificationCodeExpiresSecond", VerificationCodeExpiresSecond.ToString());
            dic.Add("HomeShareDescription", HomeShareDescription.ToString());
            return dic.ToConfig("GlobalConfig");
        }

        public GlobalConfig FromConfig(string Config)
        {
            GlobalConfig info = new GlobalConfig();

            if (Config.IsEmpty())
                return info;

            info.WebUrl = Config.GetTag("WebUrl");
            info.DebugMode = Config.GetTag("DebugMode").BoolByString();
            info.MaxSendSMSCodeCountPerDayPerIp = Config.GetTag("MaxSendSMSCodeCountPerDayPerIp").IntByString();
            if (info.MaxSendSMSCodeCountPerDayPerIp == 0)
                info.MaxSendSMSCodeCountPerDayPerIp = 50;
            info.VerificationCodeExpiresSecond = Config.GetTag("VerificationCodeExpiresSecond").IntByString();
            if (info.MaxSendSMSCodeCountPerDayPerIp == 0)
                info.MaxSendSMSCodeCountPerDayPerIp = 600;

            info.HomeShareDescription = Config.GetTag("HomeShareDescription");

            return info;
        }
    }
}
