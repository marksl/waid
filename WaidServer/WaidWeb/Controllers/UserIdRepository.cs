using System;
using System.Web;

namespace WaidWeb.Controllers
{
    public class UserIdRepository
    {
        public static bool TryGetUserId(out Guid userId)
        {
            var cookie = HttpContext.Current.Request.Cookies["userid"];
            if (cookie != null && Guid.TryParse(cookie.Value, out userId))
            {
                return true;
            }

            userId = Guid.Empty;
            return false;
        }

        public static void Set(string userName, Guid uploadId, HttpResponseBase response)
        {
            var cookie = new HttpCookie("userid", uploadId.ToString());
            response.Cookies.Add(cookie);
        }
    }
}