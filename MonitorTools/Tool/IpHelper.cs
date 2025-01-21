using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

namespace MonitorTools.Tool
{
    public static class IpHelper
    {
        private static Dictionary<string, string> connections;

        static IpHelper()
        {
            // 从配置文件加载 IP 和数据库连接映射
            string json = ConfigurationManager.AppSettings["IpData"];
            connections = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        // 获取所有的 IP 和数据库连接映射
        public static Dictionary<string, string> GetConnections()
        {
            return connections;
        }

        // 获取特定 IP 对应的数据库连接字符串
        public static string GetConn(string ip)
        {
            if (connections.ContainsKey(ip))
            {
                return connections[ip];
            }
            return null;
        }
    }
}
