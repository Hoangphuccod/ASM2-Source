using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WaterBillManagement
{
    public partial class row : Form
    {
        string[] names = new string[100];
        int[] types = new int[100];
        double[] lastMonths = new double[100];
        double[] thisMonths = new double[100];
        int[] numberPeoples = new int[100];
        double[] usages = new double[100];
        double[] totalCosts = new double[100];

        int n = 0;

        public row()
        {
            InitializeComponent();
        }
        private void Display()
        {

            dataGridView1.DataSource = null;
            var bills = new DataTable(); 
            bills.Columns.Add("Name"); 
            bills.Columns.Add("Last Month"); 
            bills.Columns.Add("This Month"); 
            bills.Columns.Add("Usage");
            bills.Columns.Add("Total Cost");
            for (int i = 0; i < n; i++)
            {
                bills.Rows.Add(names[i], lastMonths[i], thisMonths[i], usages[i], totalCosts[i]);
            }
            dataGridView1.DataSource = bills;
            dataGridView1.Refresh();
        }
        

        public void ClearData()
        {
            txtctn.Text = "";
            txtwlm.Text = "";
            txtwtm.Text = "";
            txtnum.Text = "";
            cbtype.Text = "";
            Display();
        }
        private void cbCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private double GetPriceUnit(int customerType, double usageTotal)
        {
            double priceUnit = 0;

            if (customerType == 0)
            {
                int numberPeople = int.Parse(txtnum.Text);
                double avg = usageTotal / numberPeople;

                if (avg <= 10)
                {
                    priceUnit = 5973;
                }
                else if (avg <= 20)
                {
                    priceUnit = 7052;
                }
                else if (avg <= 30)
                {
                    priceUnit = 8699;
                }
                else
                {
                    priceUnit = 15929;
                }
            }
            else if (customerType == 1)
            {
                priceUnit = 9955;
            }
            else if (customerType == 2)
            {
                priceUnit = 11615;
            }
            else if (customerType == 3)
            {
                priceUnit = 22068;
            }

            return priceUnit;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Export_Click(object sender, EventArgs e)
        {
            string customerName = txtctn.Text;
            double waterMeterLastMonth = double.Parse(txtwlm.Text);
            double waterMeterThisMonth = double.Parse(txtwtm.Text);
            int customerType = cbtype.SelectedIndex;
            double usageTotal = waterMeterThisMonth - waterMeterLastMonth;
            double priceUnit = GetPriceUnit(customerType, usageTotal);
            double totalBill = priceUnit * usageTotal * 1.1;

            MessageBox.Show("Customer name: " + customerName + "\n" +
                            "Usage: " + usageTotal + " m3" + "\n" +
                            "Bill: " + totalBill + " VND");

        }

        private void save_Click(object sender, EventArgs e)
        {
            string customerName = txtctn.Text;
            double waterMeterLastMonth = double.Parse(txtwlm.Text);
            double waterMeterThisMonth = double.Parse(txtwtm.Text);
            int customerType = cbtype.SelectedIndex;
            double usageTotal = waterMeterThisMonth - waterMeterLastMonth;
            double priceUnit = GetPriceUnit(customerType, usageTotal);
            double totalBill = priceUnit * usageTotal * 1.1;

            names[n] = customerName;
            types[n] = customerType;
            lastMonths[n] = waterMeterLastMonth;
            thisMonths[n] = waterMeterThisMonth;
            numberPeoples[n] = (customerType == 0) ? int.Parse(txtnum.Text) : 0;
            usages[n] = usageTotal;
            totalCosts[n] = totalBill;
            n++;

            Display();
            MessageBox.Show("Saved successfully");
            ClearData();
        }


        private void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            txtctn.Text = names[index];
            cbtype.SelectedIndex = types[index];
            txtwlm.Text = lastMonths[index].ToString();
            txtwtm.Text = thisMonths[index].ToString();
            txtnum.Text = (types[index] == 0) ? numberPeoples[index].ToString() : "";
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cbtype.SelectedIndex;

            if (selectedIndex == 0)
            {
                label5.Visible = true;
                txtnum.Visible = true;
            }
            else
            {
                label5.Visible = false;
                txtnum.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox11.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter the name of a customer.");
                return;
            }

            var searchResults = new DataTable();
            searchResults.Columns.Add("Name");
            searchResults.Columns.Add("Last Month");
            searchResults.Columns.Add("This Month");
            searchResults.Columns.Add("Usage");
            searchResults.Columns.Add("Total Cost");

            for (int i = 0; i < n; i++)
            {
                if (names[i].IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    searchResults.Rows.Add(names[i], lastMonths[i], thisMonths[i], usages[i], totalCosts[i]);
                }
            }

            if (searchResults.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = searchResults;
            }
            else
            {
                MessageBox.Show("No matching customer found.");
                dataGridView1.DataSource = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var customerData = new List<(string Name, int Type, double LastMonth, double ThisMonth, int NumberPeople, double Usage, double TotalCost)>();

            for (int i = 0; i < n; i++)
            {
                customerData.Add((names[i], types[i], lastMonths[i], thisMonths[i], numberPeoples[i], usages[i], totalCosts[i]));
            }

            customerData = customerData.OrderBy(data => data.TotalCost).ToList();

            for (int i = 0; i < customerData.Count; i++)
            {
                names[i] = customerData[i].Name;
                types[i] = customerData[i].Type;
                lastMonths[i] = customerData[i].LastMonth;
                thisMonths[i] = customerData[i].ThisMonth;
                numberPeoples[i] = customerData[i].NumberPeople;
                usages[i] = customerData[i].Usage;
                totalCosts[i] = customerData[i].TotalCost;
            }

            Display();
            MessageBox.Show("Sorted by total cost in ascending order.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;

                for (int i = selectedIndex; i < n - 1; i++)
                {
                    names[i] = names[i + 1];
                    types[i] = types[i + 1];
                    lastMonths[i] = lastMonths[i + 1];
                    thisMonths[i] = thisMonths[i + 1];
                    numberPeoples[i] = numberPeoples[i + 1];
                    usages[i] = usages[i + 1];
                    totalCosts[i] = totalCosts[i + 1];
                }

                n--;
                Display();
                MessageBox.Show("Row deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
    }
}
