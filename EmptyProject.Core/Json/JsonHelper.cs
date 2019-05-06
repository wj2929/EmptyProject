using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BaseCommon.Core.Json
{
    public static class JsonHelper
    {
        public static string SerializeObject(this object Obj)
        {
            return JsonConvert.SerializeObject(Obj);
        }

        public static T DeserializeObject<T>(this string JsonStr)
        {
            return JsonConvert.DeserializeObject<T>(JsonStr);
        }

        //public static T DeserializeObject<T>(this string JsonStr, JsonSerializerSettings Settings)
        //{
        //    return JsonConvert.DeserializeObject<T>(JsonStr, Settings);
        //}
    }
}
