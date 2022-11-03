using D5WW0Y_HatodikHet.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D5WW0Y_HatodikHet.Entities
{
    class Car : Toy
    {
        protected override void DrawImage(Graphics g)
        {
            Image imageFile = Image.FromFile("Images/car.png");
            g.DrawImage(imageFile, new Rectangle(0, 0, Width, Height));
        }
    }
}
