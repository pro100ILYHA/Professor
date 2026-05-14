using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CollectWordGame
{
    /// <summary>
    /// Управляет загрузкой, сохранением и редактированием словаря.
    /// Файл словаря имеет секции [Тема] и строки со словами.
    /// </summary>
    public class DictionaryManager
    {
        private const string DictionaryFileName = "dictionary.txt";
        public Dictionary<string, List<string>> WordsByTopic { get; private set; }

        public DictionaryManager()
        {
            WordsByTopic = new Dictionary<string, List<string>>();
            LoadDictionary();
        }

        /// <summary>
        /// Загружает словарь из файла. Если файла нет, создаёт демонстрационный набор.
        /// </summary>
        private void LoadDictionary()
        {
            WordsByTopic = new Dictionary<string, List<string>>();

            if (!File.Exists(DictionaryFileName))
            {
                MessageBox.Show($"Файл словаря '{DictionaryFileName}' не найден.\n" +
                                "Поместите словарь в папку с программой или создайте его через Управление словарём.",
                                "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Словарь останется пустым – игра не запустится, но форма управления словарём будет работать
            }

            string[] lines = File.ReadAllLines(DictionaryFileName);
            string currentTopic = null;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentTopic = trimmed.Substring(1, trimmed.Length - 2);
                    if (!WordsByTopic.ContainsKey(currentTopic))
                        WordsByTopic[currentTopic] = new List<string>();
                }
                else if (currentTopic != null)
                {
                    string word = trimmed.ToLower();
                    if (!WordsByTopic[currentTopic].Contains(word))
                        WordsByTopic[currentTopic].Add(word);
                }
            }

            // Дополнительная проверка: если словарь пустой – показать предупреждение
            if (WordsByTopic.Count == 0 || WordsByTopic.Sum(x => x.Value.Count) < 10)
            {
                MessageBox.Show("В словаре меньше 10 слов. Игра невозможна. Добавьте темы и слова через Управление словарём.",
                                "Пустой словарь", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// Сохраняет словарь в файл.
        /// </summary>
        public void SaveDictionary()
        {
            using (StreamWriter writer = new StreamWriter(DictionaryFileName))
            {
                foreach (var topic in WordsByTopic.Keys.OrderBy(k => k))
                {
                    writer.WriteLine($"[{topic}]");
                    foreach (string word in WordsByTopic[topic].OrderBy(w => w))
                        writer.WriteLine(word);
                }
            }
        }

        public List<string> GetAllTopics() => WordsByTopic.Keys.OrderBy(k => k).ToList();

        public List<string> GetWordsByTopic(string topic)
        {
            if (WordsByTopic.ContainsKey(topic))
                return WordsByTopic[topic].OrderBy(w => w).ToList();
            return new List<string>();
        }

        public void AddWord(string topic, string word)
        {
            word = word.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(word)) return;

            if (!WordsByTopic.ContainsKey(topic))
                WordsByTopic[topic] = new List<string>();

            if (!WordsByTopic[topic].Contains(word))
            {
                WordsByTopic[topic].Add(word);
                SaveDictionary();
            }
        }

        public void RemoveWord(string topic, string word)
        {
            if (WordsByTopic.ContainsKey(topic))
            {
                WordsByTopic[topic].Remove(word.ToLower().Trim());
                if (WordsByTopic[topic].Count == 0)
                    WordsByTopic.Remove(topic);
                SaveDictionary();
            }
        }
    }
}