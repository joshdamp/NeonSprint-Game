using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NeonSprint
{
    // QuizForm represents a form for a quiz game
    public class QuizForm : Form
    {
        private PictureBox questionPictureBox;
        private List<Button> choiceButtons;
        private Button submitButton;
        private List<Question> questionPool;
        private Random random;
        private PictureBox bgTest;
        private bool answeredCorrectly = false;
        public int score = 0;
        private Timer closeTimer;

        public QuizForm()
        {
            // Form settings
            MaximizeBox = false;
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            DoubleBuffered = true;
            random = new Random();
            InitializeQuestionPool();
            InitializeComponents();
            LoadRandomQuestion();
            StartPosition = FormStartPosition.CenterScreen;

            // add a label to display the timer
            Label timerLabel = new Label
            {
                BackColor = Color.Black,
                Location = new System.Drawing.Point(this.Width - 200, 10),
                Size = new System.Drawing.Size(150, 40),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Red,
                Visible = true
            };
            Controls.Add(timerLabel);
            timerLabel.BringToFront();

            // set up a timer to close the form after 10 seconds
            int remainingTime = 15;
            closeTimer = new Timer();
            closeTimer.Interval = 1000;

            // update the timer label text
            closeTimer.Tick += (s, args) =>
            {
                remainingTime--;

                if (remainingTime >= 0)
                {
                    timerLabel.Text = $"Time: {remainingTime} s";
                }
                else
                {
                    closeTimer.Stop();
                    score -= 500;
                    MessageBox.Show("Time's up! The game will continue.\n-500 Points", "Game Continuing,", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                }
            };

            // Start the timer immediately
            closeTimer.Start();
        }

        // initialize the question pool
        private void InitializeQuestionPool()
        {
            questionPool = new List<Question>
            {
                new Question(@"pictures\question1.gif", "Choice 1"),
                new Question(@"pictures\question2.gif", "Choice 2"),
                new Question(@"pictures\question3.gif", "Choice 2"),
                new Question(@"pictures\question4.gif", "Choice 1"),
                new Question(@"pictures\question5.gif", "Choice 2"),
            };
        }

        // Load a random question from the pool
        private void LoadRandomQuestion()
        {
            //randomly select a question from the pool
            Question randomQuestion = questionPool[random.Next(questionPool.Count)];
            questionPictureBox.Image = System.Drawing.Image.FromFile(randomQuestion.QuestionImage);
            List<string> choices = new List<string>(randomQuestion.Choices);
            choiceButtons[0].Text = choices[0];
            choiceButtons[0].Tag = choices[0] == randomQuestion.CorrectAnswer;

            choiceButtons[1].Text = choices[1];
            choiceButtons[1].Tag = choices[1] == randomQuestion.CorrectAnswer;

            // Shuffle the remaining choices
            ShuffleChoices(choices.GetRange(2, choices.Count - 2));

            for (int i = 2; i < choiceButtons.Count; i++)
            {
                choiceButtons[i].Text = choices[i];
                choiceButtons[i].Tag = choices[i] == randomQuestion.CorrectAnswer;
            }
        }

        // Shuffle a list of choices
        private void ShuffleChoices(List<string> choices)
        {
            int n = choices.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = choices[k];
                choices[k] = choices[n];
                choices[n] = value;
            }
        }

        // Initialize form components
        private void InitializeComponents()
        {
            Text = "QuizGame";
            Width = 1500;
            Height = 720;
            BackgroundImage = System.Drawing.Image.FromFile(@"pictures\quizbg.gif");
            BackgroundImageLayout = ImageLayout.Stretch;
            questionPictureBox = new PictureBox
            {
                Location = new System.Drawing.Point(250, 30),
                Size = new System.Drawing.Size(1000, 500),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Controls.Add(questionPictureBox);

            // Choice Buttons
            choiceButtons = new List<Button>();
            for (int i = 0; i < 2; i++)
            {
                Button choiceButton = new Button
                {
                    Location = new System.Drawing.Point(400 + i % 2 * 470, 540 + i / 2 * 80),
                    Size = new System.Drawing.Size(170, 70),
                };
                choiceButton.Click += ChoiceButton_Click;
                choiceButtons.Add(choiceButton);
                Controls.Add(choiceButton);
            }

            // Submit Button
            submitButton = new Button
            {
                Text = "Submit",
                Location = new System.Drawing.Point(645, 600),
                Size = new System.Drawing.Size(150, 40),
                DialogResult = DialogResult.OK,
            };
            submitButton.Click += SubmitButton_Click;
            Controls.Add(submitButton);
            bgTest = new PictureBox
            {
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(1500, 720),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = System.Drawing.Image.FromFile(@"pictures\quizBg1.gif"),
            };
            Controls.Add(bgTest);
            bgTest.SendToBack();
        }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // check if a choice has been selected
            bool choiceSelected = false;
            foreach (Button choiceButton in choiceButtons)
            {
                if (choiceButton.Tag is bool isCorrect && isCorrect)
                {
                    choiceSelected = true;
                    break;
                }
            }

            if (!choiceSelected)
            {
                MessageBox.Show("Please select an answer before submitting.", "No Answer Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // stop timer
            closeTimer.Stop();

            // check the selected choice
            foreach (Button choiceButton in choiceButtons)
            {
                choiceButton.Enabled = false;
                if (choiceButton.Tag is bool isCorrect && isCorrect)
                {
                    if (choiceButton.BackColor == SystemColors.Control)
                    {
                        answeredCorrectly = true;
                        score += 200;

                        MessageBox.Show("Correct! You have gained 200 points!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    choiceButton.BackColor = SystemColors.Control;
                }
            }

            if (!answeredCorrectly)
            {
                score -= 500;

                MessageBox.Show($"Incorrect. You have lost 500 points.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // reset the answeredCorrectly 
            answeredCorrectly = false;
            LoadRandomQuestion();
        }

        private void ChoiceButton_Click(object sender, EventArgs e)
        {
            Button choiceButton = (Button)sender;
            foreach (Button button in choiceButtons)
            {
                button.BackColor = button == choiceButton ? Color.LightBlue : SystemColors.Control;
            }
        }
    }

    // question class to represent a quiz question
    public class Question
    {
        public string QuestionImage { get; }
        public string CorrectAnswer { get; }
        public List<string> Choices { get; }
        public Question(string questionImage, string correctAnswer)
        {
            QuestionImage = questionImage;
            CorrectAnswer = correctAnswer;
            Choices = new List<string> { "Choice 1", "Choice 2" };
        }
    }
}
