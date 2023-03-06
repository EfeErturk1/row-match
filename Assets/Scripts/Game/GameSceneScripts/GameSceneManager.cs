using System.Collections;
using System.Collections.Generic;
using Game.GameSceneScripts.Board;
using Game.GameSceneScripts.UI;
using Game.Level;
using UnityEngine;

namespace Game.GameSceneScripts
{
    public class GameSceneManager: MonoBehaviour
    {
        public BoardManager boardManager;
        public static GameSceneManager Instance;
        public LevelInfoTopUI levelInfoTopUI;
        public LevelLosePopUp levelLosePopUp;
        public BlackBackground blackBackground;
        public LevelWinPopUp levelWinPopUp;
        
        public void Awake()
        {
            Instance = this;
        }

        public void Init(int levelNo)
        {
            Camera.main.transform.position = new Vector3(1.65f, 2.7f, -10);
            CreateLevel(levelNo);
            levelInfoTopUI.Init(levelNo);
            levelInfoTopUI.gameObject.SetActive(true);
            levelLosePopUp.gameObject.SetActive(false);
            blackBackground.gameObject.SetActive(false);
            levelWinPopUp.gameObject.SetActive(false);
        }

        public void CreateLevel(int levelNo)
        {
            boardManager.UploadBoard(LevelParser.levels[levelNo - 1]);
        }
        
        public void InitiateLoseSequence()
        {
            StartCoroutine(LoseSequence());
        }
        
        public void InitiateWinSequence(int highScore)
        {
            StartCoroutine(WinSequence(highScore));
        }

        private IEnumerator LoseSequence()
        {
            yield return new WaitForSeconds(1f); 
            levelLosePopUp.gameObject.SetActive(true);
            blackBackground.gameObject.SetActive(true);
            blackBackground.StartFadeOut(0.3f,1.4f);
            levelLosePopUp.StartAnimation();
        }

        public IEnumerator WinSequence(int highScore)
        {
            yield return new WaitForSeconds(1.3f); 
            levelWinPopUp.gameObject.SetActive(true);
            blackBackground.gameObject.SetActive(true);
            blackBackground.StartFadeOut(0.3f,2.1f);
            levelWinPopUp.UpdateHighScoreText(highScore);
            levelWinPopUp.StartAnimation();
        }

        public void QuitGame()
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);
            boardManager.ResetBoard();
            levelInfoTopUI.gameObject.SetActive(false);
            levelLosePopUp.gameObject.SetActive(false);
            blackBackground.gameObject.SetActive(false);
            levelWinPopUp.gameObject.SetActive(false);
        }
    }
}