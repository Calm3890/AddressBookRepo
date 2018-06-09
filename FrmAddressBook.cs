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
namespace AddressBook
{
    public partial class FrmAddressBook : Form
    {
        public FrmAddressBook()
        {
            InitializeComponent();
        }
        int[] selectedRows = new int[25];
        int selectedRowsAfterFilter = 0;
        private void FrmAddressBook_Load(object sender, EventArgs e)
        {
            try
            {
                this.dgvData.Rows.Clear();
                if (File.Exists("addressbook.csv"))
                {
                    String[] arrLine = File.ReadAllLines("addressbook.csv");
                    if (arrLine.Length > 0)
                    {
                        foreach (String item in arrLine)
                        {
                            string[] arrItem = item.Split(';');
                            this.dgvData.Rows.Add(new string[] {
                                arrItem[0],
                                arrItem[1],
                                arrItem[2],
                                arrItem[3],
                                arrItem[4],
                                arrItem[5],

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.lblBykRecordData.Text = $"{this.dgvData.Rows.Count.ToString("n0")}Record Data.";
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            FrmTambahData frm = new FrmTambahData(true);
            this.Hide();
            frm.ShowDialog();

            this.Show();
            FrmAddressBook_Load(null, null);
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedRowsAfterFilter == 0)
            {
                try
                {
                    if (File.Exists("addressbook.csv"))
                    {
                        string[] arrLine = File.ReadAllLines("addressbook.csv");
                        string[] newArrLine = new string[arrLine.Length - 1];
                        for (int i = 0; i < arrLine.Length; i++)
                        {
                            if (dgvData.Rows[i].Selected == true)
                            {
                                for (int j = i; j < arrLine.Length - 1; j++)
                                {
                                    arrLine[j] = arrLine[j + 1];
                                }
                                break;
                            }
                        }
                        //newArrLine = arrLine; <-- fail
                        for (int i = 0; i < newArrLine.Length; i++)
                        {
                            newArrLine[i] = arrLine[i];
                        }
                        File.WriteAllLines("addressbook.csv", newArrLine);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AddressBook", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                try
                {
                    if (File.Exists("addressbook.csv"))
                    {
                        string[] arrLine = File.ReadAllLines("addressbook.csv");
                        string[] newArrLine = new string[arrLine.Length - 1];
                        for (int i = 0; i < arrLine.Length; i++)
                        {
                            if (dgvData.Rows[i].Selected == true)
                            {
                                for (int j = selectedRows[i]; j < arrLine.Length - 1; j++)
                                {
                                    arrLine[j] = arrLine[j + 1];
                                }
                                break;
                            }
                        }

                        for (int i = 0; i < newArrLine.Length; i++)
                        {
                            newArrLine[i] = arrLine[i];
                        }
                        File.WriteAllLines("addressbook.csv", newArrLine);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AddressBook", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            FrmAddressBook_Load(null, null);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count > 0)
            {
                DataGridViewRow row = this.dgvData.SelectedRows[0];
                AddressBook addrBook = new AddressBook();
                addrBook.Nama = row.Cells[0].Value.ToString();
                addrBook.Alamat = row.Cells[1].Value.ToString();
                addrBook.Kota = row.Cells[2].Value.ToString();
                addrBook.NoHp = row.Cells[3].Value.ToString();
                addrBook.TanggalLahir = Convert.ToDateTime(row.Cells[4].Value).Date;
                addrBook.Email = row.Cells[5].Value.ToString();
                FrmTambahData form = new FrmTambahData(false, addrBook);
                if (form.Run(form)) {
                    FrmAddressBook_Load(null, null);
                }
            }
        }
    }
}
