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
using System.Text.RegularExpressions;

namespace UsersProject
{   
    public partial class SingUpPage : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=MyComputer\\SQLEXPRESS;Initial Catalog=user_datas;Integrated Security=True");
        bool passwordSecurityCheck(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            // Does the password contain at least one number?
            if (!Regex.IsMatch(password, @"\d"))
            {
                return false;
            }

            //Does the password contain at least one special character?
            if (!Regex.IsMatch(password, @"[!@#$%^&*()]"))
            {
                return false;
            }

            //Does the password contain at least one upper character?
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            return true;
        }
        public SingUpPage()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        static string captcha()
        {
            Random random = new Random();
            string operators = "+-";
            int n1 = random.Next(10, 51);
            int n2 = random.Next(10, 51);
            int RandomOperatorIndexSellect = random.Next(0, 2);
            char op = operators[RandomOperatorIndexSellect];
            string proccess;
            if (op == '+')
                proccess = n1.ToString() + " " + op + " " + n2.ToString() + " = " + (n1 + n2);
            else
            {
                if (n1 > n2)
                    proccess = n1.ToString() + " " + op + " " + n2.ToString() + " = " + (n1 - n2);
                else
                    proccess = n1.ToString() + " " + op + " " + n2.ToString() + " = " + (n2 - n1);
            }
                return proccess;
        }

        static readonly string captchaProccess = captcha();
        static int equalIndex = (byte)captchaProccess.IndexOf('=');
        readonly string captchaL = captchaProccess.Substring(0,equalIndex+1).Trim();
        readonly string captchaR = captchaProccess.Substring(equalIndex + 1).Trim();

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand userDatasCommand = new SqlCommand("SELECT username, email FROM UserDB", conn);
            SqlDataReader userDatasReader = userDatasCommand.ExecuteReader();

            bool usernameExists = false;
            bool emailExists = false;

            while (userDatasReader.Read())
            {
                if (userDatasReader["username"].ToString() == usernameBox.Text)
                {
                    usernameExists = true;
                }

                if (userDatasReader["email"].ToString() == emailBox.Text)
                {
                    emailExists = true;
                }

                if (usernameExists && emailExists)
                {
                    break;
                }
            }

            if (usernameBox.Text == "" || emailBox.Text == "" || passwordBox.Text == "" || secondPasswordBox.Text == "" || captchaBox.Text == "")
            {
                MessageBox.Show("Please fill in all text boxes.");
            }
            else if (usernameExists)
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
            }
            else if (emailExists)
            {
                MessageBox.Show("Email address already exists. Please use a different email address.");
            }
            else if (!passwordSecurityCheck(passwordBox.Text))
            {
                MessageBox.Show("The password must be at least 8 characters; It must contain 1 number, 1 special character and 1 uppercase letter.");
            }
            else if(passwordBox.Text != secondPasswordBox.Text)
            {
                MessageBox.Show("Passwords Are Not The Same!");
            }
            else if (captchaBox.Text != captchaR.ToString()) {
                MessageBox.Show("captcha is not correct!");
            }
            else
            {
                //Everything is okey.
                userDatasReader.Close();
                SqlCommand userRegisterCommand = new SqlCommand("INSERT INTO UserDB (userName, password, email, isItActive) VALUES (@p1, @p2, @p3, @p4)", conn);
                userRegisterCommand.Parameters.AddWithValue("@p1", usernameBox.Text);
                userRegisterCommand.Parameters.AddWithValue("@p2", passwordBox.Text);
                userRegisterCommand.Parameters.AddWithValue("@p3", emailBox.Text);
                userRegisterCommand.Parameters.AddWithValue("@p4", 1);
                userRegisterCommand.ExecuteNonQuery();
                MessageBox.Show("Registration Successful");
                this.Close();
            }
            conn.Close();

        }

        private void SingUpPage_Load(object sender, EventArgs e)
        {
            captchaLabel.Text = captchaL;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
