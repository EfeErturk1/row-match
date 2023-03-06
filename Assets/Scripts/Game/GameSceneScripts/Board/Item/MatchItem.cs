using System;
using UnityEngine;

namespace Game.GameSceneScripts.Board.Item
{
    public class MatchItem : MonoBehaviour
    {
        public int x;
        public int y;
        public bool canBeSwiped = true;
        public ItemType itemType;
        private ParticleSystem particleSystem;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public void Init(int x, int y, Sprite sprite, float scale, ItemType itemType)
        {
            this.x = x;
            this.y = y;
            Vector3 pos = new Vector3(x,y,0);
            transform.position = pos * scale;
            GetComponent<SpriteRenderer>().sprite = sprite;
            transform.localScale *= scale;
            this.itemType = itemType;
        }
        
        public void Lock()
        {
            canBeSwiped = false;
        }

        public void SetOrderForSwap(int order)
        {
            GetComponent<SpriteRenderer>().sortingOrder = order;
        }
        
        public void PlayAnimation()
        {
            particleSystem.Play();
        }
    }
}