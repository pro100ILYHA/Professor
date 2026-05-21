using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WordGuessGame.Classes
{
    [Serializable]
    public class GameSettings
    {
        public string Theme { get; set; } = "Светлая";
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
        public int BestScore { get; set; } = 0;
        public string SelectedTheme { get; set; } = "Все темы";
    }

    public enum Difficulty { Easy, Medium, Hard }

    public static class SettingsManager
    {
        private static string settingsPath = "Data/settings.xml";
        private static string bestScorePath = "Data/bestscore.dat";

        public static GameSettings LoadSettings()
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
                    using (FileStream fs = new FileStream(settingsPath, FileMode.Open))
                    {
                        return (GameSettings)serializer.Deserialize(fs);
                    }
                }
            }
            catch { }
            return new GameSettings();
        }

        public static void SaveSettings(GameSettings settings)
        {
            try
            {
                string directory = Path.GetDirectoryName(settingsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
                using (FileStream fs = new FileStream(settingsPath, FileMode.Create))
                {
                    serializer.Serialize(fs, settings);
                }
            }
            catch { }
        }

        public static int LoadBestScore()
        {
            try
            {
                if (File.Exists(bestScorePath))
                {
                    string content = File.ReadAllText(bestScorePath);
                    if (int.TryParse(content, out int score))
                        return score;
                }
            }
            catch { }
            return 0;
        }

        public static void SaveBestScore(int score)
        {
            try
            {
                string directory = Path.GetDirectoryName(bestScorePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                File.WriteAllText(bestScorePath, score.ToString());
            }
            catch { }
        }

        public static void ApplyTheme(Form form, GameSettings settings)
        {
            if (settings.Theme == "Тёмная")
            {
                form.BackColor = Color.FromArgb(45, 45, 48);
                foreach (Control ctrl in form.Controls)
                {
                    if (ctrl is Button)
                    {
                        Button btn = (Button)ctrl;
                        btn.BackColor = Color.FromArgb(62, 62, 66);
                        btn.ForeColor = Color.White;
                        btn.FlatStyle = FlatStyle.Flat;
                    }
                    else if (ctrl is Label)
                    {
                        Label lbl = (Label)ctrl;
                        lbl.ForeColor = Color.White;
                    }
                    else if (ctrl is TextBox)
                    {
                        TextBox txt = (TextBox)ctrl;
                        txt.BackColor = Color.FromArgb(30, 30, 30);
                        txt.ForeColor = Color.White;
                    }
                    else if (ctrl is ListBox)
                    {
                        ListBox list = (ListBox)ctrl;
                        list.BackColor = Color.FromArgb(30, 30, 30);
                        list.ForeColor = Color.White;
                    }
                    else if (ctrl is ComboBox)
                    {
                        ComboBox cb = (ComboBox)ctrl;
                        cb.BackColor = Color.FromArgb(62, 62, 66);
                        cb.ForeColor = Color.White;
                    }
                    else if (ctrl is TabControl)
                    {
                        TabControl tabs = (TabControl)ctrl;
                        tabs.BackColor = Color.FromArgb(45, 45, 48);
                        foreach (TabPage page in tabs.TabPages)
                        {
                            page.BackColor = Color.FromArgb(45, 45, 48);
                            page.ForeColor = Color.White;
                        }
                    }
                }
            }
            else
            {
                form.BackColor = SystemColors.Control;
                foreach (Control ctrl in form.Controls)
                {
                    if (ctrl is Button)
                    {
                        Button btn = (Button)ctrl;
                        btn.BackColor = SystemColors.Control;
                        btn.ForeColor = SystemColors.ControlText;
                        btn.FlatStyle = FlatStyle.Standard;
                    }
                    else if (ctrl is Label)
                    {
                        Label lbl = (Label)ctrl;
                        lbl.ForeColor = SystemColors.ControlText;
                    }
                    else if (ctrl is TextBox)
                    {
                        TextBox txt = (TextBox)ctrl;
                        txt.BackColor = Color.White;
                        txt.ForeColor = SystemColors.ControlText;
                    }
                    else if (ctrl is ListBox)
                    {
                        ListBox list = (ListBox)ctrl;
                        list.BackColor = Color.White;
                        list.ForeColor = SystemColors.ControlText;
                    }
                    else if (ctrl is ComboBox)
                    {
                        ComboBox cb = (ComboBox)ctrl;
                        cb.BackColor = Color.White;
                        cb.ForeColor = SystemColors.ControlText;
                    }
                    else if (ctrl is TabControl)
                    {
                        TabControl tabs = (TabControl)ctrl;
                        tabs.BackColor = SystemColors.Control;
                        foreach (TabPage page in tabs.TabPages)
                        {
                            page.BackColor = SystemColors.Control;
                            page.ForeColor = SystemColors.ControlText;
                        }
                    }
                }
            }
        }

        public static void GetWordLengthByDifficulty(Difficulty difficulty, out int minLength, out int maxLength)
        {
            if (difficulty == Difficulty.Easy)
            {
                minLength = 3;
                maxLength = 5;
            }
            else if (difficulty == Difficulty.Medium)
            {
                minLength = 4;
                maxLength = 7;
            }
            else
            {
                minLength = 6;
                maxLength = 10;
            }
        }
    }
}