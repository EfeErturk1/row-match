using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class HomeSceneManager : MonoBehaviour
    {
        public GameObject levelsButton;
        public LevelsPopUp levelsPopUp;
        public ScrollbarController scrollbarController;
        public LevelButtonContainer levelButtonContainer;
        
        public static HomeSceneManager Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            levelsButton.SetActive(true);
            levelsPopUp.gameObject.SetActive(false);
            levelButtonContainer.Init();
        }

        public void OnLevelsButtonClicked()
        {
            levelsButton.SetActive(false);
            levelsPopUp.gameObject.SetActive(true);
            levelsPopUp.StartAnimation();
            levelButtonContainer.UpdateLevelButtons();
            scrollbarController.Init();
        }
        
        public void OnCloseButtonClicked()
        {
            levelsPopUp.gameObject.SetActive(false);
            levelsButton.SetActive(true);
        }
    }
}