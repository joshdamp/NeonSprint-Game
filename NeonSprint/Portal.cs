using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonSprint
{
    public class Portal : GameObject
    {
        public Portal(Point location, Image image) : base(location, new Size(300, 270), image)
        {
            BringToFront();
        }
    }
}
