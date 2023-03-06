using System;
using UnityEngine;

namespace Game.GameSceneScripts.UI
{
    public class QuitGameButton:MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                GameManager.Instance.QuitLevel();
            }
        }
        
    }
}