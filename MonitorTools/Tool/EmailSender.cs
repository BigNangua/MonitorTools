using System.Net.Mail;
using System.Net;
using System;

namespace MonitorTools.Tool
{
    public class EmailSender
    {
        // SMTP 服务器相关信息
        private string smtpServer; // SMTP 服务器地址
        private int smtpPort; // SMTP 端口号
        private string smtpUser; // SMTP 用户名（通常是发件人邮箱地址）
        private string smtpPassword; // SMTP 密码（通常是发件人邮箱的密码或应用专用密码）
        private string fromEmail; // 发件人邮箱地址
        private string toEmail; // 收件人邮箱地址

        // 构造函数，初始化邮件发送相关配置
        public EmailSender(string smtpServer, int smtpPort, string smtpUser, string smtpPassword, string fromEmail, string toEmail)
        {
            // 初始化邮件服务器相关信息
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUser = smtpUser;
            this.smtpPassword = smtpPassword;
            this.fromEmail = fromEmail;
            this.toEmail = toEmail;
        }

        // 发送邮件的方法
        // subject：邮件主题，body：邮件正文
        public bool SendEmail(string subject, string body)
        {
            try
            {
                // 创建 SmtpClient 对象，用于与邮件服务器进行连接
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    // 设置 SMTP 客户端的认证信息（用户名和密码）
                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);

                    // 启用 SSL 加密（如果需要）
                    smtpClient.EnableSsl = true;

                    // 创建邮件消息
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail), // 设置发件人邮箱地址
                        Subject = subject, // 设置邮件主题
                        Body = body, // 设置邮件正文内容
                        IsBodyHtml = false // 设置邮件正文为纯文本格式，如果需要发送 HTML 邮件，可以将其设置为 true
                    };

                    // 添加收件人邮箱地址
                    mailMessage.To.Add(toEmail);

                    // 发送邮件
                    smtpClient.Send(mailMessage);
                }

                // 如果没有异常发生，则表示邮件发送成功
                return true;
            }
            catch (Exception ex)
            {
                // 捕获并打印发送邮件过程中发生的任何异常
                Console.WriteLine($"邮件发送失败: {ex.Message}");
                return false; // 如果发生错误，返回 false
            }
        }
    }
}
