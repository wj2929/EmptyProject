using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmptyProject.Core
{
    public class EnumHelper
    {
        public static IList<EnumInfo> GetEnumInfos(Type EnumType)
        {
            IList<EnumInfo> infos = new List<EnumInfo>();
            System.Array enumArr = System.Enum.GetValues(EnumType);
            foreach (int Item in enumArr)
            {
                infos.Add(new EnumInfo
                {
                    Text = System.Enum.GetName(EnumType, Item),
                    Value = Item.ToString(),
                }
                );
            }
            return infos;
        }

        public static IList<string> ToTextArray<T>()
        {
            IList<string> texts = new List<string>();

            foreach (int s in System.Enum.GetValues(typeof(T)))
            {
                texts.Add(System.Enum.GetName(typeof(T), s));
            }

            return texts;
        }
    }

    public class EnumInfo
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
