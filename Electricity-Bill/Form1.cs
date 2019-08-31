using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace Electricity_Bill
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int iPrevious;
        int iCurrent;        
        int iSubUnits;
        SqlCommand cmd;
        string sQuery;
        string sConString;
        float fUnits;

        private void btnShow_Click(object sender, EventArgs e)
        {            
            try
            {
                iPrevious = Convert.ToInt32(txtPrevious.Text);
                iCurrent = Convert.ToInt32(txtCurrent.Text);                
                fUnits = float.Parse(txtUnit.Text);
                iSubUnits = iCurrent - iPrevious;
                if (iSubUnits > 0)
                {
                    Thread.Sleep(2);
                    txtAmount.Text = (iSubUnits * fUnits).ToString();
                }
                else
                {
                    MessageBox.Show("Current unit is less then Previous \n\tCheck Again");
                    return;
                }
                using (SqlConnection con = new SqlConnection(sConString))
                {
                    con.Open();
                    sQuery = "insert into tbl_Bill_Details(Previous_Reading,Current_Reading,Per_Unit,Payable_Amount,Payable_Units,Check_DateTime) values(" + txtPrevious.Text + "," + txtCurrent.Text + "," + txtUnit.Text + "," + txtAmount.Text + "," + iSubUnits + "," + DateTime.Now.ToString("MM/dd/yyyy") + ")";                    
                    cmd = new SqlCommand(sQuery, con);
                    cmd.ExecuteNonQuery();
                }
                txtPrevious.Enabled = false;
                txtCurrent.Enabled = false;
                txtUnit.Enabled = false;
                btnShow.Enabled = false;
                btnContinue.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Enter The Value");
            }    
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnContinue.Enabled = false;
            sConString = "Data Source=AETELELINK-PC;Initial Catalog=Electricity_Bill;Persist Security Info=True;User ID=sa;Password=sa";
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            txtPrevious.Enabled = true;
            txtCurrent.Enabled = true;
            txtUnit.Enabled = true;
            txtAmount.Text = "";
            txtCurrent.Text = "";
            txtPrevious.Text = "";
            txtUnit.Text = "";
            btnContinue.Enabled = false;
            btnShow.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.Show();
            using (SqlConnection con = new SqlConnection(sConString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from tbl_Bill_Details",con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
            }
        }
    }
}
