using System;
using Game.GameSceneScripts;
using Game.HomeSceneScripts;
using Game.Level;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class GameManager: MonoBehaviour
    {
        public HomeSceneManager HomeScene;
        public GameSceneManager GameScene;
        public static GameManager Instance;
        public UserData UserData;

        private void Awake()
        {
            Instance = this;
            UserData = ScriptableObject.CreateInstance<UserData>();
            LevelParser.ParseLevels();
            DownloadOnlineLevels();
        }

        private void Start()
        {
            UserData.LoadPlayerData();
            HomeScene.GameObject().SetActive(true);
        }

        private void DownloadOnlineLevels()
        {
            StartCoroutine(LevelParser.DownloadAndParseLevels());
        }
        
        public void OnPlayButtonClicked(int levelNo)
        {
            HomeScene.GameObject().SetActive(false);
            GameScene.GameObject().SetActive(true);
            GameScene.Init(levelNo);
        }

        public void QuitLevel()
        {
            SaveUserProgress();
            GameScene.GameObject().SetActive(false);
            GameScene.QuitGame();
            HomeScene.GameObject().SetActive(true);
            HomeScene.OnLevelsButtonClicked();
        }

        public void SaveUserProgress()
        {
            UserData.SavePlayerData();
        }
    }
}