using D5WW0Y_OtodikHet.Entities;
using D5WW0Y_OtodikHet.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace D5WW0Y_OtodikHet
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            var mnbCurrencie = new MNBArfolyamServiceSoapClient();      //8.
            var request = new GetExchangeRatesRequestBody();
            var response = mnbCurrencie.GetCurrencies(request);
            var result = response.GetCurrenciesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                var childElement = (XmlElement)element.ChildNodes[0];
                var c = (childElement.InnerText).ToString();
                if (childElement == null) continue;
            }

            RefreshData();
        }

        private void RefreshData()
        {
            Rates.Clear();
            //WebServer();
            dataGridView1.DataSource = Rates;
            XmlLoad(WebServer());
            DataDiagram();
            //comboBox1.DataSource = Currencies;
        }

        private string WebServer()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = (comboBox1.SelectedItem).ToString(),
                startDate = (dateTimePicker1.Value).ToString(),
                endDate = (dateTimePicker2.Value).ToString()
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            return result;
        } 
        
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        private void XmlLoad(string result)
        {
            var xml = new XmlDocument();        //xml példányosítás
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                //Dátum - "date" nevű attribútum értéke kell -> GetAttribure
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                //Valuta - lekérdezi az aktuális elem első gyermek elemét egy változóba (ChildNodes)
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                //Érték - 2 rész: alapegység(unit), egységhez tartozó érték(InnerText)
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        private void DataDiagram()
        {
            chart1.DataSource = Rates;   //formon új chart -> adatforrása Rates

            var series = chart1.Series[0];      //Series = tömb, alapértelmezetten egy elemű
            series.ChartType = SeriesChartType.Line;        //adatsor típusa
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;                 //kétszeres vastagság

            var legend = chart1.Legends[0];         //ne látszódjon oldalt a címke
            legend.Enabled = false;

            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;  //ne látszódjanak a fő grid vonalak
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
