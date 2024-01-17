using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeonSprint
{

    internal class LoadingScreen : Form
    {
        Timer loadingTimer = new Timer();

        PictureBox loadingBG = new PictureBox();

        public LoadingScreen()
        {
            Size = new Size(1189, 565);
            DoubleBuffered = true;

            LoadingMethod();

            loadingTimer.Interval = 9300; //9.3s
            loadingTimer.Tick += new EventHandler(EndLoadingMethod);

            void LoadingMethod()
            {
                loadingBG.Size = new Size(1189, 565);
                loadingBG.Image = NeonSprint.Properties.Resources.loading;
                this.Controls.Add(loadingBG);
                loadingTimer.Start();
            }


            void EndLoadingMethod(object sender, EventArgs e)
            {
                // Stop and disable the loading timer
                loadingTimer.Stop();
                loadingTimer.Enabled = false;
                //to homescren
                HomeScreen homeScreen = new HomeScreen();
                this.Hide();
                homeScreen.ShowDialog();
                this.Close();
            }
        }
    }
}
