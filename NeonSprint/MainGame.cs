using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ButtonBackToMain;
using static NeonSprint.Leaderboards;
using WMPLib;

namespace NeonSprint
{

    public class MainGame : Form
    {

        private List<Obstacle> obstaclesList = new List<Obstacle>();
        private List<Obstacle> secondObstaclesList = new List<Obstacle>();
        private List<NeonOrbs> neonOrbsList = new List<NeonOrbs>();
        private LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[5];

        private Player player;

        PictureBox mainBG = new PictureBox();


        private System.Media.SoundPlayer jumpSoundPlayer;
        private System.Media.SoundPlayer slideSoundPlayer;



        private Obstacle obstacle;
        private Obstacle spike;
        private Obstacle secondObstacle;


        private Label lblScore;
        private Label lblInvincibility;
        private Label lblMultiplier;
        private Label lblAddlife;

        private NeonOrbs InvincibilityOrb;
        private NeonOrbs orbMultiplier;
        private NeonOrbs lifeOrb;
        private NeonOrbs randomOrb;

        private Portal portal;
        private int gravity;
        private readonly int gravityValue = 20;
        private readonly int platformLevel = 385;
        private double obstacleSpeed;
        private int obstacleDistance;
        private double multiplier;
        private bool hasLifeOrb;
        public double score;

        private bool quizTriggered;
        private bool gameOver;

        private readonly Timer invincibilityTimer = new Timer();
        private readonly Timer multiplierTimer = new Timer();
        private readonly Timer slideTimer = new Timer();
        private readonly Timer lifeTimer = new Timer();
        private readonly Timer deathTimer = new Timer();

        private bool isInvincible;
        private bool isSliding;
        private bool gameIsPaused;
        private bool portalIsAdded;

        private PictureBox btBack; // Declare btBack at the class level


        private readonly Random random = new Random();
        private readonly Timer gameTimerEvent = new Timer();


        public MainGame()
        {
            DoubleBuffered = true;
            InitializeForm();
            InitializeComponents();
            RestartGame();
        }

        private System.Media.SoundPlayer bgMusicPlayer;
        private WindowsMediaPlayer form_bg = new WindowsMediaPlayer();

        private void InitializeMediaPlayer()
        {
            form_bg = new WindowsMediaPlayer();

            // Set properties for the WindowsMediaPlayer control
            form_bg.URL = @"pictures\bgmusicgame.wav";
            form_bg.settings.volume = 50; // Set the volume (0 to 100)
            form_bg.settings.setMode("loop", true); // Enable looping
            

            // Add the WindowsMediaPlayer control to your form

        }
        private void InitializeForm()
        {
            Text = "NEON SPRINT";
            Size = new Size(1189, 565);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundImage = NeonSprint.Properties.Resources.gamewallpaper;
            BackgroundImageLayout = ImageLayout.Stretch;
            MaximizeBox = false;
            //ControlBox = false;

            mainBG.Size =new Size(1189, 235);
            mainBG.Location =new Point(-7,0);
            mainBG.Image = NeonSprint.Properties.Resources.gamewallpaper;
            mainBG.BackgroundImageLayout = ImageLayout.Stretch; // Optional: Stretch the image to fill the PictureBox

            bgMusicPlayer = new System.Media.SoundPlayer();
            bgMusicPlayer.SoundLocation = @"pictures\bgmusicgame.wav"; // Replace with the actual path to your background music file
            bgMusicPlayer.Load(); // Load the music

            // Play the background music on loop
            bgMusicPlayer.PlayLooping();

            InitializeMediaPlayer();


        }

        public void InitializeComponents()
        {

            PictureBox btBack = ButtonBackToMain.CreateBackButton.CreateMainMenuButton();
            btBack.Click += new EventHandler(btBack_Click);
            btBack.Location = new Point(980,-40);
            Controls.Add(btBack);

            lblScore = new ScoreLabel();
            Controls.Add(lblScore);

            var tile = new Tile(new Point(0, 480), NeonSprint.Properties.Resources.platform_tilesblue);
            Controls.Add(tile);

            player = new Player(new Point(200, platformLevel), NeonSprint.Properties.Resources.Run);
            Controls.Add(player);

            gameTimerEvent.Interval = 20;
            //gameTimerEvent.Enabled = true;
            gameTimerEvent.Tick += new EventHandler(GameTimerEvent);

            slideTimer.Interval = 750;
            slideTimer.Tick += new EventHandler(EndSlideTimerTick);

            lblInvincibility = new InvinciLabel();
            Controls.Add(lblInvincibility);

            lblMultiplier = new MultiplierLabel();
            Controls.Add(lblMultiplier);

            lblAddlife = new LifeLabel();
            Controls.Add(lblAddlife);

            invincibilityTimer.Interval = 3000; // 3 seconds
            invincibilityTimer.Tick += new EventHandler(EndInvincibility);

            multiplierTimer.Interval = 5000; // 5 seconds
            multiplierTimer.Tick += new EventHandler(EndMultiplier);

            lifeTimer.Interval = 2000; // 5 seconds
            lifeTimer.Tick += new EventHandler(endLifeTimer);

            deathTimer.Interval = 3500;
            deathTimer.Tick += new EventHandler(EndDeathTimer);

            // Initialize the SoundPlayer for the jump sound effect
            jumpSoundPlayer = new System.Media.SoundPlayer(@"pictures\jump1.wav");
            jumpSoundPlayer.Load();

            // Initialize the SoundPlayer for the slide sound effect
            slideSoundPlayer = new System.Media.SoundPlayer(@"pictures\slide.wav");
            slideSoundPlayer.Load();



            btBack = ButtonBackToMain.CreateBackButton.CreateMainMenuButton();
            btBack.Click += new EventHandler(btBack_Click);


            KeyPreview = true;
            KeyUp += Form_KeyUp;
        }

        private void RestartGame()
        {

            obstacleDistance = 1000;
            score = 0;
            portalIsAdded = false;
            obstacleSpeed = 10;
            multiplier = 1;

            gameIsPaused = false;
            quizTriggered = false;
            gameOver = false;
            hasLifeOrb = false; // Player starts without a life orb
            lblAddlife.Text = "";
            lblMultiplier.Text = "";
            lblInvincibility.Text = "";
            isInvincible = false;
            isSliding = false;


            player.Location = new Point(200, platformLevel);
            player.Image = NeonSprint.Properties.Resources.Run;

            if (obstacle != null)
            {
                RemoveControls();

            }
            obstaclesList.Clear();
            obstacle = new Obstacle(new Point(obstacleDistance, 380), new Size(53, 93), NeonSprint.Properties.Resources.box1);
            Controls.Add(obstacle);
            obstacle.BringToFront();
            obstaclesList.Add(obstacle);

            spike = new Obstacle(new Point(obstacleDistance + 1000, 380), new Size(53, 93), NeonSprint.Properties.Resources.spike);
            Controls.Add(spike);
            spike.BringToFront();
            obstaclesList.Add(spike);




            secondObstaclesList.Clear();
            secondObstacle = new Obstacle(new Point(obstacleDistance + 500, 310), new Size(100, 100), NeonSprint.Properties.Resources.obstaclefloat);
            Controls.Add(secondObstacle);
            secondObstaclesList.Add(secondObstacle);





            neonOrbsList.Clear();

            InvincibilityOrb = new NeonOrbs(new Point(2000, 420), NeonSprint.Properties.Resources.orb, OrbType.Invincibility);
            Controls.Add(InvincibilityOrb);
            neonOrbsList.Add(InvincibilityOrb);

            orbMultiplier = new NeonOrbs(new Point(4000, 420), NeonSprint.Properties.Resources.orb2x, OrbType.Multiplier);
            Controls.Add(orbMultiplier);
            neonOrbsList.Add(orbMultiplier);

            lifeOrb = new NeonOrbs(new Point(5000, 420), NeonSprint.Properties.Resources.lifeOrb, OrbType.Addlife);
            Controls.Add(lifeOrb);
            neonOrbsList.Add(lifeOrb);

            randomOrb = new NeonOrbs(new Point(3200, 420), NeonSprint.Properties.Resources.randomOrb, OrbType.RandomOrb);
            Controls.Add(randomOrb);
            neonOrbsList.Add(randomOrb);



            if (portal != null)
            {
                Controls.Remove(portal);
                portal.Dispose();
            }

           

            Controls.Add(mainBG);
            gameTimerEvent.Start();


        }

        private void RemoveControls()
        {
            Controls.Remove(obstacle);
            obstacle.Dispose();
            Controls.Remove(spike);
            spike.Dispose();

            Controls.Remove(InvincibilityOrb);
            InvincibilityOrb.Dispose();


            Controls.Remove(orbMultiplier);
            orbMultiplier.Dispose();

            Controls.Remove(lifeOrb);
            lifeOrb.Dispose();

            Controls.Remove(randomOrb);
            randomOrb.Dispose();

            Controls.Remove(secondObstacle);
            secondObstacle.Dispose();
        }
        private void MoveObstacle(Obstacle x)
        {
            x.Left -= (int)obstacleSpeed;

            if (x.Left < -100)
            {
                x.Left = obstacleDistance + 500;
                //score++;
            }


        }

        private void EndGame()
        {
            
            gameTimerEvent.Stop();
            ActivateDeathTimer();
            gameOver = true;

            PromptPlayerNameAndShowLeaderboards();
        }

        //--------------------------------------------------------------------------------------------------------------------------------  LEADER BOARD SECTION
        private void PromptPlayerNameAndShowLeaderboards()
        {
            // Create a form to prompt for the player's name

            Form prompt = new Form()
            {
                Width = 400,
                Height = 190,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackgroundImage = Properties.Resources.bgLB,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label enterN = new Label()
            {
                Text = "Enter Name: ",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Left = 20,
                Width = 110,
                Top = 32,
                Font = new Font("Arial", 11) // Specify your font name and size here
            };
            TextBox textBox = new TextBox()
            {
                Left = 120,
                Top = 30,
                Width = 175,
                BackColor = Color.LightGray
            };
            Button confirmation = new Button()
            {
                Size = new Size(80, 30),
                Text = "Leaderboards",
                Font = new Font("Arial", 8),
                BackColor = Color.DimGray,
                Left = 150,
                Width = 85,
                Top = 80,
                DialogResult = DialogResult.OK
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(enterN);
            prompt.AcceptButton = confirmation;


            if (prompt.ShowDialog() == DialogResult.OK)
            {
                string playerName = textBox.Text;

                LeaderboardEntry entry = new LeaderboardEntry
                {
                    PlayerName = playerName,
                    Score = score
                };

                leaderboardEntries[4] = entry;

                Array.Sort(leaderboardEntries, (x, y) =>
                {
                    if (x == null && y == null)
                        return 0;
                    if (x == null)
                        return 1;
                    if (y == null)
                        return -1;

                    return y.Score.CompareTo(x.Score);
                });

                DisplayLeaderboard();

                Leaderboards leaderboardsForm = new Leaderboards();
                leaderboardsForm.Show(); // Use Show instead of ShowDialog to allow the main form to remain open
                leaderboardsForm.AppendToLeaderboard(entry);

                PictureBox btBack = ButtonBackToMain.CreateBackButton.CreateMainMenuButton();
                btBack.Click += new EventHandler(btBack_Click);
                leaderboardsForm.Controls.Add(btBack);
                btBack.BringToFront();
            }
        }

        private void GoLboards_Click(object sender, EventArgs e)
        {
            Leaderboards leaderboardsForm = new Leaderboards();  // Create an instance of the Leaderboards class
            this.Hide();
            leaderboardsForm.ShowDialog();  // Call ShowDialog on the instance, not on the class
            this.Close();
        }
        public void btBack_Click(object sender, EventArgs e)
        {
            gameTimerEvent.Stop();
            ButtonBackToMain.CreateBackButton.GoBack(this, () => new HomeScreen());
        }
        private void DisplayLeaderboard()
        {
            StringBuilder leaderboardText = new StringBuilder();
            leaderboardText.AppendLine("Leaderboard:");

            if (leaderboardEntries != null)
            {
                foreach (var entry in leaderboardEntries)
                {
                    if (entry != null)
                    {
                        leaderboardText.Append($"\n{entry.PlayerName}: {entry.Score}\n");
                    }
                }
            }

        }
        //--------------------------------------------------------------------------------------------------------------------------------  LEADER BOARD SECTION

        private void AddPortal()
        {
            portalIsAdded = true;
            quizTriggered = false;
            portal = new Portal(new Point(800, 200), NeonSprint.Properties.Resources.portalnew);
            portal.BringToFront();
            Controls.Add(portal);
            if (obstacleSpeed <= 20)
            { obstacleSpeed += 2; }

        }

        public void GameTimerEvent(object sender, EventArgs e)
        {
            lblScore.Text = "Score: " + score;
            score += multiplier;
            if ((score % 1000 == 0) || ((score - 1) % 1000 == 0 && !portalIsAdded && score != 1))
            {
                AddPortal();
            }




            if (portal != null)
            {
                portal.Left -= (int)obstacleSpeed;

                if (portal.Bounds.IntersectsWith(player.Bounds) && portal.Left < player.Left && quizTriggered == false)
                {
                    quizTriggered = true;
                    portalIsAdded = false;
                    PauseGame();

                    QuizForm quiz = new QuizForm();


                    quiz.FormClosed += (s, args) =>
                    {

                        score += quiz.score;
                        ResumeGame();
                        ActivateInvincibility();
                    };

                    quiz.ShowDialog();
                }
            }





            player.Top += gravity;

            if (player.Top >= platformLevel)
            {
                gravity = 0;
                player.Top = platformLevel;
            }
            else if (player.Top < 100 && player.Top != platformLevel)
            {
                if (!isInvincible) { player.Image = NeonSprint.Properties.Resources.fall; }
                if (isInvincible)
                {
                    player.Image = NeonSprint.Properties.Resources.falling;
                }

                gravity = 15;
                slideTimer.Start();
            }

            foreach (Obstacle x in obstaclesList)
            {
                MoveObstacle(x);
                if (x.Bounds.IntersectsWith(player.Bounds) && !isInvincible && !gameOver)
                {
                    if (!hasLifeOrb)
                    {
                        EndGame();
                    }
                    if (hasLifeOrb)
                    {

                        lblAddlife.Text = "";
                        lblAddlife.AutoSize = false;
                        lifeTimer.Start();

                    }
                }

            }

            foreach (Obstacle x in secondObstaclesList)
            {
                MoveObstacle(x);
                if (x.Bounds.IntersectsWith(player.Bounds) && !isInvincible && !isSliding && !gameOver)
                {
                    if (!hasLifeOrb)
                    {
                        EndGame();
                    }
                    if (hasLifeOrb)
                    {

                        lblAddlife.Text = "";
                        lblAddlife.AutoSize = false;
                        lifeTimer.Start();
                    }
                }
            }





            foreach (NeonOrbs orbs in neonOrbsList)
            {
                int[] orbLocs = { 300, 420 };
                int randomIndex = random.Next(0, orbLocs.Length);
                int randomElement = orbLocs[randomIndex];

                orbs.Left -= (int)obstacleSpeed;

                if (orbs.Left < -100)
                {
                    orbs.Left = random.Next(5000, 6000);
                    orbs.Top = randomElement;
                }

                if (orbs.Bounds.IntersectsWith(player.Bounds) && orbs.Left < player.Left)
                {
                    orbs.Left = random.Next(5000, 6000);
                    orbs.Top = randomElement;

                    if (orbs.Type == OrbType.Multiplier)
                    {
                        ActivateScoreMultiplier();
                    }
                    else if (orbs.Type == OrbType.Invincibility)
                    {
                        ActivateInvincibility();
                    }
                    else if (orbs.Type == OrbType.Addlife)
                    {

                        ActivateExtraLife();

                    }
                    else if (orbs.Type == OrbType.RandomOrb)
                    {
                        // Implement logic for the random orb
                        int randomOrbType = random.Next(3); // 0, 1, or 2
                        if (randomOrbType == 0)
                        {
                            ActivateScoreMultiplier();
                        }
                        else if (randomOrbType == 1)
                        {
                            ActivateInvincibility();
                        }
                        else
                        {
                            // Deduct 300 from the score (if the score is greater than 300)
                            if (score > 300)
                            {
                                lblScore.Text = $"Score: {score} -300";
                                lblScore.ForeColor = Color.Red;
                                score -= 300;

                                // Set up a timer to reset the color after a brief delay (e.g., 1000 milliseconds)
                                Timer resetColorTimer = new Timer();
                                resetColorTimer.Interval = 1300;
                                resetColorTimer.Tick += (s, args) =>
                                {
                                    resetColorTimer.Stop();
                                    lblScore.ForeColor = Color.White; // Change to the original color
                                    resetColorTimer.Dispose(); // Dispose the timer to free resources
                                };
                                resetColorTimer.Start();
                            }
                            else
                            {
                                // Set the score to 0 if it's less than or equal to 300
                                score = 0;
                            }
                        }

                    }
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------------- ACTIVATION OF ORBS

        public void ResumeGame()
        {
            gameTimerEvent.Start();
            gameIsPaused = false;

        }

        public void PauseGame()
        {
            gameTimerEvent.Stop();

        }

        private void ActivateInvincibility()
        {
            // Implement invincibility effect here
            isInvincible = true;

            player.Image = NeonSprint.Properties.Resources.playerinvincible;
            invincibilityTimer.Start();

            // Display "Invincible Mode" text
            lblInvincibility.Text = "INVINCIBLE MODE";
        }

        private void ActivateExtraLife()
        {
            // Set the flag indicating that the player has a life orb
            hasLifeOrb = true;
            lblAddlife.Text = "EXTRA LIFE";
            player.Image = NeonSprint.Properties.Resources.orblife;
            lblAddlife.AutoSize = true;

        }

        private void ActivateDeathTimer()
        {
            player.Image = NeonSprint.Properties.Resources.dead1;
            deathTimer.Start();
        }

        private void ActivateScoreMultiplier()
        {

            multiplier = 2;
            multiplierTimer.Start();
            player.Image = NeonSprint.Properties.Resources.orb2x1;
            lblMultiplier.Text = "SCORE MULTIPLIER";



        }

        private void EndMultiplier(object sender, EventArgs e)
        {

            multiplier = 1;
            multiplierTimer.Stop();
            lblMultiplier.Text = "";

        }
        private void endLifeTimer(object sender, EventArgs e)
        {

            hasLifeOrb = false;
            lifeTimer.Stop();


        }



        private void EndInvincibility(object sender, EventArgs e)
        {
            isInvincible = false; // reset the flag

            invincibilityTimer.Stop(); // stop the timer
            player.Image = NeonSprint.Properties.Resources.Run;

            // Remove "Invincible Mode" text
            lblInvincibility.Text = "";
            //neonOrbsList.Remove(InvincibilityOrb);
        }

        private void EndDeathTimer(object sender, EventArgs e)
        {
            player.Image = null;
            deathTimer.Stop();
            deathTimer.Dispose(); // Dispose of the timer
        }



        //---------------------------------------------------------------------------------------------------------------------------------- MOVEMENTS SECTION
        private void EndSlideTimerTick(object sender, EventArgs e)
        {
            // Update the hitbox of the player for 1.5 seconds
            isSliding = false;
            slideTimer.Stop();
            if (!isInvincible) { player.Image = NeonSprint.Properties.Resources.Run; }
            if (isInvincible)
            {
                player.Image = NeonSprint.Properties.Resources.playerinvincible;
            }



        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RestartGame();


            }

            if (!gameOver)
            {
                if (e.KeyCode == Keys.W && !gameIsPaused)
                {
                    if (player.Top == platformLevel)
                    {
                        if (!isInvincible) { player.Image = NeonSprint.Properties.Resources.jump2; }
                        if (isInvincible)
                        {
                            player.Image = NeonSprint.Properties.Resources.jumpinvincible;
                            
                        }
                        jumpSoundPlayer.Play();
                        player.Top -= gravity;
                        gravity = -gravityValue;
                    }
                }

                if (e.KeyCode == Keys.Escape)
                {
                    if (gameIsPaused == false)
                    {
                        // If the game is not paused, pause it
                        // Add back button
                        
                        
                        PauseGame();
                        gameIsPaused = true;

                    }
                    else if (gameIsPaused == true)
                    {
                        // If the game is paused, resume it
                        

                        ResumeGame();
                    }
                }

                else if (e.KeyCode == Keys.S && !gameIsPaused)
                {
                    if (player.Top == platformLevel)
                    {
                        isSliding = true;
                        slideTimer.Start();
                        slideSoundPlayer.Play();
                        if (!isInvincible) { player.Image = NeonSprint.Properties.Resources.slidetest; }
                        if (isInvincible)
                        {

                            player.Image = NeonSprint.Properties.Resources.slideinvincible;
                        }
                    }


                    if (player.Top != platformLevel)
                    {
                        player.Top += 50;

                        gravity = 50;
                        if (!isInvincible)
                        {
                            player.Image = NeonSprint.Properties.Resources.fall;
                        }

                        if (isInvincible)
                        {
                            player.Image = NeonSprint.Properties.Resources.falling;
                        }
                        slideTimer.Start();
                    }
                }
            }
        }
    }

}