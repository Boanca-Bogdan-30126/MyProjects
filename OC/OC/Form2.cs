using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3ll
{
    public partial class Form2 : Form
    {
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-GGH7PSJ\\SQLEXPRESS;Initial Catalog=incercam;Integrated Security=True");


        private string nume;
        private string prenume;
        private string cod;
        public Form2(string nume,string prenume,string cod)
        {
            InitializeComponent();
            this.nume = nume;
            this.prenume = prenume;
            this.cod = cod;
            label1.Text = cod;
            
        }
        
        

        public void SaveDataToDatabase()
        {

            con.Open();


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (!row.IsNewRow && row.Cells[0].Value != null && !row.Cells[0].Value.Equals(this.cod))
                {

                    
                    string coloana0 = row.Cells[0].Value.ToString();
                    string coloana1 = row.Cells[1].Value.ToString(); 
                    string coloana2 = row.Cells[2].Value.ToString();
                    string coloana3 = row.Cells[3].Value.ToString();
                    string coloana4 = row.Cells[4].Value.ToString();
                    string coloana5 = row.Cells[5].Value.ToString();

   
                    string query = "INSERT INTO DatePersoane (Cod,Universitate,An,Specializare,Grupa,Media) VALUES (@Cod,@Valoare1, @Valoare2,@Valoare3,@Valoare4,@Valoare5)"; 

  
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@Valoare1", coloana1);
                    command.Parameters.AddWithValue("@Valoare2", coloana2);
                    command.Parameters.AddWithValue("@Valoare3", coloana3);
                    command.Parameters.AddWithValue("@Valoare4", coloana4);
                    command.Parameters.AddWithValue("@Valoare5", coloana5);
                    command.Parameters.AddWithValue("@Cod", coloana0);

                    command.ExecuteNonQuery();
                }
            }

            con.Close();

            MessageBox.Show("Datele au fost salvate cu succes în baza de date.");
        }




        private void AfiseazaToateDateleLegateDupaCod(string cod)
        {
            // Aplică filtrul doar dacă este specificat un cod valid
            string query = "SELECT * FROM DatePersoane";
            if (!string.IsNullOrEmpty(cod))
            {
                query += " WHERE Cod = @Cod";
            }

            using (con)
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    if (!string.IsNullOrEmpty(cod))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@Cod", cod);
                    }
                    DataSet ds = new DataSet();
                    da.Fill(ds, "DatePersoane");
                    dataGridView1.DataSource = ds.Tables["DatePersoane"].DefaultView;
                }
            }
        }





        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDataToDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AfiseazaToateDateleLegateDupaCod(cod);


        }

    }

}
