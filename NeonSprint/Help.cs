using System;
using System.Drawing;
using System.Windows.Forms;
using ButtonBackToMain;
namespace NeonSprint
    
{
    internal class Help : Form
    {
        public Help()
        {
            this.Size = new Size(1189, 595);
            
            PictureBox helpBG = new PictureBox();
            helpBG.Image = NeonSprint.Properties.Resources.Help1;
            helpBG.Size = new Size(1189, 565);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackgroundImage = (NeonSprint.Properties.Resources.Help1);
            this.BackgroundImageLayout = ImageLayout.Stretch;


            MaximizeBox = false;

            PictureBox btBack = ButtonBackToMain.CreateBackButton.CreateMainMenuButton();
            btBack.BackColor = Color.Transparent;
            btBack.Click += new EventHandler(btBack_Click);


            
            this.Controls.Add(btBack);
            this.Controls.Add(helpBG);
        }

        public void btBack_Click(object sender, EventArgs e)
        {
            ButtonBackToMain.CreateBackButton.GoBack(this, () => new HomeScreen());
        }


    }
}
