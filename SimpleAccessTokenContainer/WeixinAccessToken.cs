using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleAccessTokenContainer
{
    public static class WeixinAccessToken
    {
        private const string KeyName = "WeixinAccessToken";
        private const string ExpTimeName = "WeixinAccessTokenExpTime";

        public static string GetNewToken(string appid, string secret, string grant_type = "client_credential")
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type={grant_type}&appid={appid}&secret={secret}";
            var ret = Utils.GetString(url);
            if (ret.Contains("errcode"))throw new Exception("error return code");
            var regx = new Regex(@"^\{""access_token"":""(.*?)"",""expires_in"":(\d+)\}$");
            var mt = regx.Match(ret);
            if (!mt.Success) throw new InvalidOperationException("wrong return");
            Utils.WriteValue(appid, KeyName, mt.Groups[1].Value);
            //超时提前个2分钟。以免出现问题
            Utils.WriteValue(appid, ExpTimeName, DateTime.UtcNow.AddSeconds(int.Parse(mt.Groups[2].Value) - 120).ToString("yyyyMMddHHmmss"));
            return mt.Groups[1].Value;
        }

        public static string GetToken(string appid, string secret)
        {
            //首先拿到缓存下来的Token
            var token = Utils.ReadValue(appid, KeyName);
            //如果缓存里的超时为空，就强制超时
            var exp = string.IsNullOrEmpty(Utils.ReadValue(appid, ExpTimeName)) ? new DateTime(1970, 1, 1) : DateTime.ParseExact(Utils.ReadValue(appid, ExpTimeName), "yyyyMMddHHmmss", null, DateTimeStyles.None);
            //如果缓存里没有token或者已经超时，就重新获取一个Token，否则返回缓存Token
            if (string.IsNullOrEmpty(token) || DateTime.UtcNow > exp)return GetNewToken(appid, secret);
            return token;
        }
    }
}
