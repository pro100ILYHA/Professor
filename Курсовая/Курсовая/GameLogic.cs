using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGuessGame.Classes
{
    public class GameLogic
    {
        private string secretWord;
        private string secretTheme;
        private int attempts;
        private int score;
        private List<string> history;

        // Состояние подсказок
        private bool lengthRevealed;
        private bool themeRevealed;
        private List<int> revealedLetterPositions;  // Какие позиции букв уже открыты

        public string SecretWord => secretWord;
        public string SecretTheme => secretTheme;
        public int Attempts => attempts;
        public int Score => score;
        public List<string> History => history;
        public bool LengthRevealed => lengthRevealed;
        public bool ThemeRevealed => themeRevealed;
        public int SecretWordLength => secretWord?.Length ?? 0;

        public string GetRevealedWord()
        {
            if (string.IsNullOrEmpty(secretWord)) return "???";
            char[] result = new char[secretWord.Length];
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (revealedLetterPositions.Contains(i))
                    result[i] = secretWord[i];
                else
                    result[i] = '?';
            }
            return new string(result);
        }

        public int RevealedLettersCount => revealedLetterPositions.Count;

        public GameLogic()
        {
            history = new List<string>();
            attempts = 0;
            score = 0;
            lengthRevealed = false;
            themeRevealed = false;
            revealedLetterPositions = new List<int>();
        }

        public void StartNewGame(string word, string theme)
        {
            secretWord = word?.ToLower() ?? "";
            secretTheme = theme ?? "";
            attempts = 0;
            score = 0;
            history.Clear();
            lengthRevealed = false;
            themeRevealed = false;
            revealedLetterPositions = new List<int>();
        }

        public int CheckWord(string userWord)
        {
            userWord = userWord?.ToLower() ?? "";
            if (string.IsNullOrEmpty(userWord)) return -1;

            attempts++;

            var secretCount = new Dictionary<char, int>();
            foreach (char c in secretWord)
                if (secretCount.ContainsKey(c)) secretCount[c]++; else secretCount[c] = 1;

            var userCount = new Dictionary<char, int>();
            foreach (char c in userWord)
                if (userCount.ContainsKey(c)) userCount[c]++; else userCount[c] = 1;

            int matches = 0;
            foreach (var kvp in userCount)
                if (secretCount.ContainsKey(kvp.Key))
                    matches += Math.Min(kvp.Value, secretCount[kvp.Key]);

            score += matches;
            history.Add($"{userWord} → {matches} совп.(+{matches}) | Очки: {score}");
            return matches;
        }

        public string UseHint()
        {
            score -= 5; 
            string result = "";

            if (!lengthRevealed)
            {
                lengthRevealed = true;
                result = $"Длина слова: {secretWord.Length}";
            }
            else if (!themeRevealed)
            {
                themeRevealed = true;
                result = $"Тема слова: {secretTheme}";
            }
            else
            {
                for (int i = 0; i < secretWord.Length; i++)
                {
                    if (!revealedLetterPositions.Contains(i))
                    {
                        revealedLetterPositions.Add(i);
                        revealedLetterPositions.Sort();
                        char revealedChar = secretWord[i];
                        result = $"Буква {i + 1}: {revealedChar.ToString().ToUpper()}";

                        if (revealedLetterPositions.Count == secretWord.Length)
                        {
                            result = $"Всё слово: {secretWord}";
                        }
                        break;
                    }
                }

                if (string.IsNullOrEmpty(result))
                    result = "Все буквы уже открыты";
            }

            string wordDisplay = GetRevealedWord();
            history.Add($"[ПОДСКАЗКА] {result} (-5) | Слово: {wordDisplay} | Очки: {score}");
            return result;
        }

        public int GetCurrentScore() => score;
        public bool CanUseHint() => !string.IsNullOrEmpty(secretWord);
    }
}