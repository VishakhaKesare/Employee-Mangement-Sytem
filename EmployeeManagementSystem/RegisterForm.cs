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


namespace EmployeeManagementSystem
{
    public partial class RegisterForm : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=LAPTOP-3VBIOI70\MSSQLSERVER03;Initial Catalog=employee;Integrated Security=True;TrustServerCertificate=True");
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signup_loginBtn_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void signup_showPassword_CheckedChanged(object sender, EventArgs e)
        {
            signup_password.PasswordChar = signup_showPassword.Checked ? '\0' : '*';
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            if (signup_username.Text == "" || signup_password.Text == "")
            {
                MessageBox.Show("PLEASE FILL ALL BLANK FIELDS",
                                "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (connect.State != ConnectionState.Open)
            {
                connect.Open();
            }

            // TO CHECK IF THE USER IS EXISTING ALREADY
            string selectUsername = "SELECT COUNT(id) FROM users WHERE username = @user";
            SqlCommand checkUser = new SqlCommand(selectUsername, connect);
            checkUser.Parameters.AddWithValue("@user", signup_username.Text.Trim());
            int count = (int)checkUser.ExecuteScalar();

            if (count >= 1)
            {
                MessageBox.Show(signup_username.Text.Trim() + " is already taken",
                                "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
                return;
            }

            DateTime today = DateTime.Today;

            string insertData = "INSERT INTO users " +
                                "(username, password, date_register) " +
                                "VALUES(@username, @password, @dateReg)";

            SqlCommand cmd = new SqlCommand(insertData, connect);
            cmd.Parameters.AddWithValue("@username", signup_username.Text.Trim());
            cmd.Parameters.AddWithValue("@password", signup_password.Text.Trim());
            cmd.Parameters.AddWithValue("@dateReg", today);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registered successfully!",
                                "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
    }
}