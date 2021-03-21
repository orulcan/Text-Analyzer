using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace textAnalyze
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            string s = @"Data Source = ""; Initial Catalog=hash;Integrated Security=SSPI";
            SqlConnection con = new SqlConnection(s);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;         
            string url = textBox1.Text;
            var client = new RestClient("https://text-analyzer.p.rapidapi.com/analyze-text/text?url="+url+"");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "KEY");
            request.AddHeader("x-rapidapi-host", "text-analyzer.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            string read = response.Content;
            string count = response.ContentLength.ToString();
            string website = response.ResponseUri.ToString().Replace("https://text-analyzer.p.rapidapi.com/analyze-text/text?url=", string.Empty); 
            

            byte[] encodedPassword = new UTF8Encoding().GetBytes(read);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty);
            label2.Text = count;
            richTextBox1.Text = read;
            try
            {
                con.Open();         
                string hashCatch = "insert into GetHash values(N'" + website + "', N'" + encoded + "', N'" + count + "', N'"+ System.DateTime.Now.ToString("dd:MM:yyyy") + "')";
                cmd.Connection = con;
                cmd.CommandText = hashCatch;
                cmd.ExecuteNonQuery();  
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
