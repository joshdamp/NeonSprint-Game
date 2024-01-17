using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonSprint
{
    public class NeonOrbs : GameObject
    {
        public OrbType Type { get; private set; }

        public NeonOrbs(Point location, Image image, OrbType type)
            : base(location, new Size(35, 35), image)
        {
            Type = type;
        }
    }
}
