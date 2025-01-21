using System.Data.SqlClient;
using System;

namespace MonitorTools.Tool
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            this.connectionString = "";
        }

        // 构造函数：从配置文件或其他来源获取数据库连接字符串
        public DatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // 执行简单的数据库查询，返回查询结果
        public bool ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();  // 打开数据库连接
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteScalar(); // 执行查询
                }

                return true;  // 查询成功
            }
            catch (Exception ex)
            {
                // 发生异常时记录日志
                LogHelper logHelper = new LogHelper();
                logHelper.AppendLog($"数据库查询失败: {ex.Message}");
                return false;  // 查询失败
            }
        }

        // 更新连接字符串，支持切换数据库
        public void UpdateConnectionString(string newConnectionString)
        {
            this.connectionString = newConnectionString;
            LogHelper logHelper = new LogHelper();
            logHelper.AppendLog("数据库连接字符串已更新");
        }
    }
}
