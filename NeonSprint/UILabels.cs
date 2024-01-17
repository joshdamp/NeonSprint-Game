
//labels when player gets a power up
using System.Drawing;
using System.Windows.Forms;

namespace NeonSprint
{
    public class ScoreLabel : Label
    {
        public ScoreLabel()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.LightGray;
            AutoSize = true;
            Font = new Font("Impact", 18.2f, FontStyle.Bold);
            Location = new Point(20, 10); // Adjust the position as needed
        }
    }

    public class InvinciLabel : Label
    {
        public InvinciLabel()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.Yellow;
            AutoSize = true;
            Font = new Font("Impact", 16.2f, FontStyle.Bold);
            Location = new Point(800, 10); // Adjust the position as needed
        }
    }

    public class MultiplierLabel : Label
    {
        public MultiplierLabel()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.LightBlue;
            AutoSize = true;
            Font = new Font("Impact", 16.2f, FontStyle.Bold);
            Location = new Point(500, 10); // Adjust the position as needed
        }
    }
    
    public class LifeLabel : Label
    {
        public LifeLabel()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.Pink;
            AutoSize = true;
            Font = new Font("Impact", 16.2f, FontStyle.Bold);
            Location = new Point(220, 10); // Adjust the position as needed
        }
    }
}
