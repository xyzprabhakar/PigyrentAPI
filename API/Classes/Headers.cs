
using API.Models;
using System.Reflection;

namespace API.Classes
{
    public class Headers 
    {
        public static void BindLanguage<T>(dtoHeaders header,T modelData)
        {
            if (modelData == null)
            {
                return;
            }
            Type t = modelData.GetType();
            PropertyInfo? propAllLangulage = t.GetProperty("IncludeAllLanguage");
            if (propAllLangulage != null)
            {
                propAllLangulage.SetValue(modelData, header.IncludeAllLanguage);
            }

            PropertyInfo? propLangulage = t.GetProperty("Language");
            if (propLangulage != null)
            {
                var listData = propLangulage.GetValue(modelData) as Google.Protobuf.Collections.RepeatedField<string>;                
                if (listData != null)
                {
                    if (header.Language != null)
                    {
                        listData.AddRange(header.Language.Split(","));
                    }
                }                
            }
        }
    }

}
