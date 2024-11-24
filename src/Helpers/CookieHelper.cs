using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IDWM_TallerAPI.Src.Helpers
{
    public static class CookieHelper
    {
        public static void SetCookie(HttpResponse response, string key, object value, int? expireTime = null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (expireTime.HasValue)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddMinutes(expireTime.Value);
            }

            var serializedValue = JsonConvert.SerializeObject(value);
            response.Cookies.Append(key, serializedValue, cookieOptions);
        }

        public static T? GetCookie<T>(HttpRequest request, string key)
        {
            var cookie = request.Cookies[key];
            return cookie == null ? default : JsonConvert.DeserializeObject<T>(cookie);
        }
    }
}
