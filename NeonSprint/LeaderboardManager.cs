using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NeonSprint
{
    public class LeaderboardEntry
    {
        public string PlayerName { get; set; }
        public double Score { get; set; }
    }

    public static class LeaderboardManager
    {
        private static readonly string LeaderboardFilePath = Path.Combine(Environment.CurrentDirectory, "leaderboard.txt");

        private static LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[5];

        public static LeaderboardEntry[] LoadLeaderboard()
        {
            LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[5];

            if (File.Exists(LeaderboardFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(LeaderboardFilePath);

                    for (int i = 0; i < Math.Min(lines.Length, 5); i++)
                    {
                        string[] parts = lines[i].Split(',');
                        if (parts.Length == 2 && double.TryParse(parts[1], out double score))
                        {
                            leaderboardEntries[i] = new LeaderboardEntry
                            {
                                PlayerName = parts[0],
                                Score = score
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading leaderboard file: {ex.Message}");
                }
            }

            return leaderboardEntries;
        }

        public static void SaveLeaderboard(LeaderboardEntry[] leaderboardEntries)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(LeaderboardFilePath, false))
                {
                    for (int i = 0; i < Math.Min(leaderboardEntries.Length, 5); i++)
                    {
                        if (leaderboardEntries[i] != null)
                        {
                            writer.WriteLine($"{leaderboardEntries[i].PlayerName},{leaderboardEntries[i].Score}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving leaderboard file: {ex.Message}");
            }
        }
    }
}
