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
    public partial class IForgotPassword : Form
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

        private short selectedUserID;
        public IForgotPassword(short id)
        {
            InitializeComponent();
            selectedUserID = id;
        }

        private void IForgotPassword_Load(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand userDataCommand = new SqlCommand("SELECT email FROM UserDB WHERE id = @p1" , conn);
            userDataCommand.Parameters.AddWithValue("@p1", selectedUserID);
            SqlDataReader userDataReader = userDataCommand.ExecuteReader();
            if (userDataReader.Read())
                emailLabel.Text = "A password renewal code has been sent to \"" + userDataReader["email"].ToString() + "\" email address.";
            else
                MessageBox.Show("User not found.");
            conn.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void emailLabel_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            if (codeInput.Text == "")
            {
                MessageBox.Show("Please Enter A Code In The Code Entry.");
            }
            else if (passwordInput.Text == "" || repeatPasswordInput.Text == "")
            {
                MessageBox.Show("Please Enter A Password In The Password Entries.");
            }
            else
            {
                // Everything is okey.
                bool hasPasswordChanged = false;
                using (SqlCommand userDataCommand = new SqlCommand("SELECT code,email FROM UserDB WHERE id = @p1", conn))
                {
                    userDataCommand.Parameters.AddWithValue("@p1", selectedUserID);
                    using (SqlDataReader userDataReader = userDataCommand.ExecuteReader())
                    {
                        if (userDataReader.Read())
                        {
                            if (codeInput.Text == userDataReader["code"].ToString())
                            {
                                if (passwordSecurityCheck(passwordInput.Text))
                                {
                                    if (passwordInput.Text == repeatPasswordInput.Text)
                                    {
                                        // Everything is okey
                                        userDataReader.Close();
                                        using (SqlCommand passwordChangeCommand = new SqlCommand("UPDATE UserDB SET password = @p1, code = @p2 WHERE id = @p3", conn))
                                        {
                                            passwordChangeCommand.Parameters.AddWithValue("@p1", passwordInput.Text);
                                            passwordChangeCommand.Parameters.AddWithValue("@p2", "");
                                            passwordChangeCommand.Parameters.AddWithValue("@p3", selectedUserID);
                                            passwordChangeCommand.ExecuteNonQuery();
                                            hasPasswordChanged = true;
                                        }
                                    }
                                    else
                                        MessageBox.Show("Passwords Are Not The Same!");
                                }
                                else
                                    MessageBox.Show("The password must be at least 8 characters; It must contain 1 number, 1 special character and 1 uppercase letter.");
                            }
                            else
                                MessageBox.Show("Code Is Not Correct!");
                        }
                        else
                            MessageBox.Show("User not found!");
                    }
                }
                if (hasPasswordChanged)
                {
                    MessageBox.Show("Your password has been successfully reset!");
                    this.Close();
                }
                conn.Close();
            }
        }


    }
}
