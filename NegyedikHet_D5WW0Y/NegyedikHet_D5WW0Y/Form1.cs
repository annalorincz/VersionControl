using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegyedikHet_D5WW0Y
{
    public partial class Form1 : Form
    {

        List<Flat> Flats;       //4.1
        RealEstateEntities context = new RealEstateEntities();      //4.2

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Flats = context.Flat.ToList();
        }
    }
}
