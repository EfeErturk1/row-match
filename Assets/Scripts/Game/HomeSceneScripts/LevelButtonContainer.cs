using System.Collections.Generic;
using Game.Level;
using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class LevelButtonContainer:MonoBehaviour
    {
        public GameObject levelButton;
        private List<GameObject> levelButtons;
        public static LevelButtonContainer Instance;

        private void Start()
        {
            Instance = this;
            levelButton = transform.GetChild(0).gameObject;
            levelButtons = new List<GameObject>();
        }
        
        public void Init()
        {
            CreateLevelButtons();
        }
        
        private void CreateLevelButtons()
        {
            for (var i = 1; i <= LevelParser.levels.Count; i++)
            {
                GameObject spawnedCell = Instantiate(levelButton, transform, true);
                
                var button = spawnedCell.GetComponent<LevelButton>();
                if (button == null)
                {
                    button = spawnedCell.AddComponent<LevelButton>();
                }

                button.Init(i);
                spawnedCell.SetActive(true);
                levelButtons.Add(spawnedCell);
            }
        }
        
        public void UpdateLevelButtons()
        {
            foreach (var button in levelButtons)
            {
                button.GetComponent<LevelButton>().UpdateInfoLabel();
            }
        }
    }
}