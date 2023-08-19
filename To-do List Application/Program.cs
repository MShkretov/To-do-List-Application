using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace To_do_List_Application
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            dbConnect dbConnect = new dbConnect("localhost", "ToDoListDB");

            try
            {
                using (SqlConnection connection = dbConnect.GetConnection())
                {
                    connection.Open();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            Application.Run(new MainForm());
        }
    }
}
