using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;

namespace To_do_List_Application
{
    public partial class NewTaskForm : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect("localhost", "ToDoListDB");
        SqlDataReader dr;

        MainForm mainForm = new MainForm();

        public NewTaskForm(MainForm mainForm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.GetConnection().ConnectionString);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                SaveTask();
            }
        }

        private void SaveTask()
        {
            if (CheckFields())
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("INSERT INTO Tasks (TaskName, DueDate, TaskDescription, IsCompleted) VALUES (@TaskName, @DueDate, @TaskDescription, @IsCompleted)", cn);
                cm.Parameters.AddWithValue("@TaskName", taskNameTextBox.Text);
                cm.Parameters.AddWithValue("@DueDate", dueDateTextBox.Text);
                cm.Parameters.AddWithValue("@TaskDescription", descriptionTextBox.Text);
                cm.Parameters.AddWithValue("@IsCompleted", 0);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Task data has been successufully added or updated");
                this.Dispose();
                mainForm.LoadTasks();
            }
        }

        public bool CheckFields()
        {
            if (string.IsNullOrWhiteSpace(taskNameTextBox.Text))
            {
                MessageBox.Show("Please enter a task name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!DateTime.TryParse(dueDateTextBox.Text, out DateTime dueDate))
            {
                MessageBox.Show("Please enter a valid due date in the format yyyy-MM-dd.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dueDate < DateTime.Today)
            {
                MessageBox.Show("Please select a valid due date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description for the task.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("UPDATE Tasks SET TaskName = @TaskName, TaskDescription = @TaskDescription, DueDate = @DueDate WHERE TaskId = @TaskId", cn);
                cm.Parameters.AddWithValue("@TaskId", lblTaskId.Text);
                cm.Parameters.AddWithValue("@TaskName", taskNameTextBox.Text);
                cm.Parameters.AddWithValue("@TaskDescription", descriptionTextBox.Text);
                cm.Parameters.AddWithValue("@DueDate", dueDateTextBox.Text);

                cn.Open();
                cm.ExecuteNonQuery();
                cn.Close();
                mainForm.LoadTasks();
                this.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewTaskForm_Load(object sender, EventArgs e)
        {

        }
    }
}
