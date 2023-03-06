using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class CloseButton: MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                HomeSceneManager.Instance.OnCloseButtonClicked();
            }
        }
    }
}