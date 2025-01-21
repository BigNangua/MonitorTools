using MonitorTools.Tool;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace MonitorTools
{
    public partial class MainForm : Form
    {
        // 创建日志和数据库操作的帮助类实例
        private readonly LogHelper logHelper;
        private readonly DatabaseHelper databaseHelper = new DatabaseHelper();

        public MainForm()
        {
            InitializeComponent();
            // 实例化 LogHelper，并传入委托方法
            logHelper = new LogHelper(AppendLogToUI);
        }

        // 用于更新窗体上的 TextBox 控件
        private void AppendLogToUI(string logMessage)
        {
            if (txtLog.InvokeRequired)
            {
                // 如果不是主线程，则通过 Invoke 来确保线程安全
                txtLog.Invoke(new Action<string>(AppendLogToUI), logMessage);
            }
            else
            {
                // 将日志信息追加到 TextBox
                txtLog.AppendText(logMessage + Environment.NewLine);
                txtLog.ScrollToCaret();  // 确保滚动到最新的日志
            }
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            pingTimer.Start(); // 启动定时器
            logHelper.AppendLog("服务已启动，开始每小时 ping 检查...");
        }

        private void pingTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dictionary = IpHelper.GetConnections();

                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    string ipAddress = pair.Key; // IP 地址
                    string connectionString = pair.Value; // 数据库连接字符串
                    Ping ping = new Ping();

                    PingReply reply = ping.Send(ipAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        databaseHelper.UpdateConnectionString(connectionString); // 更新数据库连接字符串
                        bool isQuerySuccessful = databaseHelper.Equals("SELECT 1");
                    }
                    else
                    {
                        logHelper.AppendLog($"{ipAddress} 无法 ping 通。");
                    }
                }
            }
            catch (Exception ex)
            {
                logHelper.AppendLog($"Ping 操作失败: {ex.Message}");
            }
        }
    }
}
