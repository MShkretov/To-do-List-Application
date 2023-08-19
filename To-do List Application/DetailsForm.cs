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
    public partial class DetailsForm : Form
    {


        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect("localhost", "ToDoListDB");
        SqlDataReader dr;

        MainForm mainForm = new MainForm();


        public DetailsForm(MainForm mainForm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.GetConnection().ConnectionString);
        }
      
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
