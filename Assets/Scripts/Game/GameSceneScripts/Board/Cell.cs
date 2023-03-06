using Game.GameSceneScripts.Board.Item;
using UnityEngine;

namespace Game.GameSceneScripts.Board
{
    public class Cell : MonoBehaviour
    {
        public MatchItem currentMatchItem;
        public int x;
        public int y;

        private Vector2 fingerDown;
        private Vector2 fingerUp;
        private bool swipeStarted = false;
        private bool hasSwiped = false;

        private float minDistanceOfSwipe = 40f;
        private float maxDistanceFromStart = 0.5f;

        public void SetItem(MatchItem matchItem)
        {
            currentMatchItem = matchItem;
            currentMatchItem.transform.position = transform.position;
        }
        
        public void Init(int x, int y, MatchItem matchItem, float scale)
        {
            this.x = x;
            this.y = y;
            Vector3 pos = new Vector3(x,y,0);
            transform.position = pos * scale;
            SetItem(matchItem);
            transform.localScale *= scale;
            
            minDistanceOfSwipe *= scale;
            maxDistanceFromStart *= scale;
        }
        
        public void Update()
        {
            if (hasSwiped) 
                return;

            if (Input.touchCount > 0)
            {
                Swipe();
            }
        }

        private void Swipe()
        {
            // swipe is detected on the second cell
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Input.GetTouch(0).position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y,10));
            //Debug.Log("Touch position in world space: " + worldPos);
            
            float distanceFromStart = Vector2.Distance(worldPos, transform.position);
            
            if (distanceFromStart > maxDistanceFromStart) { return; }

            if (!swipeStarted)
            {
                swipeStarted = true;
                fingerDown = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                swipeStarted = false;
                fingerUp = touch.position;

                float deltaX = fingerUp.x - fingerDown.x;
                float deltaY = fingerUp.y - fingerDown.y;
                
                //Debug.Log("DeltaX: " + deltaX + " DeltaY: " + deltaY + "");

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                {
                    if (deltaX > minDistanceOfSwipe)
                    {
                        Debug.Log("Left swipe detected on cell: " + x + " " + y + "with distance: " + deltaX);
                        hasSwiped = true;
                        BoardManager.Instance.SwapCells(x, y, 'l');
                    }
                    else if (deltaX < -minDistanceOfSwipe)
                    {
                        Debug.Log("Right swipe detected on cell: " + x + " " + y + "with distance: " + deltaX);
                        hasSwiped = true;
                        BoardManager.Instance.SwapCells(x, y, 'r');
                    }
                }
                else
                {
                    if (deltaY > minDistanceOfSwipe)
                    {
                        Debug.Log("Down swipe detected on cell: " + x + " " + y + "with distance: " + deltaY);
                        hasSwiped = true;
                        BoardManager.Instance.SwapCells(x, y, 'd');
                    }
                    else if (deltaY < -minDistanceOfSwipe)
                    {
                        Debug.Log("Up swipe detected on cell: " + x + " " + y + "with distance: " + deltaY);
                        hasSwiped = true;
                        BoardManager.Instance.SwapCells(x, y, 'u');
                    }
                }

                if (hasSwiped)
                {
                    Invoke("ResetSwipe", 0.5f);
                }
                
            }
        }
    
        private void ResetSwipe()
        {
            hasSwiped = false;
        }

    }
}
