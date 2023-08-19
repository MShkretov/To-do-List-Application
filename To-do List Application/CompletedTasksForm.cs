using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace To_do_List_Application
{
    public partial class CompletedTasksForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect("localhost", "ToDoListDB");
        SqlDataReader dr;

        public CompletedTasksForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.GetConnection().ConnectionString);
            DisplayCompletedTasks();
        }

        private void dgvTaskList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void DisplayCompletedTasks()
        {
            dgvCompletedTasks.Rows.Clear();

            using (SqlConnection cn = new SqlConnection(dbcon.GetConnection().ConnectionString))
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("SELECT TaskName, TaskDescription, DueDate FROM CompletedTasks WHERE IsCompleted = 1", cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    string taskName = dr["TaskName"].ToString();
                    string taskDescription = dr["TaskDescription"].ToString();
                    DateTime dueDate = Convert.ToDateTime(dr["DueDate"]);

                    dgvCompletedTasks.Rows.Add(taskName, taskDescription, dueDate);
                }
                dr.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            MainForm mainForm = new MainForm();

            mainForm.ShowDialog();
        }
    }
}