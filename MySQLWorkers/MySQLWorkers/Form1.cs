using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQLWorkers
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=localhost;Database=employeedb;Uid=root;Pwd=root;";
        int workersID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                // используем хранимую процедуру WorkersAddOrEdit
                MySqlCommand mySqlCmd = new MySqlCommand("WorkersAddOrEdit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_WorkersID", workersID);
                mySqlCmd.Parameters.AddWithValue("_WorkersName", txtName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersSurname", txtSurname.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersPatronymic", txtPatronymic.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersBirthday", txtBirthday.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersAddress", txtAddress.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersDepartment", txtDepartment.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_WorkersAbout", txtAbout.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Данные успешно добавлены!");
                Clear();
                GridFill();
            }
        }

        void GridFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                // используем хранимую процедуру WorkersViewAll
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("WorkersViewAll", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblWorkers = new DataTable();
                sqlDa.Fill(dtblWorkers);
                dgvWorkers.DataSource = dtblWorkers;
                dgvWorkers.Columns[0].Visible = false; // скрываем столбец ID
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            GridFill();
        }

        void Clear()
        {
            txtName.Text = txtSurname.Text = txtPatronymic.Text = txtBirthday.Text = txtAddress.Text = txtDepartment.Text = txtAbout.Text = txtSearch.Text = "";
            workersID = 0;
            btnSave.Text = "Сохранить"; // кнопка  будет изменятся на "Обновить"
            btnDelete.Enabled = false;
        }

        private void dgvWorkers_DoubleClick(object sender, EventArgs e)
        {
            if(dgvWorkers.CurrentRow.Index != -1)
            {
                txtName.Text = dgvWorkers.CurrentRow.Cells[1].Value.ToString();
                txtSurname.Text = dgvWorkers.CurrentRow.Cells[2].Value.ToString();
                txtPatronymic.Text = dgvWorkers.CurrentRow.Cells[3].Value.ToString();
                txtBirthday.Text = dgvWorkers.CurrentRow.Cells[4].Value.ToString();
                txtAddress.Text = dgvWorkers.CurrentRow.Cells[5].Value.ToString();
                txtDepartment.Text = dgvWorkers.CurrentRow.Cells[6].Value.ToString();
                txtAbout.Text = dgvWorkers.CurrentRow.Cells[7].Value.ToString();
                workersID = Convert.ToInt32(dgvWorkers.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Обновить"; // кнопка  будет изменятся на "Обновить"
                btnDelete.Enabled = Enabled;

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                // используем хранимую процедуру WorkersSearchByValue
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("WorkersSearchByValue", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                DataTable dtblWorkers = new DataTable();
                sqlDa.Fill(dtblWorkers);
                dgvWorkers.DataSource = dtblWorkers;
                dgvWorkers.Columns[0].Visible = false; // скрываем столбец ID
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                // используем хранимую процедуру WorkersDeleteByID
                MySqlCommand mySqlCmd = new MySqlCommand("WorkersDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_WorkersID", workersID);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Данные успешно удалены!");
                Clear();
                GridFill();
            }
        }
    }
}

