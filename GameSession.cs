using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectWordGame
{
    /// <summary>
    /// Управляет одной игровой сессией: выбор тем и слов, генерация анаграмм, подсчёт очков.
    /// </summary>
    public class GameSession
    {
        private DictionaryManager dictionary;
        private SettingsManager settings;
        private Random random;
        private List<string> usedWords;
        private const int TotalRounds = 10;

        public int CurrentRound { get; private set; }
        public int Score { get; private set; }
        public string CurrentTopic { get; private set; }
        public string CurrentWord { get; private set; }
        public string CurrentAnagram { get; private set; }
        public int HintsLeft { get; private set; }       // количество оставшихся подсказок для текущего слова
        public bool HintUsed { get; private set; }       // флаг использования подсказки
        public int TimeLeft { get; set; }                // оставшееся время раунда (для формы)
        public List<RoundResult> RoundResults { get; private set; }

        public GameSession(DictionaryManager dict, SettingsManager sett)
        {
            dictionary = dict;
            settings = sett;
            random = new Random();
            RoundResults = new List<RoundResult>();
        }

        /// <summary>
        /// Начинает новую сессию.
        /// </summary>
        public void StartNewSession()
        {
            usedWords = new List<string>();
            Score = 0;
            RoundResults.Clear();
            CurrentRound = 0;
        }

        /// <summary>
        /// Подготавливает следующий раунд. Возвращает true, если есть ещё раунды.
        /// </summary>
        public bool NextRound()
        {
            if (CurrentRound >= TotalRounds) return false;
            CurrentRound++;

            // Выбор доступной темы (где есть неиспользованные слова)
            List<string> topics = dictionary.GetAllTopics();
            List<string> availableTopics = topics.Where(t => dictionary.GetWordsByTopic(t).Any(w => !usedWords.Contains(w))).ToList();
            if (availableTopics.Count == 0)
                return false; // больше нет слов

            CurrentTopic = availableTopics[random.Next(availableTopics.Count)];
            List<string> words = dictionary.GetWordsByTopic(CurrentTopic).Where(w => !usedWords.Contains(w)).ToList();
            CurrentWord = words[random.Next(words.Count)];
            usedWords.Add(CurrentWord);

            // Генерация анаграммы, гарантированно отличающейся от исходного слова
            do
            {
                CurrentAnagram = new string(CurrentWord.OrderBy(c => random.Next()).ToArray());
            } while (CurrentAnagram == CurrentWord && CurrentWord.Length > 1);

            // Количество подсказок = длина слова / 2 (целочисленное деление)
            HintsLeft = CurrentWord.Length / 2;
            HintUsed = false;
            TimeLeft = settings.TimerSeconds;

            return true;
        }

        /// <summary>
        /// Использовать подсказку (показывает первую букву, снимает часть очков).
        /// Возвращает первую букву или null, если подсказок больше нет.
        /// </summary>
        public char? UseHint()
        {
            if (HintsLeft > 0)
            {
                HintsLeft--;
                HintUsed = true;
                return CurrentWord[0];
            }
            return null;
        }

        /// <summary>
        /// Проверяет ответ пользователя. При правильном ответе начисляет очки.
        /// </summary>
        public bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrEmpty(userAnswer)) return false;
            bool correct = userAnswer.Trim().ToLower() == CurrentWord;
            if (correct)
            {
                int basePoints = settings.GetBasePoints();
                // Масштабирование по времени не реализовано явно, так как время фиксировано на весь раунд.
                // По ТЗ очки зависят от настройки таймера (100/80/60/50), и при подсказке — половина.
                double multiplier = 1.0;
                if (HintUsed) multiplier = 0.5;
                int earned = (int)(basePoints * multiplier);
                Score += earned;

                RoundResults.Add(new RoundResult
                {
                    Round = CurrentRound,
                    Word = CurrentWord,
                    Topic = CurrentTopic,
                    Correct = true,
                    PointsEarned = earned
                });
            }
            return correct;
        }

        /// <summary>
        /// Фиксирует пропуск слова (по таймеру или кнопке Пропустить).
        /// </summary>
        public void SkipRound()
        {
            RoundResults.Add(new RoundResult
            {
                Round = CurrentRound,
                Word = CurrentWord,
                Topic = CurrentTopic,
                Correct = false,
                PointsEarned = 0
            });
        }

        public int GetCurrentRoundNumber() => CurrentRound;
        public int GetTotalRounds() => TotalRounds;
    }

    /// <summary>
    /// Информация о результате одного раунда.
    /// </summary>
    public class RoundResult
    {
        public int Round { get; set; }
        public string Word { get; set; }
        public string Topic { get; set; }
        public bool Correct { get; set; }
        public int PointsEarned { get; set; }
    }
}