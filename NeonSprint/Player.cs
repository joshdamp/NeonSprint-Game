using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonSprint
{
    public class Player : GameObject
    {
        public Player(Point location, Image image) : base(location, new Size(90, 90), image)
        {

            SendToBack();
        }
    }
}
