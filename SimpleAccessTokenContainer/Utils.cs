using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccessTokenContainer
{
    public static class Utils
    {
        private const string IniPath = "Z:\\SimpleAccessTokenContainer.ini";//为了方便我把ini放进了内存盘，内存盘有iis的写权限

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, IniPath);
        }

        public static string ReadValue(string section, string key)
        {
            var temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, null, temp, 255, IniPath);
            return temp.ToString();
        }

        public static Task<string> GetStringAsync(string uri, Encoding encoding = null)
        {
            var wc = new WebClient { Encoding = encoding ?? Encoding.UTF8 };
            return wc.DownloadStringTaskAsync(uri);
        }

        public static string GetString(string uri, Encoding encoding = null)
        {
            var wc = new WebClient { Encoding = encoding ?? Encoding.UTF8 };
            return wc.DownloadString(uri);
        }

    }
}
