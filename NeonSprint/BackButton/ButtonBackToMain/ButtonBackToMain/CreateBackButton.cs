using System.Windows.Forms;
using System.Drawing;
using System;

namespace ButtonBackToMain
{
    public class CreateBackButton
    {
        public static PictureBox CreateMainMenuButton()
        {
            PictureBox btBack = new PictureBox();

            // Use the correct method to load the image from the resources
            btBack.Image = Properties.Resources.backBT;
            btBack.Location = new Point(940, 400);
            btBack.BackColor = Color.Transparent;
            btBack.SizeMode = PictureBoxSizeMode.Zoom; // Set the PictureBox size mode
            btBack.BorderStyle = BorderStyle.None;
            btBack.Size = new Size(200, 150);

            return btBack;
        }
        public static void GoBack(Form currentForm, Func<Form> createHomeScreen)
        {
            currentForm.Hide();
            Form homescreen = createHomeScreen();
            homescreen.ShowDialog();
            currentForm.Close();
        }   
    }
}
