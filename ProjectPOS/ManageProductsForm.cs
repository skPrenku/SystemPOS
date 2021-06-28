using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ProjectPOS
{
    public partial class ManageProductsForm : Form
    {



        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\ProgramData\LocalDataBase.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd;

        public static SqlDataAdapter adapter;
        public static SqlDataReader reader;
        List<Products> dataBase = new List<Products>();
        public ManageProductsForm()
        {
            InitializeComponent();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxBarcode.Text) || string.IsNullOrEmpty(txtBoxPrice.Text) || string.IsNullOrEmpty(txtBoxName.Text))
            {
                MessageBox.Show("Keni harruar ndonje Fushe te zbrazet.");
            }
            else if(txtBoxBarcode.Text.Length <3)
            {
                MessageBox.Show("Barcodi duhet ti ket me shum se 3 shkronja");
            }
            else
            {
                if (ItemExists(txtBoxBarcode.Text))
                {
                    MessageBox.Show("Ky Barcod Egziston ne Databaze");
                }
                else
                {
                    if (string.IsNullOrEmpty(txtBoxBarcode.Text) && string.IsNullOrEmpty(txtBoxPrice.Text) && string.IsNullOrEmpty(txtBoxId.Text) && string.IsNullOrEmpty(txtBoxName.Text))
                    {
                        MessageBox.Show("Fushat jan te zbrazeta.");
                    }

                    else
                    {
                        cmd = new SqlCommand("insert into MyTable(productName,productBarcode,productPrice,productCreated) values(@NAME,@BARCODE,@PRICE,@CREATED)", con);
                        con.Open();
                        // cmd.Parameters.AddWithValue("@ID", int.Parse(txtBoxId.Text));
                        cmd.Parameters.AddWithValue("@NAME", txtBoxName.Text);
                        cmd.Parameters.AddWithValue("@BARCODE", txtBoxBarcode.Text);
                        cmd.Parameters.AddWithValue("@PRICE", double.Parse(txtBoxPrice.Text));
                        cmd.Parameters.AddWithValue("@CREATED", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ReloadGridView();

                        dbActialisation();

                        clearTextBoxes();

             

                    }
                }
            }

        }

        private void ReloadGridView()
        {

            con.Open();
            adapter = new SqlDataAdapter("select * from MyTable", con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            con.Close();

            dataGridViewMngr.DataSource = dt;


            

        }

        private void ManageProductsForm_Load(object sender, EventArgs e)
        {
            ReloadGridView();
            dbActialisation();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {



                if (string.IsNullOrEmpty(txtBoxBarcode.Text) && string.IsNullOrEmpty(txtBoxPrice.Text) && string.IsNullOrEmpty(txtBoxId.Text) && string.IsNullOrEmpty(txtBoxName.Text))
                {
                    MessageBox.Show("Te gjitha Hapsirat jan te Zbrazeta, per te vazhduar ju lutem qe separi te vendosni te dhena te reja");
                }
                else
                {
                    cmd = new SqlCommand("update MyTable set productName=@NAME,productBarcode=@BARCODE,productPrice=@PRICE where id = @ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", int.Parse(txtBoxId.Text));
                    cmd.Parameters.AddWithValue("@NAME", txtBoxName.Text);
                    cmd.Parameters.AddWithValue("@BARCODE", txtBoxBarcode.Text);
                    cmd.Parameters.AddWithValue("@PRICE", Convert.ToDouble(txtBoxPrice.Text));
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ReloadGridView();
                }
       
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

            if (txtBoxBarcode.Text == string.Empty && txtBoxName.Text == string.Empty && txtBoxPrice.Text == string.Empty)
            {
                ReloadGridView();
            }
            else
            {
                


                con.Open();
              adapter = new SqlDataAdapter("select * from MyTable where productBarcode like '"+txtBoxBarcode.Text+ "' or productName like'" + txtBoxName.Text + "' or productPrice like'" + txtBoxPrice.Text + "'  ", con);
              //adapter = new SqlDataAdapter("select * from MyTable where (productName is null or productName='"+txtBoxName.Text+ "')", con);

                DataTable dt = new DataTable();
            adapter.Fill(dt);
            con.Close();

            dataGridViewMngr.DataSource = dt;


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!ItemExists(txtBoxBarcode.Text))
            {
                MessageBox.Show("Ky Barcod Nuk Egziston ne Databaze");
            }
            else
            {
                if (string.IsNullOrEmpty(txtBoxBarcode.Text) && (string.IsNullOrEmpty(txtBoxId.Text)))
                {
                    MessageBox.Show("Per te Fshir te Dhena nga databaza na duhet ID ose BARCODI i Produktit.");
                }
                else
                {
                    cmd = new SqlCommand("delete from MyTable where productBarcode=@BARCODE or id=@ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@BARCODE", txtBoxBarcode.Text);
                    cmd.Parameters.AddWithValue("@ID", txtBoxId.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ReloadGridView();

                }
            }

            txtBoxId.Text = "";
            txtBoxBarcode.Text = "";
            txtBoxName.Text = "";
            txtBoxPrice.Text = "";

        }



        private void dataGridViewMngr_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtBoxId.Text = dataGridViewMngr.CurrentRow.Cells["id"].Value.ToString();
            txtBoxBarcode.Text = dataGridViewMngr.CurrentRow.Cells["productBarcode"].Value.ToString();
            txtBoxName.Text = dataGridViewMngr.CurrentRow.Cells["productName"].Value.ToString();
            txtBoxPrice.Text = dataGridViewMngr.CurrentRow.Cells["productPrice"].Value.ToString();
        }

        private void dbActialisation()
        {
            cmd = new SqlCommand("select * from MyTable", con);
            con.Open();
            reader = cmd.ExecuteReader();


            while(reader.Read())
            {
                dataBase.Add(new Products() { Id = int.Parse(reader["id"].ToString()), Barcode = reader["productBarcode"].ToString(), Name = reader["productName"].ToString(), Price = double.Parse(reader["productPrice"].ToString()) });
            }

            con.Close();
        }






        private bool ItemExists(string barcode)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();


                string sql = "SELECT * FROM MyTable WHERE productBarcode = @CODE";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CODE",barcode);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;

                MessageBox.Show(msg, ex.Message);


            }
            finally
            {
                con.Close();
            }

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearTextBoxes();
        }

        private void clearTextBoxes()
        {
            txtBoxBarcode.Text = "";
            txtBoxId.Text = "";
            txtBoxName.Text = "";
            txtBoxPrice.Text = "";
        }
    }

}
