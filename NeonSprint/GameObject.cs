using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NeonSprint
{
    //forgame objects
    public class GameObject : PictureBox
    {
        public GameObject(Point location, Size size, Image image)
        {
            Location = location;
            Size = size;
            Image = image;
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
        }
    }

}
