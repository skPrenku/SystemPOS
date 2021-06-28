using System;
using System.Windows.Forms;

namespace SystemPOS
{
    public partial class ProgSettings : Form
    {
        public static string companyIcon = Properties.Settings.Default.CompanyIconPath;
        public static string companyName = Properties.Settings.Default.CompanyName;
        public static string barCodeIdent = Properties.Settings.Default.BarcodeIdentifier;


        public ProgSettings()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ICO icon|*.ico";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxIconPath.Text = ofd.FileName;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if(checkBox1.Checked == true)
            {
                Properties.Settings.Default.CompanyIconPath = txtBoxIconPath.Text;
                Properties.Settings.Default.CompanyName = txtBoxName.Text;
                Properties.Settings.Default.BarcodeIdentifier = txtBoxBarCode.Text;
                Properties.Settings.Default.Save();
            }
            if (checkBox1.Checked == false)
            {
                Properties.Settings.Default.CompanyIconPath = "";
                Properties.Settings.Default.CompanyName = "";
                Properties.Settings.Default.BarcodeIdentifier = "";
                Properties.Settings.Default.Save();
            }

           
        }

        private void ProgSettings_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.CompanyIconPath != string.Empty)
            {
                txtBoxIconPath.Text = Properties.Settings.Default.CompanyIconPath;
            }

            if(Properties.Settings.Default.CompanyName != string.Empty)
            {
                txtBoxName.Text = Properties.Settings.Default.CompanyName;
            }

            if(Properties.Settings.Default.BarcodeIdentifier != string.Empty)
            {
                txtBoxBarCode.Text = Properties.Settings.Default.BarcodeIdentifier;
            }



        }


    }
}
