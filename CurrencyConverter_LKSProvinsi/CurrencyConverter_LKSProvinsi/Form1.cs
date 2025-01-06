using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurrencyConverter_LKSProvinsi
{
    public partial class Form1 : Form
    {
        CurrencyConverterEntities db = new CurrencyConverterEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            periodBindingSource.DataSource = db.Periods.ToList();
            currencyBindingSource.DataSource = db.Currencies.ToList();
            textBox1_TextChanged(sender, e);

        }

        private void currencyBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (currencyBindingSource.Current is Currency currency)
            {
                currencyBinding2.DataSource = db.Currencies.Where(s => s.id != currency.id).AsNoTracking().ToList();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var previous_ex = textBox1.Text;
            try
            {
                decimal text1 = decimal.Parse(textBox1.Text);
                if (currencyBindingSource.Current is Currency currency1 && periodBindingSource.Current is Period period && currencyBinding2.Current is Currency currency2)
                {
                    var exchangeRate = db.USDExchangeRates.FirstOrDefault(s => s.period_id == period.id && s.currency_id == currency1.id).rate;
                    var exchangeRate2 = db.USDExchangeRates.FirstOrDefault(s => s.period_id == period.id && s.currency_id == currency2.id).rate;
                    if (exchangeRate != 0)
                    {
                        var hasil = (text1 * exchangeRate2) / exchangeRate;
                        textBox2.Text = hasil.ToString("F3");
                    }
                    else
                    {
                        textBox1.Text = "1";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("mohon masukan nilai decimal yang tepat");
                textBox1.Text = "1";
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tempDataSource = currencyBindingSource.DataSource;
            currencyBindingSource.DataSource = currencyBinding2.DataSource;
            currencyBinding2.DataSource = tempDataSource;
            textBox1_TextChanged(sender, e);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }
    }
}
