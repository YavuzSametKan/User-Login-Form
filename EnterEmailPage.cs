using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UsersProject
{
    public partial class EnterEmailPage : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=MyComputer\\SQLEXPRESS;Initial Catalog=user_datas;Integrated Security=True");
        public EnterEmailPage()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        string RandomCodeGenerator(){
            Random random = new Random();
            return random.Next(100000, 999999 + 1).ToString();
        }

        int defaultMethod(int x = 1, int c = 15)
        {
            return x + c;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValidEmail(emailInput.Text))
            {
                conn.Open();
               
                SqlCommand userEmailDatasCommand = new SqlCommand("SELECT email,id from UserDB where isItActive = 1",conn);
                SqlDataReader userEmailDatasReader = userEmailDatasCommand.ExecuteReader();
                bool IsItEmailSuccess = false;
                while (userEmailDatasReader.Read())
                {
                    if(emailInput.Text == userEmailDatasReader["email"].ToString())
                    {
                        IsItEmailSuccess = true;
                        break;
                    }
                }
                if (IsItEmailSuccess)
                {
                    short checkedUserId = Convert.ToInt16(userEmailDatasReader["id"].ToString());
                    userEmailDatasReader.Close();
                    string randomCode = RandomCodeGenerator();
                    SqlCommand codeGenerateCommand = new SqlCommand("UPDATE UserDB SET code = @p1 WHERE id = @p2", conn);
                    codeGenerateCommand.Parameters.AddWithValue("@p1", randomCode);
                    codeGenerateCommand.Parameters.AddWithValue("@p2", checkedUserId.ToString());
                    codeGenerateCommand.ExecuteNonQuery();
                    try
                    {
                        string toAddress = emailInput.Text; // Alıcı e-posta adresi
                        string subject = "Your Password Reset Code"; // E-posta konusu
                        string body = "Reset Code = " + randomCode; // E-posta içeriği

                        // Gmail SMTP sunucu ve kimlik bilgileri
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Host = "smtp.gmail.com";
                        smtpClient.Port = 587; // Gmail SMTP sunucu bağlantı noktası
                        smtpClient.Credentials = new NetworkCredential("codesenderr@gmail.com", "vbxw oqmv uroz fegx");
                        smtpClient.EnableSsl = true; // SSL kullan

                        // E-posta gönderenin adresi
                        MailAddress fromAddress = new MailAddress("codesenderr@gmail.com", "Yavuz Samet Kan");

                        // E-posta oluşturma
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = fromAddress;
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.To.Add(toAddress);

                        // E-postayı gönder
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Email sending error: " + ex.Message);
                    }
                    this.Close();
                    IForgotPassword IFPPage = new IForgotPassword(checkedUserId);
                    IFPPage.Show();
                }
                else
                {
                    MessageBox.Show(emailInput.Text + " is not an email registered in the system!");
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("This is not an email: " + emailInput.Text);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void EnterEmailPage_Load(object sender, EventArgs e)
        {

        }
    }
}
