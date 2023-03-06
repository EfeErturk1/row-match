using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UserData:ScriptableObject
    {
        public int currentLevelNo;
        public List<int> highestScores;
        private int totalLevels = 25;
        
        private const string CURRENT_LEVEL_KEY = "CurrentLevel";
        private const string HIGHEST_SCORE_COUNT_KEY = "HighestScores";
        
        public UserData()
        {
            currentLevelNo = 1;
            highestScores = new List<int>();
            for (var i = 0; i < totalLevels; i++)
            {
                highestScores.Add(0);
            }
            Debug.Log("UserData created");
            
        }

        private void OnEnable()
        {
            // to reset the data
            //PlayerPrefs.DeleteAll();
        }

        public void SavePlayerData()
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevelNo);
            PlayerPrefs.SetInt(HIGHEST_SCORE_COUNT_KEY, highestScores.Count);

            for (var i = 0; i < highestScores.Count; i++)
            {
                var key = GetHighestScoreKey(i);
                var value = highestScores[i];
                PlayerPrefs.SetInt(key, value);
            }

            PlayerPrefs.Save();
        }

        public void LoadPlayerData()
        {
            currentLevelNo = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);

            var scoreCount = PlayerPrefs.GetInt(HIGHEST_SCORE_COUNT_KEY, 25);

            for (var i = 0; i < scoreCount; i++)
            {
                var key = GetHighestScoreKey(i);
                var value = PlayerPrefs.GetInt(key, 0);
                highestScores[i] = value;
            }
            Debug.Log("UserData loaded");
        }

        private string GetHighestScoreKey(int index)
        {
            return $"{HIGHEST_SCORE_COUNT_KEY}_{index}";
        }
        
        public void UpdateHighestScore(int levelNo, int score)
        {
            highestScores[levelNo - 1] = score;
        }

        public void UpdateCurrentLevelNo(int levelNo)
        {
            currentLevelNo = levelNo;
        }
    }
}