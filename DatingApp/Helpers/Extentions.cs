using Microsoft.AspNetCore.Http;

namespace DatingApp.helpers
{
    public static class Extentions
    {
        public static void AddApplicationError(this HttpResponse response, string messgae){
            response.Headers.Add("Application-Error",messgae);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }
    }
}