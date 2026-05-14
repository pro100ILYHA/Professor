using System;
using System.IO;

namespace CollectWordGame
{
    /// <summary>
    /// Управляет сохранением и загрузкой настроек (имя игрока, длительность таймера).
    /// </summary>
    public class SettingsManager
    {
        private const string SettingsFileName = "settings.ini";
        public string PlayerName { get; set; } = "Игрок";
        public int TimerSeconds { get; set; } = 30; // 20, 30, 60, 90
        public int[] AllowedTimes { get; } = { 20, 30, 60, 90 };

        public SettingsManager()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(SettingsFileName)) return;
            string[] lines = File.ReadAllLines(SettingsFileName);
            foreach (string line in lines)
            {
                if (line.StartsWith("Name=")) PlayerName = line.Split('=')[1].Trim();
                else if (line.StartsWith("Time=") && int.TryParse(line.Split('=')[1], out int t))
                    TimerSeconds = t;
            }
        }

        public void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter(SettingsFileName))
            {
                writer.WriteLine($"Name={PlayerName}");
                writer.WriteLine($"Time={TimerSeconds}");
            }
        }

        /// <summary>
        /// Возвращает базовые очки за раунд без учёта скорости и подсказок.
        /// В ТЗ: 100 за 20с, 80 за 30с, 60 за 60с, 50 за 90с.
        /// </summary>
        public int GetBasePoints()
        {
            switch (TimerSeconds)
            {
                case 20: return 100;
                case 30: return 80;
                case 60: return 60;
                case 90: return 50;
                default: return 80;
            }
        }
    }
}