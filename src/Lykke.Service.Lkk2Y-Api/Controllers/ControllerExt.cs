using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Lykke.Service.Lkk2Y_Api.Controllers
{
    public static class ControllerExt
    {



        public static async Task<string> BodyAsStringAsync(this HttpRequest httpRequest){

            try
            {
                httpRequest.Body.Position = 0;
                
                using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }

            }
            catch (Exception){
                return "";
            }

        }
        
        private static T GetHeaderValueAs<T>(HttpContext httpContext, string headerName)
        {
            StringValues values;

            if (httpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
        
        private static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }
        
        public static string GetIp(this Controller ctx)
        {
            string ip = string.Empty;

            // http://stackoverflow.com/a/43554000/538763
            var xForwardedForVal = GetHeaderValueAs<string>(ctx.HttpContext, "X-Forwarded-For").SplitCsv().FirstOrDefault();

            if (!string.IsNullOrEmpty(xForwardedForVal))
            {
                ip = xForwardedForVal.Split(':')[0];
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && ctx.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = ctx.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>(ctx.HttpContext, "REMOTE_ADDR");

            return ip;
        }

    }
}
