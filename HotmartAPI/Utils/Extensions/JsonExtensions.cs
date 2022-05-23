using HotmartAPI.Helpers;

namespace HotmartAPI.Utils.Extensions
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this object obj)
        {
            return JsonHelper.Deserialize<T>(obj.ToString());
        }

        public static string SerializeToJson(this object obj)
        {
            return JsonHelper.Serialize(obj);
        }
    }
}
