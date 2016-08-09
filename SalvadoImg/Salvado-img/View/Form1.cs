using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace View
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnclick_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //exibir a imagem no form
                string nomeArquivo = openFileDialog1.FileName;
                Bitmap bitmap = new Bitmap(nomeArquivo);
                pbImg.Image = bitmap;

                //
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                //
                byte[] foto = ms.ToArray();

                SqlConnection conn = new SqlConnection(@"Data Source=LOCALHOST;Initial Catalog=SalvandoImagem;Integrated Security=True");
                SqlCommand comand = new SqlCommand("INSERT INTO Cliente_Foto (IdCliente, Foto) VALUES (1, @foto)", conn);

                SqlParameter parameter = new SqlParameter("@foto", SqlDbType.Binary);
                parameter.Value = foto;

                comand.Parameters.Add(parameter);

                conn.Open();
                comand.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //exibir img do banco
            SqlConnection conn = new SqlConnection(@"Data Source=LOCALHOST;Initial Catalog=SalvandoImagem;Integrated Security=True");
            SqlCommand comand = new SqlCommand("SELECT Foto FROM Cliente_Foto WHERE IdCliente = 1", conn);

            conn.Open();
            SqlDataReader reader = comand.ExecuteReader();

            Image imagem = null;
            if (reader.Read())
            {
                byte[] foto = (byte[])reader["Foto"];

                MemoryStream ms = new MemoryStream(foto);
                imagem = Image.FromStream(ms);
            }

            pbImg.Image = imagem;

            conn.Close();
        }
    }
}
