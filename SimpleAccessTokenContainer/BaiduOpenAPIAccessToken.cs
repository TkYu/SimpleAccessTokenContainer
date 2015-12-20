using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleAccessTokenContainer
{
    public static class BaiduOpenAPIAccessToken
    {
        private const string KeyName = "BaiduOpenAPIAccessToken";
        private const string SessKeyName = "BaiduOpenAPISessKey";
        private const string SessSecretName = "BaiduOpenAPISessSecret";
        private const string RefreshTokenName = "BaiduOpenAPIRefreshToken";
        private const string ExpTimeName = "BaiduOpenAPIAccessTokenExpTime";

        public static string GetNewToken(string client_id, string client_secret, string grant_type = "client_credentials")
        {
            var url = $"https://openapi.baidu.com/oauth/2.0/token?grant_type={grant_type}&client_id={client_id}&client_secret={client_secret}";
            var ret = Utils.GetString(url);
            if (ret.Contains("errcode"))throw new Exception("error return code");
            var regx = new Regex(@"""access_token"":""(.*?)"",""session_key"":""(.*?)"".*refresh_token"":""(.*?)"",""session_secret"":""(.*?)"",""expires_in"":(\d+)");
            var mt = regx.Match(ret);
            if (!mt.Success) throw new InvalidOperationException("wrong return");
            Utils.WriteValue(client_id, KeyName, mt.Groups[1].Value);
            Utils.WriteValue(client_id, SessKeyName, mt.Groups[2].Value);
            Utils.WriteValue(client_id, RefreshTokenName, mt.Groups[3].Value);
            Utils.WriteValue(client_id, SessSecretName, mt.Groups[4].Value);
            Utils.WriteValue(client_id, ExpTimeName, DateTime.UtcNow.AddSeconds(int.Parse(mt.Groups[5].Value) - 120).ToString("yyyyMMddHHmmss"));
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
