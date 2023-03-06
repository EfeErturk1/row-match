using Game.GameSceneScripts;
using Game.GameSceneScripts.Board;
using Game.GameSceneScripts.UI;
using UnityEngine;

namespace Game.Level
{
    public class LevelWinLoseManager
    {
        public int levelNo;
        public int currentScore;
        public int highestScore;
        public int remainingMoveCount;
        private LevelState levelState; 

        public LevelWinLoseManager(int levelNo)
        {
            this.levelNo = levelNo;
            highestScore = GameManager.Instance.UserData.highestScores[levelNo - 1];
            currentScore = 0;
            remainingMoveCount = LevelParser.levels[levelNo - 1].moveCount;
            levelState = LevelState.PLAYING;
        }
        
        public void IncreaseScore(int score)
        {
            currentScore += score;
            LevelInfoTopUI.Instance.UpdateScore(currentScore);
        }

        public void DecreaseMove()
        {
            remainingMoveCount--;
            LevelInfoTopUI.Instance.UpdateMoves(remainingMoveCount);
            CheckLevelEnd();
        }
        
        public void CheckLevelEnd()
        {
            if (remainingMoveCount == 0)
            {
                BoardManager.Instance.DisableTouchOnBoard();
                if (CheckHighestScore())
                {
                    InitiateWinSequence();
                }
                else
                {
                    InitiateLoseSequence();
                }
                
            }
            else if (CheckAutomaticLose())
            {
                BoardManager.Instance.DisableTouchOnBoard();
                if (CheckHighestScore())
                {
                    InitiateWinSequence();
                }
                else
                {
                    Debug.Log("Automatic Lose");
                    InitiateLoseSequence();
                }
            }
        }
        
        private void InitiateLoseSequence()
        {
            levelState = LevelState.LOSE;
            GameSceneManager.Instance.InitiateLoseSequence();
        }

        private void InitiateWinSequence()
        {
            levelState = LevelState.WIN;
            highestScore = currentScore;
            GameManager.Instance.UserData.UpdateHighestScore(levelNo, highestScore);
            if (levelNo+1 > GameManager.Instance.UserData.currentLevelNo)
            {
                GameManager.Instance.UserData.UpdateCurrentLevelNo(levelNo+1);
                GameManager.Instance.UserData.SavePlayerData();
            }
            LevelInfoTopUI.Instance.UpdateBestScore(currentScore);
            GameSceneManager.Instance.InitiateWinSequence(highestScore);
        }

        public bool CheckHighestScore()
        {
            return currentScore > highestScore;
        }
        
        public bool CheckAutomaticLose()
        {
            return BoardManager.Instance.CheckAutomaticLose(remainingMoveCount);
        }
    }
}