using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleAccessTokenContainer;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入你的微信appid然后回车：");
            var appid = Console.ReadLine();
            Console.WriteLine("输入你的微信appsecret然后回车：");
            var appsecret = Console.ReadLine();
            Console.WriteLine($"您目前的微信AccessToken为：{WeixinAccessToken.GetToken(appid, appsecret)}");
            Console.WriteLine($"您目前的微信JSSDKTicket为：{WeixinJSSDKTicket.GetToken(appid, appsecret)}");
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
