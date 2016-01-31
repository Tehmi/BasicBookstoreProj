

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BookStore
{
 
    public partial class frmBookDetails : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        SqlDataAdapter da;
        DataSet ds;
        string op;
        string addnew;
        string rec;

    
        public frmBookDetails()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=FACULTY03\APTECH;Initial Catalog=BookStore;Integrated Security=True");
            da = new SqlDataAdapter();
            ds = new DataSet();
        }

        // Loads data from the database
        private void frmBookDetails_Load(object sender, EventArgs e)
        {
            int rows = 0;
            lvwBookDetails.Items.Clear();
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            ds.Clear();
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM BookMaster";
            da.SelectCommand = cmd;
            da.Fill(ds, "BookMaster");
            for (rows = 0; rows < ds.Tables[0].Rows.Count; rows++)
            {
                lvwBookDetails.Items.Add(ds.Tables[0].Rows[rows].ItemArray[0].ToString());
                lvwBookDetails.Items[rows].SubItems.Add(ds.Tables[0].Rows[rows].ItemArray[1].ToString());
                lvwBookDetails.Items[rows].SubItems.Add(ds.Tables[0].Rows[rows].ItemArray[2].ToString());
                lvwBookDetails.Items[rows].SubItems.Add(ds.Tables[0].Rows[rows].ItemArray[3].ToString());
                lvwBookDetails.Items[rows].SubItems.Add(ds.Tables[0].Rows[rows].ItemArray[4].ToString());
            }
            ClearControls();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Displayrec();
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                txtSearch.Enabled = true;
                btnSearch.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                txtSearch.Enabled = false;
                btnSearch.Enabled = false;
            }
            op = "";
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnAddNew.Enabled = true;
            DisableControls();
        }

        // Deletes the rec
        private void btnDelete_Click(object sender, EventArgs e)
        {
            rec = "Title: " + txtTitle.Text + "\nLanguage: " + txtLanguage.Text + "\nAuthor: " + txtAuthor.Text + "\nPages: " + txtPages.Text + "\nPrice: " + txtPrice.Text;
            addnew = "DELETE FROM BookMaster WHERE Title='" + txtTitle.Text + "'";
            if (!dr.IsClosed)
                dr.Close();
            cmd = con.CreateCommand();
            cmd.CommandText = addnew;
            cmd.ExecuteNonQuery();
            MessageBox.Show("The rec has been deleted sucessfully.\n" +
rec, "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
            frmBookDetails_Load(null, null);
        }

        // Displays the rec
        void Displayrec()
        {
            txtTitle.Text = dr.GetString(0);
            txtLanguage.Text = dr.GetString(1);
            txtAuthor.Text = dr.GetString(2);
            txtPages.Text = dr.GetInt32(3).ToString();
            txtPrice.Text = dr.GetInt32(4).ToString();
        }

        // Clears the controls
        void ClearControls()
        {
            txtTitle.Text = "";
            txtLanguage.Text ="";
            txtAuthor.Text = "";
            txtPages.Text = "";
            txtPrice.Text = "";
        }

        // Adds a new rec
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearControls();
            op = "insert";
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAddNew.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            txtSearch.Enabled = false;
            btnSearch.Enabled = false;
            EnableControls();
            txtTitle.Focus();
        }

        // Edits the rec
        private void btnEdit_Click(object sender, EventArgs e)
        {
            op = "update";
            btnAddNew.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            txtSearch.Enabled = false;
            btnSearch.Enabled = false;
            EnableControls();
            txtTitle.Focus();
        }

        // Validates and saves the rec in the database.
        private void btnSave_Click(object sender, EventArgs e)
        {
            int index = 0;
            if(txtTitle.Text == "")
            {
                MessageBox.Show("Please enter the title.","Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTitle.Focus();
                return;
            }
            else if(txtLanguage.Text == "")
            {
                MessageBox.Show("Please enter the language.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLanguage.Focus();
                return;
            }
            else if(txtAuthor.Text == "")
            {
                MessageBox.Show("Please enter the author.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAuthor.Focus();
                return;
            }
            else if(txtPages.Text == "")
            {
                MessageBox.Show("Please enter the pages.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPages.Focus();
                return;
            }
            else if(txtPrice.Text == "")
            {
                MessageBox.Show("Please enter the price.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrice.Focus();
                return;
            }
            
            if (txtPages.Text != "")
            {
                try
                {
                    index = Convert.ToInt32(txtPages.Text);
                    if (index < 0)
                    {
                        MessageBox.Show("Please enter a positive value for pages.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPages.Focus();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter a numeric value for pages.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPages.Focus();
                    return;
                }
            }
            
            if (txtPrice.Text != "")
            {
                try
                {
                    index = Convert.ToInt32(txtPrice.Text);
                    if (index < 0)
                    {
                        MessageBox.Show("Please enter a positive value for price.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPrice.Focus();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter a numeric value for price.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPrice.Focus();
                    return;
                }
            }
            
            if (op == "insert")
                addnew = "INSERT INTO BookMaster VALUES ('" + txtTitle.Text + "','" + txtLanguage.Text + "','" + txtAuthor.Text + "'," + txtPages.Text + "," + txtPrice.Text +")";
            else if (op == "update")
                addnew = "UPDATE BookMaster SET Language='" + txtLanguage.Text + "',Author='" + txtAuthor.Text + "',Pages=" + txtPages.Text + ",Price=" + txtPrice.Text + " where Title = '" + txtTitle.Text +"'";
            
            if(op != "")
            {
                if (!dr.IsClosed)
                    dr.Close();
                cmd = con.CreateCommand();
                cmd.CommandText = addnew;
                cmd.ExecuteNonQuery();
                rec = "Title: " + txtTitle.Text + "\nLanguage: " + txtLanguage.Text + "\nAuthor: " + txtAuthor.Text + "\nPages: " + txtPages.Text + "\nPrice: " + txtPrice.Text;
                MessageBox.Show("The following details have been saved sucessfully.\n" + rec, "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmBookDetails_Load(null, null);
            }
        }

        // Searches the rec in the database
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                ds.Clear();
                cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM BookMaster WHERE Title like '" + txtSearch.Text + "%'";
                da.SelectCommand = cmd;
                da.Fill(ds, "BookMaster");
                ClearControls();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                    Displayrec();
                else
                    MessageBox.Show("No rec is found.", "Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter the search text.","Book Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.Focus();
            }
        }

        // Cancels the op
        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmBookDetails_Load(null, null);
        }

        // Loads the data
        private void lvwBookDetails_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM BookMaster";
            dr = cmd.ExecuteReader();
            dr.Read();
            while (dr.GetString(0) != lvwBookDetails.SelectedItems[0].Text)
                dr.Read();
            Displayrec();
        }

        // Disables the controls
        void DisableControls()
        {
            txtTitle.Enabled = false;
            txtLanguage.Enabled = false;
            txtAuthor.Enabled = false;
            txtPages.Enabled = false;
            txtPrice.Enabled = false;
        }

        // Enables the controls
        void EnableControls()
        {
            txtTitle.Enabled = true;
            txtLanguage.Enabled = true;
            txtAuthor.Enabled = true;
            txtPages.Enabled = true;
            txtPrice.Enabled = true;
        }
    }
}