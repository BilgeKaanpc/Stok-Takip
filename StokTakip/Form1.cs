using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace StokTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SQLiteConnection baglan;
        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "Select * from stok";
            dataGridView1.DataSource = CRUD.Listele(sql);
        }
        public class Baglan
        {
            public static SQLiteConnection connection = new SQLiteConnection("Data source=D:/Githubim/Stok-Takip/StokTakip/database.db;Version=3");

        }
        DataTable dt;
        void Listele()
        {
            string sql = "Select * from stok";

            dt = new DataTable();
            SQLiteDataAdapter adtr = new SQLiteDataAdapter(sql, baglan);
            adtr.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public class CRUD
        {
            static DataTable dt;
            public static DataTable Listele(string sql)
            {

                dt = new DataTable();
                SQLiteDataAdapter adtr = new SQLiteDataAdapter(sql, Baglan.connection);
                adtr.Fill(dt);
                return dt;
            }

            public static void DataAdd(TextBox t1, TextBox t2, MaskedTextBox t3,DataGridView dtm)
            {
                Baglan.connection.Open();
                SQLiteCommand ekle = new SQLiteCommand("insert into stok (urun_kodu,urun_tanimi,adet) values (@k1,@k2,@k3)",Baglan.connection);
                ekle.Parameters.AddWithValue("@k1", t1.Text);
                ekle.Parameters.AddWithValue("@k2", t2.Text);
                ekle.Parameters.AddWithValue("@k3", int.Parse(t3.Text));
                ekle.ExecuteNonQuery();
                Baglan.connection.Close();
                t1.Clear();
                t2.Clear();
                t3.Clear();

                string sql = "Select * from stok";
                dtm.DataSource = CRUD.Listele(sql);
            }
            public static void DataSearch(TextBox t1,DataGridView dgv)
            {
                Baglan.connection.Open();
                SQLiteCommand search = new SQLiteCommand("Select * From stok where urun_kodu like '%" + t1.Text + "%'", Baglan.connection);
                DataTable dt = new DataTable();
                SQLiteDataAdapter da = new SQLiteDataAdapter(search);
                da.Fill(dt);
                dgv.DataSource = dt;


                Baglan.connection.Close();
            }
            public static void DataUpdate(MaskedTextBox m1 , TextBox t1,DataGridView dtm)
            {
                Baglan.connection.Open();
                SQLiteCommand search = new SQLiteCommand("Select adet From stok where urun_kodu like '%" + t1.Text + "%'", Baglan.connection);
                SQLiteDataReader drm = search.ExecuteReader();
                int oldValue = 0;
                while (drm.Read())
                {
                    oldValue = int.Parse(drm[0].ToString());
                }
                int newValue = oldValue - int.Parse(m1.Text);

                SQLiteCommand searchUpdate = new SQLiteCommand("UPDATE stok set adet = '" + newValue + " 'WHERE urun_kodu like '%" + t1.Text + "%'", Baglan.connection);
                searchUpdate.ExecuteNonQuery();
                SQLiteCommand searchUpdateDate = new SQLiteCommand("UPDATE stok set tarih = '" + DateTime.Now + " 'WHERE urun_kodu like '%" + t1.Text + "%'", Baglan.connection);
                searchUpdateDate.ExecuteNonQuery();
                string sql = "Select * from stok";
                dtm.DataSource = CRUD.Listele(sql);
                m1.Clear();
                t1.Clear();
                Baglan.connection.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CRUD.DataUpdate(maskedTextBox2, textBox5,dataGridView1);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CRUD.DataAdd(textBox1, textBox2, maskedTextBox1,dataGridView1);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CRUD.DataSearch(textBox4, dataGridView1);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }

}