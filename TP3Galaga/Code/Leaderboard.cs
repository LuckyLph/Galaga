using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3Galaga
{
    public class Leaderboard
    {
        //Contenu du leaderboard en liste
        List<string> leaderboardContent = new List<string>();
        //Tableau des plus hauts scores
        string[] highestScores = new string[Constants.MAX_LEADERBOARD_ENTRY_SHOWN];
        //Tableau des noms associés au plus haut scores
        string[] highestScoresNames = new string[Constants.MAX_LEADERBOARD_ENTRY_SHOWN];

        public Leaderboard()
        {
            for (int i = 0; i < highestScores.Length; i++)
            {
                highestScores[i] = "0";
            }
        }

        public string GetLeaderboard()
        {
            leaderboardContent.Clear();
            for (int i = 0; i < highestScores.Length; i++)
            {
                highestScores[i] = "0";
            }
            string valueToReturn = "";
            LoadLeaderboard();

            for (int i = 0; i < leaderboardContent.Count; i++)
            {
                string[] lineContent = leaderboardContent[i].Split('=');
                for (int j = 0; j < highestScores.Length; j++)
                {
                    if (Int32.Parse(lineContent[0]) > Int32.Parse(highestScores[j]))
                    {
                        for (int y = highestScores.Length - 2; y > j - 1; y--)
                        {
                            highestScores[y + 1] = highestScores[y];
                            highestScoresNames[y + 1] = highestScoresNames[y];
                        }
                        highestScores[j] = lineContent[0];
                        highestScoresNames[j] = lineContent[1];
                        break;
                    }
                }
            }

            for (int i = 0; i < highestScores.Length; i++)
            {
                if (Int32.Parse(highestScores[i]) != 0)
                    valueToReturn += highestScores[i].ToString() + " " + highestScoresNames[i] + "\n";
            }
            return valueToReturn;
        }

        public void AddEntryToLeaderboard(int score, string playerName)
        {
            int lowestScore = 0;
            int lowestScoreIndex = 0;
            leaderboardContent.Clear();
            LoadLeaderboard();
            if (leaderboardContent.Count < 10)
            {
                leaderboardContent.Add(score.ToString() + "=" + playerName);
            }
            else
            {
                for (int i = 0; i < leaderboardContent.Count; i++)
                {
                    string[] lineContent = leaderboardContent[i].Split('=');
                    if (Int32.Parse(lineContent[0]) < lowestScore)
                    {
                        lowestScoreIndex = i;
                        lowestScore = Int32.Parse(lineContent[0]);
                    }
                }
                leaderboardContent[lowestScoreIndex] = score.ToString() + "=" + playerName;
            }
            System.IO.File.WriteAllLines(Constants.LEADERBOARD_FILE_PATH, leaderboardContent);
        }

        public void LoadLeaderboard()
        {
            if (System.IO.File.Exists(Constants.LEADERBOARD_FILE_PATH))
            {
                string[] tabLeaderboard = System.IO.File.ReadAllLines(Constants.LEADERBOARD_FILE_PATH);
                ErrorCode leaderboardCondition = Parse(tabLeaderboard);
                if (leaderboardCondition == ErrorCode.BAD_FILE_FORMAT)
                {
                    System.Windows.Forms.MessageBox.Show("Le format du fichier est incorrect !", "Erreur de chargement du leaderboard",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (leaderboardCondition == ErrorCode.MISSING_FIELD)
                {
                    System.Windows.Forms.MessageBox.Show("Il y a un champ manquant !", "Erreur de chargement du leaderboard",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (leaderboardCondition == ErrorCode.UNKNOWN_ERROR)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur inconnue !", "Erreur de chargement du leaderboard",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Le fichier n'existe pas !", "Erreur de chargement du leaderboard",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
        }

        public ErrorCode Parse(string[] tabLeaderboard)
        {
            try
            {
                for (int i = 0; i < tabLeaderboard.Length; i++)
                {
                    for (int j = 0; j < tabLeaderboard[i].Length; j++)
                    {
                        if (tabLeaderboard[i][j] != ' ')
                        {
                            tabLeaderboard[i] = tabLeaderboard[i].Trim();
                            leaderboardContent.Add(tabLeaderboard[i]);
                            break;
                        }
                    }
                }
                return ErrorCode.OK;
            }
            catch (FormatException)
            {
                return ErrorCode.BAD_FILE_FORMAT;
            }
            catch (IndexOutOfRangeException)
            {
                return ErrorCode.MISSING_FIELD;
            }
            catch (Exception)
            {
                return ErrorCode.UNKNOWN_ERROR;
            }
        }
    }
}
