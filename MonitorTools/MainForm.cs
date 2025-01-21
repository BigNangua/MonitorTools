using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;
using MonitorTools.Tool;

namespace MonitorTools
{
    public partial class MainForm : Form
    {
        // 创建日志和数据库操作的帮助类实例
        private readonly LogHelper logHelper;

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
            if (this.btnStartService.Text == "启动服务")
            {
                this.btnStartService.Text = "停止服务";
                pingTimer.Start(); // 启动定时器
                logHelper.AppendLog("服务已启动，开始ping 检查...");
            }
            else
            {
                this.btnStartService.Text = "启动服务";
                pingTimer.Stop(); // 停止定时器
            }
        }

        /// <summary>
        /// 定时器 Tick 事件，用于执行 telnet 数据库端口操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pingTimer_Tick(object sender, EventArgs e)
        {
            this.pingTimer.Stop(); // 停止定时器
            EmailSender email = new EmailSender();
            try
            {
                Dictionary<string, string> dictionary = IpHelper.GetConnections();
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    // IP 地址
                    string ipAddress = pair.Key;
                    bool isOk = IsPortOpen(ipAddress, 1433);
                    if (!isOk)
                    {
                        email.SendEmail("网络故障通知", $"IP 地址 {ipAddress} 不可达");
                        logHelper.AppendLog($"IP 地址 {ipAddress} 不可达");
                    }
                }
            }
            catch (Exception ex)
            {
                email.SendEmail("网络故障通知", $"Ping 操作异常: {ex.Message}");
                logHelper.AppendLog($"Ping 操作失败: {ex.Message}");
            }
            finally
            {
                this.pingTimer.Start(); // 重新启动定时器
            }
        }

        /// <summary>
        /// 检查指定 IP 地址和端口是否开放
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool IsPortOpen(string ip, int port)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(ip, port);
                    return true;
                }
            }
            catch (SocketException)
            {
                return false;
            }
        }

    }
}

