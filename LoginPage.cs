using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UsersProject
{
    public partial class LoginForm : Form
    {
        
        SqlConnection conn = new SqlConnection("Data Source=MyComputer\\SQLEXPRESS;Initial Catalog=user_datas;Integrated Security=True");
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            passwordInput.UseSystemPasswordChar = true;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if(!(userNameInput.Text == "" || passwordInput.Text == ""))
            {
                conn.Open();
                SqlCommand userLoginDatasCommand = new SqlCommand("SELECT userName,email,password FROM UserDB where isItActive = 1", conn);
                SqlDataReader userLoginDataReader = userLoginDatasCommand.ExecuteReader(); // used to run the select query
                bool IsItLoginSuccess = false;
                while (userLoginDataReader.Read())
                {
                    if ((userNameInput.Text == userLoginDataReader["userName"].ToString() || userNameInput.Text == userLoginDataReader["email"].ToString()) && passwordInput.Text == userLoginDataReader["password"].ToString())
                    {
                        IsItLoginSuccess = true;
                        break;
                    }
                }

                if (IsItLoginSuccess)
                {
                    MessageBox.Show("Login Successful: " + userLoginDataReader["userName"].ToString());
                }
                else
                {
                    MessageBox.Show("User name/email or password is incorrect.");
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("UserName/email or password input cannot be empty!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EnterEmailPage EEPage = new EnterEmailPage();
            EEPage.Show();
        }

        private void closeAppButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void passwordVisiblityButton_Click(object sender, EventArgs e)
        {
            if (passwordVisiblityButton.Text == "show")
            {
                passwordVisiblityButton.Text = "hide";
                passwordInput.UseSystemPasswordChar = false;
            }
            else
            {
                passwordVisiblityButton.Text = "show";
                passwordInput.UseSystemPasswordChar = true;
            }
        }

        private void userNameInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SingUpPage SUPage = new SingUpPage();
            SUPage.Show();
        }
    }
}
