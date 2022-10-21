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
            //WebServer();
            dataGridView1.DataSource = Rates;
            XmlLoad(WebServer());
            DataDiagram();
        }

        private string WebServer()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            return result;
        } 
        
        BindingList<RateData> Rates = new BindingList<RateData>();

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

            var series = chart1.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chart1.Legends[0];
            legend.Enabled = false;

            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

       
    }
}
