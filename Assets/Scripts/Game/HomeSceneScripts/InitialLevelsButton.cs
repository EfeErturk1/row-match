using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class InitialLevelsButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                HomeSceneManager.Instance.OnLevelsButtonClicked();
            }
        }
    }
}
