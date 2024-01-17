using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeonSprint
{
    public class Tile : GameObject
    {
        public Tile(Point location, Image image) : base(location, new Size(1180, 40), image)
        {
            BackgroundImage = image;
            SizeMode = PictureBoxSizeMode.Normal;
            BackgroundImageLayout = ImageLayout.Tile;
        }
    }
}
