using System;
using Game.Level;
using TMPro;
using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class LevelButton:MonoBehaviour
    {
        public Sprite[] backgrounds;
        public bool levelUnlocked;
        public int levelNo;
        public GameObject playButton;
        
        public void Init(int levelNo)
        {
            this.levelNo = levelNo;
            
            var tempPos = transform.position;
            Vector3 pos = new Vector3(tempPos.x,tempPos.y - levelNo + 1,0);
            transform.position = pos;

            UpdateInfoLabel();
        }

        private void OnMouseDown()
        {
            if (!levelUnlocked) { return; }
            if (Input.GetMouseButton(0))
            {
                GameManager.Instance.OnPlayButtonClicked(levelNo);
            }
        }
        
        public void UpdateInfoLabel()
        {
            this.levelUnlocked =  GameManager.Instance.UserData.currentLevelNo >= levelNo;
            
            playButton = transform.GetChild(0).gameObject;
            playButton.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = levelUnlocked ? backgrounds[0] : backgrounds[1];
            playButton.transform.GetChild(0).transform.localScale = levelUnlocked ? new Vector3(0.043f, 0.13f, 0.087f): new Vector3(0.067f,0.10f,0.0875f);

            var levelData = LevelParser.levels[levelNo-1];
            transform.GetChild(2).GetComponent<TextMeshPro>().text = "Level " + levelData.levelNo + " - " +
                                               levelData.moveCount + " moves \nHighest Score: " + GameManager.Instance.UserData.highestScores[levelNo - 1];
        }
    }
}