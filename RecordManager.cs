using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectWordGame
{
    /// <summary>
    /// Хранит и загружает/сохраняет рекорды. Каждая запись: имя, очки, дата.
    /// </summary>
    public class RecordManager
    {
        private const string RecordsFileName = "records.txt";
        public List<RecordEntry> Records { get; private set; }

        public RecordManager()
        {
            Records = new List<RecordEntry>();
            LoadRecords();
        }

        private void LoadRecords()
        {
            if (!File.Exists(RecordsFileName)) return;

            string[] lines = File.ReadAllLines(RecordsFileName);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3 && int.TryParse(parts[1], out int score) &&
                    DateTime.TryParse(parts[2], out DateTime date))
                {
                    Records.Add(new RecordEntry { Name = parts[0], Score = score, Date = date });
                }
            }
            Records = Records.OrderByDescending(r => r.Score).Take(10).ToList();
        }

        public void SaveRecords()
        {
            Records = Records.OrderByDescending(r => r.Score).Take(10).ToList();
            using (StreamWriter writer = new StreamWriter(RecordsFileName))
            {
                foreach (var record in Records)
                    writer.WriteLine($"{record.Name}|{record.Score}|{record.Date:yyyy-MM-dd HH:mm}");
            }
        }

        public bool IsNewRecord(int score)
        {
            return Records.Count < 10 || score > Records.Min(r => r.Score);
        }

        public void AddRecord(string name, int score)
        {
            Records.Add(new RecordEntry { Name = name, Score = score, Date = DateTime.Now });
            Records = Records.OrderByDescending(r => r.Score).Take(10).ToList();
            SaveRecords();
        }

        public List<RecordEntry> GetTopRecords() => Records;
    }

    public class RecordEntry
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}