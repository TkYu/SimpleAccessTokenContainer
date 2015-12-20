using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleAccessTokenContainer
{
    public static class WeixinJSSDKTicket
    {
        private const string KeyName = "WeixinJSSDKTicket";
        private const string ExpTimeName = "WeixinJSSDKTicketExpTime";

        public static string GetNewToken(string appid, string secret)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={WeixinAccessToken.GetToken(appid, secret)}&type=jsapi";
            var ret = Utils.GetString(url);
            if (!ret.Contains("\"errcode\":0"))throw new Exception("error return code");
            var regx = new Regex(@"^.*?""ticket"":""(.*?)"",""expires_in"":(\d+)\}$");
            var mt = regx.Match(ret);
            if (!mt.Success) throw new InvalidOperationException("wrong return");
            Utils.WriteValue(appid, KeyName, mt.Groups[1].Value);
            Utils.WriteValue(appid, ExpTimeName, DateTime.UtcNow.AddSeconds(int.Parse(mt.Groups[2].Value) - 120).ToString("yyyyMMddHHmmss"));
            return mt.Groups[1].Value;
        }

        public static string GetToken(string appid, string secret)
        {
            var token = Utils.ReadValue(appid, KeyName);
            var exp = string.IsNullOrEmpty(Utils.ReadValue(appid, ExpTimeName)) ? new DateTime(1970, 1, 1) : DateTime.ParseExact(Utils.ReadValue(appid, ExpTimeName), "yyyyMMddHHmmss", null, DateTimeStyles.None);
            if (string.IsNullOrEmpty(token) || DateTime.UtcNow > exp)return GetNewToken(appid, secret);
            return token;
        }
    }
}
