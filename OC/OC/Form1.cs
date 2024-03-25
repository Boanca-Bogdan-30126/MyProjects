using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3ll
{

    public partial class Form1 : Form
    {
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-GGH7PSJ\\SQLEXPRESS;Initial Catalog=incercam;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void StergePersoana(string nume, string prenume,string cod)
        {
            try
            {
                con.Open();
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM Persoane WHERE Nume = @Nume AND Prenume = @Prenume AND @Cod = Cod", con);
                deleteCmd.Parameters.AddWithValue("@Nume", nume);
                deleteCmd.Parameters.AddWithValue("@Prenume", prenume);
                deleteCmd.Parameters.AddWithValue("@Cod", cod);
                deleteCmd.ExecuteNonQuery();
                MessageBox.Show("Persoana a fost ștearsă din baza de date.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("A apărut o eroare la ștergerea din baza de date: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }



        private void button1_Click(object sender, EventArgs e)
{
    string TextToSave1 = textBox1.Text;
    string TextToSave2 = textBox2.Text;
    string TextToSave3 = textBox3.Text;

    // Verificăm dacă sunt introduse date în textBox1, textBox2 și textBox3
    if (string.IsNullOrWhiteSpace(TextToSave1) || string.IsNullOrWhiteSpace(TextToSave2) || string.IsNullOrWhiteSpace(TextToSave3))
    {
        MessageBox.Show("Vă rugăm să completați toate câmpurile!");
        return;
    }

    // Deschidem conexiunea pentru a verifica dacă numele și prenumele se află în baza de date
    con.Open();
    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Persoane WHERE Nume = @Nume AND Prenume = @Prenume", con);
    checkCmd.Parameters.AddWithValue("@Nume", TextToSave1);
    checkCmd.Parameters.AddWithValue("@Prenume", TextToSave2);
    int numePrenumeCount = (int)checkCmd.ExecuteScalar();
    con.Close();

    // Verificăm dacă numele și prenumele există în baza de date
    if (numePrenumeCount > 0)
    {
        // Deschidem direct Form2 dacă numele și prenumele există în baza de date
        Form2 existingForm = new Form2(TextToSave1,TextToSave2,TextToSave3);
        existingForm.Show();
        return;
    }

    // Verificăm dacă codul este unic
    bool codUnic = VerificareCodUnic(TextToSave3);

    // Dacă codul nu este unic, nu continuăm cu inserarea datelor și deschiderea Form2
    if (!codUnic)
    {
        return;
    }

    // Deschidem conexiunea pentru a insera datele în baza de date
    con.Open();
    SqlCommand cmd = new SqlCommand("INSERT INTO Persoane (Nume, Prenume, Cod) VALUES (@Text1, @Text2, @Text3)", con);
    cmd.Parameters.AddWithValue("@Text1", TextToSave1);
    cmd.Parameters.AddWithValue("@Text2", TextToSave2);
    cmd.Parameters.AddWithValue("@Text3", TextToSave3);
    cmd.ExecuteNonQuery();
    con.Close();
    // Deschidem Form2 doar dacă codul este unic și datele au fost inserate cu succes în baza de date\
    Form2 newForm = new Form2(TextToSave1,TextToSave2,TextToSave3);
    
    newForm.Show();
}



        private bool VerificareCodUnic(string Cod)
        {
            string codNou = Cod;

            con.Open();
            SqlCommand checkCmd = new SqlCommand("Select Count(*) FROM Persoane WHERE Cod = @Cod", con);
            checkCmd.Parameters.AddWithValue("@Cod", codNou);
            int codCount = (int)checkCmd.ExecuteScalar();
            con.Close();

            if (codCount > 0)
            {
                MessageBox.Show("Codul introdus apartine unui alt utilizator. Introduceți altul.");
                return false; // Returnează false dacă codul nu este unic
            }
            else
            {
                return true; // Returnează true dacă codul este unic
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string numePersoana = textBox1.Text;
            string prenumePersoana = textBox2.Text;
            string cod = textBox3.Text;

            if (string.IsNullOrWhiteSpace(numePersoana) || string.IsNullOrWhiteSpace(prenumePersoana) || string.IsNullOrWhiteSpace(cod))
                {
                MessageBox.Show("Vă rugăm să completați numele și prenumele persoanei de șters.");
                return;
                }

            StergePersoana(numePersoana, prenumePersoana,cod);

        }
    }
}
