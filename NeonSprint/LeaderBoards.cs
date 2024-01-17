// Modify the Leaderboards class
using System.Drawing;
using System.Windows.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using ButtonBackToMain;

namespace NeonSprint
{
    internal class Leaderboards : Form
    {



        private readonly LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[5];
        private Label lblLeaderboard = new Label();
        private PictureBox leaderBoardBG = new PictureBox();

        public Leaderboards()

        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            leaderboardEntries = LeaderboardManager.LoadLeaderboard();

            this.Size = new Size(1189, 595);
            this.BackgroundImage = (NeonSprint.Properties.Resources.Leaderboard2);
            this.BackgroundImageLayout = ImageLayout.Stretch;

            leaderBoardBG.Size = new Size(1189, 565);
            leaderBoardBG.Image = NeonSprint.Properties.Resources.Leaderboard2;


            PictureBox btBack = ButtonBackToMain.CreateBackButton.CreateMainMenuButton();
            btBack.Click += new EventHandler(btBack_Click);

            // add the TextBox to the form
            lblLeaderboard.Size = new Size(400, 300);
            lblLeaderboard.Location = new Point((this.Width - lblLeaderboard.Width) / 2, (this.Height - lblLeaderboard.Height) / 2);
            this.Controls.Add(lblLeaderboard);

            
            UpdateLeaderboardDisplay();

            this.Controls.Add(btBack);
            this.Controls.Add(leaderBoardBG);
        }

        public async void AppendToLeaderboard(LeaderboardEntry newEntry)
        {
            // add the new entry to the existing leaderboard
            for (int i = 0; i < leaderboardEntries.Length; i++)
            {
                if (leaderboardEntries[i] == null || newEntry.Score > leaderboardEntries[i].Score)
                {
                    ShiftLeaderboardEntries(i);
                    leaderboardEntries[i] = newEntry;
                    break;
                }
            }


            LeaderboardManager.SaveLeaderboard(leaderboardEntries);

            //  delay to allow time for the changes to take effect
            await Task.Delay(1000); 
            UpdateLeaderboardDisplay();
        }


        private void ShiftLeaderboardEntries(int startIndex)
        {
            for (int i = leaderboardEntries.Length - 1; i > startIndex; i--)
            {
                leaderboardEntries[i] = leaderboardEntries[i - 1];
            }
        }

        // Add this method to display the updated leaderboard
        private void UpdateLeaderboardDisplay()
        {
            // Clear the TextBox before updating
            lblLeaderboard.Text = "Leaderboard:\n";
            lblLeaderboard.BackColor = Color.Transparent;
            this.BackgroundImage = (NeonSprint.Properties.Resources.Leaderboard2);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            lblLeaderboard.Size = new Size(200, 300);
            lblLeaderboard.Font = new Font("Impact", 15);

            lblLeaderboard.Location = new Point(600, 160);

            if (leaderboardEntries != null)
            {
                foreach (var entry in leaderboardEntries)
                {
                    if (entry != null)
                    {
                        lblLeaderboard.Text += $"\n{entry.PlayerName.ToUpper()}: {entry.Score}\n";
                    }
                }
            }
        }

        public void btBack_Click(object sender, EventArgs e)
        {
            ButtonBackToMain.CreateBackButton.GoBack(this, () => new HomeScreen());
        }
    }
}
