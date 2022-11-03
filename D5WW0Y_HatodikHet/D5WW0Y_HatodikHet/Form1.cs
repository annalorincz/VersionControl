using D5WW0Y_HatodikHet.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D5WW0Y_HatodikHet
{
    public partial class Form1 : Form
    {

        private List<Ball> _balls = new List<Ball>();

        private BallFactory _factory;
        public BallFactory Factory      //egy példány
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
