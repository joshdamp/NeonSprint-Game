using System;
using System.Windows.Forms;
using System.Drawing;
namespace NeonSprint
{
    internal class HomeScreen : Form
    {
        Label lblTitle = new Label();
        PictureBox pbStart = new PictureBox();
        PictureBox btHelp = new PictureBox();
        PictureBox leaderBoard = new PictureBox();
        PictureBox mainBG = new PictureBox();



        public HomeScreen()

        {


            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            this.Size = new Size(1189, 595);
            this.BackgroundImage = (NeonSprint.Properties.Resources.mainemenu);
            this.BackgroundImageLayout = ImageLayout.Stretch;

            mainBG.Size = new Size(1189, 565);
            mainBG.Image = NeonSprint.Properties.Resources.mainemenu;

            lblTitle.Image = (NeonSprint.Properties.Resources.NEON_SPRINT__1_);
            lblTitle.Size = new Size(370, 69);
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(378, 50);
            lblTitle.AutoSize = false;
            

            pbStart.Image = (NeonSprint.Properties.Resources.Play1);
            pbStart.BackColor = Color.Transparent;
            pbStart.Size = new Size(263, 69);
            pbStart.Location = new Point(423, 150);
            pbStart.SizeMode = PictureBoxSizeMode.StretchImage;
            pbStart.Click += new EventHandler(pbStart_Click);

            btHelp.Image = NeonSprint.Properties.Resources.Help2;
            btHelp.Size = new Size(263, 69);
            btHelp.BackColor = Color.Transparent;
            btHelp.Location = new Point(423, 300);
            btHelp.SizeMode = PictureBoxSizeMode.StretchImage;
            btHelp.Click += new EventHandler(btHelp_Click);

            leaderBoard.Image = NeonSprint.Properties.Resources.LEADERBOARD1;
            leaderBoard.Size = new Size(263, 69);
            leaderBoard.BackColor = Color.Transparent;
            leaderBoard.Location = new Point(423, 450);
            leaderBoard.SizeMode = PictureBoxSizeMode.StretchImage;
            leaderBoard.Click += new EventHandler(leaderBoards);

            
            this.Controls.Add(lblTitle);
            this.Controls.Add(pbStart);
            this.Controls.Add(btHelp);
            this.Controls.Add(leaderBoard);
            this.Controls.Add(mainBG);

            //to start the game
            void pbStart_Click(object sender, EventArgs e)
            {
                MainGame game = new MainGame();
                this.Hide();
                game.ShowDialog();
                this.Close();
            }


            //to help
            void btHelp_Click(object sender, EventArgs e)
            {
                Help helpScreen = new Help();
                this.Hide();
                helpScreen.ShowDialog();
                this.Close();
            }
            //to leaderboards
            void leaderBoards(object sender, EventArgs e)
            {
                Leaderboards leaderboardsForm = new Leaderboards();  

                this.Hide();
                leaderboardsForm.ShowDialog(); 
                this.Close();
            }



        }
    }

}
