using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectPOS
{
    public partial class SalesForm : Form
    {

      
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\ProgramData\LocalDataBase.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd;

        SqlDataAdapter adapter;

        double total = 0;
        int items = 0;

   

        

        public SalesForm()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy hh:mm tt";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy hh:mm tt";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {


            con.Open();
            if (string.IsNullOrEmpty(txtBoxEmri.Text) && string.IsNullOrEmpty(txtBoxClientId.Text))
            {
                adapter = new SqlDataAdapter("select * from SalesTable where (productSold between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "')", con);

            }
            else if (!string.IsNullOrEmpty(txtBoxEmri.Text) && !string.IsNullOrEmpty(txtBoxClientId.Text))
            {
                adapter = new SqlDataAdapter("select * from SalesTable where (productSold between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "') and (productName = '" + txtBoxEmri.Text + "' and clientId = '" + txtBoxClientId.Text + "')", con);

            }

            else
                adapter = new SqlDataAdapter("select * from SalesTable where (productSold between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "') and (productName like '" + txtBoxEmri.Text + "' or clientId like '" + txtBoxClientId.Text + "')", con);

            //adapter.SelectCommand.Parameters.Add(new SqlParameter("@Date1", dateTimePicker1.Value.ToString("dd/MM/yyyy hh:mm tt")));
            //adapter.SelectCommand.Parameters.Add(new SqlParameter("@Date2", dateTimePicker2.Value.ToString("dd/MM/yyyy hh:mm tt")));
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            con.Close();
            dataGridViewSales.DataSource = dt;

            foreach (DataGridViewRow  item in dataGridViewSales.Rows)
            {
                total += Convert.ToDouble(item.Cells[3].Value);
                items++;
            }

            lblTotalSold.Text = "Total: " + total.ToString();
            if(items == 2)
            {
                lblTotalProdukts.Text = (items -1) + " Product";
            }
            else
                lblTotalProdukts.Text = (items - 1) + " Products";

            items = 0;
            total = 0;

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           DialogResult dialogr =  MessageBox.Show("You are about to clear the complete Sales History", "Confirm", MessageBoxButtons.YesNo);

            if (dialogr == DialogResult.Yes)
            {
                cmd = new SqlCommand("delete from SalesTable", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now.Date;
            dateTimePicker2.MaxDate = DateTime.Now.Date;
        }
    }
}
