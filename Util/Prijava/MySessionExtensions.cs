using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ServisApp.Util.Prijava
{
    public static class MySessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
