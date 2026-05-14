using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WordGuessGame.Classes
{
    public class WordEntry
    {
        public string Word { get; set; }
        public string Theme { get; set; }

        public WordEntry(string word, string theme)
        {
            Word = word.ToLower();
            Theme = theme;
        }

        public override string ToString() => $"{Word} ({Theme})";
    }

    public class WordDictionary
    {
        private List<WordEntry> words;
        private string filePath;

        public WordDictionary(string filePath = "Data/words.txt")
        {
            this.filePath = filePath;
            words = new List<WordEntry>();
        }

        public List<WordEntry> GetAllWords() => new List<WordEntry>(words);

        public List<string> GetAllThemes() => words.Select(w => w.Theme).Distinct().OrderBy(t => t).ToList();

        /// <summary>Загружает словарь из файла формата "слово|тема"</summary>
        public bool LoadDictionary()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл словаря не найден: {filePath}\nФормат: слово|тема", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    words = new List<WordEntry>();
                    return false;
                }

                var lines = File.ReadAllLines(filePath);
                var loadedWords = new List<WordEntry>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length >= 2)
                    {
                        string word = parts[0].Trim().ToLower();
                        string theme = parts[1].Trim();
                        if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(theme))
                            loadedWords.Add(new WordEntry(word, theme));
                    }
                }

                if (loadedWords.Count == 0)
                {
                    MessageBox.Show("Словарь пуст или имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    words = new List<WordEntry>();
                    return false;
                }

                words = loadedWords;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                words = new List<WordEntry>();
                return false;
            }
        }

        public void SaveDictionary()
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                File.WriteAllLines(filePath, words.Select(w => $"{w.Word}|{w.Theme}"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool AddWord(string word, string theme)
        {
            word = word.Trim().ToLower();
            theme = theme.Trim();
            if (string.IsNullOrEmpty(word) || word.Length > 30 || string.IsNullOrEmpty(theme)) return false;
            if (words.Any(w => w.Word == word)) return false;
            words.Add(new WordEntry(word, theme));
            return true;
        }

        public bool RemoveWord(string word)
        {
            word = word.Trim().ToLower();
            var entry = words.FirstOrDefault(w => w.Word == word);
            if (entry == null) return false;
            words.Remove(entry);
            return true;
        }

        public bool ContainsWord(string word) => words.Any(w => w.Word == word.Trim().ToLower());

        public string GetThemeOfWord(string word)
        {
            var entry = words.FirstOrDefault(w => w.Word == word.Trim().ToLower());
            return entry?.Theme;
        }

        public List<WordEntry> GetWordsStartingWith(char letter) =>
            words.Where(w => w.Word.StartsWith(letter.ToString())).OrderBy(w => w.Word).ToList();

        public WordEntry GetRandomWord(int minLength = 3, int maxLength = 10)
        {
            var filtered = words.Where(w => w.Word.Length >= minLength && w.Word.Length <= maxLength).ToList();
            if (filtered.Count == 0) filtered = words;
            if (filtered.Count == 0) return null;
            Random rand = new Random();
            return filtered[rand.Next(filtered.Count)];
        }

        public WordEntry GetRandomWordByTheme(string theme, int minLength = 3, int maxLength = 10)
        {
            var filtered = words.Where(w => w.Theme == theme && w.Word.Length >= minLength && w.Word.Length <= maxLength).ToList();
            if (filtered.Count == 0) return null;
            Random rand = new Random();
            return filtered[rand.Next(filtered.Count)];
        }

        public int Count => words.Count;
        public bool IsLoaded => words != null && words.Count > 0;
    }
}