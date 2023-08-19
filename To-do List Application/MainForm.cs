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

namespace To_do_List_Application
{
    public partial class MainForm : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect("localhost", "ToDoListDB");
        SqlDataReader dr;

        public MainForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.GetConnection().ConnectionString);
            LoadTasks();
        }

        public void LoadTasks()
        {
            int rowNumber = 0;
            dgvTaskList.Rows.Clear();
            cm = new SqlCommand("SELECT TaskId, TaskName, DueDate, TaskDescription FROM Tasks WHERE isCompleted = 0", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                rowNumber++;
                int taskId = (int)dr["TaskId"];
                string taskName = dr["TaskName"].ToString();
                DateTime dueDate = (DateTime)dr["DueDate"];
                string taskDescription = dr["TaskDescription"].ToString();
                dgvTaskList.Rows.Add(rowNumber, taskId, taskName, dueDate, taskDescription);
            }

            dr.Close();
            cn.Close();
        }

        private void newTaskButton_Click(object sender, EventArgs e)
        {
            NewTaskForm newTaskForm = new NewTaskForm(this);
            newTaskForm.FormClosed += NewTaskForm_FormClosed;
            newTaskForm.ShowDialog();
        }

        private void NewTaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadTasks();
        }

        private void dgvTaskList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvTaskList.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                NewTaskForm newTaskForm = new NewTaskForm(this);
                newTaskForm.lblTaskId.Text = dgvTaskList.Rows[e.RowIndex].Cells[1].Value.ToString();
                newTaskForm.taskNameTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[2].Value.ToString();
                newTaskForm.dueDateTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[3].Value.ToString();
                newTaskForm.descriptionTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[4].Value.ToString();

                newTaskForm.saveButton.Enabled = false;
                newTaskForm.UpdateButton.Enabled = true;
                newTaskForm.ShowDialog();
                LoadTasks();
            }
            else if (colName == "Details")
            {
                DetailsForm detailsForm = new DetailsForm(this);
                detailsForm.lblTaskId.Text = dgvTaskList.Rows[e.RowIndex].Cells[1].Value.ToString();
                detailsForm.taskNameTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[2].Value.ToString();
                detailsForm.dueDateTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[3].Value.ToString();
                detailsForm.descriptionTextBox.Text = dgvTaskList.Rows[e.RowIndex].Cells[4].Value.ToString();

                detailsForm.taskNameTextBox.ReadOnly = true;
                detailsForm.dueDateTextBox.ReadOnly = true;
                detailsForm.descriptionTextBox.ReadOnly = true;

                detailsForm.ShowDialog();
            }
            else if (colName == "Completed")
            {
                if (MessageBox.Show("Are you sure you want to mark this task as completed?", "Complete Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int taskId = Convert.ToInt32(dgvTaskList.Rows[e.RowIndex].Cells[1].Value);
                    string taskName = dgvTaskList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    DateTime dueDate = Convert.ToDateTime(dgvTaskList.Rows[e.RowIndex].Cells[3].Value);
                    string taskDescription = dgvTaskList.Rows[e.RowIndex].Cells[4].Value.ToString();

                    cn.Open();

                    cm = new SqlCommand("UPDATE Tasks SET IsCompleted = 1 WHERE TaskId = @TaskId", cn);
                    cm.Parameters.AddWithValue("@TaskId", taskId);
                    cm.ExecuteNonQuery();

                    cm = new SqlCommand("INSERT INTO CompletedTasks (TaskId, TaskName, TaskDescription, DueDate, IsCompleted) VALUES (@TaskId, @TaskName, @TaskDescription, @DueDate, @IsCompleted)", cn);
                    cm.Parameters.AddWithValue("@TaskId", taskId);
                    cm.Parameters.AddWithValue("@TaskName", taskName);
                    cm.Parameters.AddWithValue("@TaskDescription", taskDescription);
                    cm.Parameters.AddWithValue("@DueDate", dueDate);
                    cm.Parameters.AddWithValue("@IsCompleted", true);
                    cm.ExecuteNonQuery();

                    cn.Close();

                    MessageBox.Show("Task has been successfully marked as completed.");

                    LoadTasks();
                }
            }
        }

        private void completedTaskButton_Click(object sender, EventArgs e)
        {
            CompletedTasksForm completedTasksForm = new CompletedTasksForm();
            completedTasksForm.ShowDialog();

        }
    }
}
