using System;
using Game.Level;
using UnityEngine;

namespace Game.HomeSceneScripts
{
    public class ScrollbarController : MonoBehaviour
    {
        public Transform content;
        public float scrollSpeed = 0.005f;
        private float contentHeight = 10f;

        public void Start()
        {
            content.localPosition = new Vector3(0, 1.5f, 0);
        }

        public void Init()
        {
            contentHeight = LevelParser.levels.Count;
        }
        
        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDelta = touch.deltaPosition;
                    Vector3 position = content.localPosition;
                    position.y += touchDelta.y * scrollSpeed;
                    position.y = Mathf.Clamp(position.y, 0.0f, contentHeight);
                    content.localPosition = position;
                }
            }
        }
    }
}