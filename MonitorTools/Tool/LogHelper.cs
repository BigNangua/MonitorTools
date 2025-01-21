using System.IO;
using System;

namespace MonitorTools.Tool
{
    public class LogHelper
    {
        private string logDirectory;
        private Action<string> logToUI; // 用于传递日志信息到窗体的委托

        public LogHelper()
        {
            // 获取当前应用程序的目录
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            logDirectory = Path.Combine(currentDirectory, "logs");

            // 确保日志目录存在，如果不存在就创建
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        // 构造函数，接收委托
        public LogHelper(Action<string> logToUI)
        {
            // 获取当前应用程序的目录
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            logDirectory = Path.Combine(currentDirectory, "logs");

            // 确保日志目录存在，如果不存在就创建
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // 保存传递的委托
            this.logToUI = logToUI;
        }


        // 将日志写入文件并更新 UI
        public void AppendLog(string message)
        {
            // 获取当前日期，创建以日期为名称的日志文件
            string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");

            // 构造日志信息
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            try
            {
                // 以追加的方式写入日志到文件
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

                // 将日志信息传递到窗体的 TextBox
                logToUI?.Invoke(logMessage); // 调用委托更新 UI
            }
            catch (Exception ex)
            {
                // 如果日志写入失败，输出异常信息
                Console.WriteLine($"日志写入失败: {ex.Message}");
            }
        }
    }
}
