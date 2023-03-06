using Game.Level;
using TMPro;
using UnityEngine;

namespace Game.GameSceneScripts.UI
{
    public class LevelInfoTopUI:MonoBehaviour
    {
        public GameObject scoreLabel;
        public GameObject bestScoreLabel;
        public GameObject movesLabel;
        public static LevelInfoTopUI Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            scoreLabel = transform.GetChild(0).gameObject;
            bestScoreLabel = transform.GetChild(1).gameObject;
            movesLabel = transform.GetChild(2).gameObject;
            
            //var topPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.safeArea.height,0));
            var topPosition = new Vector3(1.65f, 6.70f, 0);
            transform.position = topPosition;
        }
        

        public void Init(int levelNo)
        {
            scoreLabel.GetComponent<TextMeshPro>().text = "Score\n0";
            bestScoreLabel.GetComponent<TextMeshPro>().text = "Best\n" + GameManager.Instance.UserData.highestScores[levelNo - 1];
            movesLabel.GetComponent<TextMeshPro>().text = "Moves\n" + LevelParser.levels[levelNo - 1].moveCount;
        }
        
        public void UpdateScore(int score)
        {
            scoreLabel.GetComponent<TextMeshPro>().text = "Score\n" + score;
        }
        
        public void UpdateMoves(int moves)
        {
            movesLabel.GetComponent<TextMeshPro>().text = "Moves\n" + moves;
        }
        
        public void UpdateBestScore(int bestScore)
        {
            bestScoreLabel.GetComponent<TextMeshPro>().text = "Best\n" + bestScore;
        }
        
    }
}