using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.GameSceneScripts.Board.Item;
using Game.Level;
using UnityEngine;

namespace Game.GameSceneScripts.Board
{
    public class BoardManager:MonoBehaviour
    {
        public static Cell[,] cells;
        public GameObject item;
        public GameObject cell;
        public Sprite[] sprites;
        public static BoardManager Instance;
        public LevelWinLoseManager levelWinLoseManager;
        private LevelData levelData;
        
        public void Awake()
        {
            Instance = this;
        }

        public void UploadBoard( LevelData data)
        {
            this.levelData = data;
            levelWinLoseManager = new LevelWinLoseManager(levelData.levelNo);
            cells = new Cell[levelData.gridWidth, levelData.gridHeight];
            var scale = FindScalingFactor(levelData.gridHeight, levelData.gridWidth);
            for (var i = 0; i < levelData.gridHeight; i++)
            {
                for (var j = 0; j < levelData.gridWidth; j++)
                {
                    GameObject spawnedCell = Instantiate(cell, transform, true);
                    
                    var boardCell = spawnedCell.GetComponent<Cell>();
                    if (boardCell == null)
                    {
                        boardCell = spawnedCell.AddComponent<Cell>();
                    }
                    
                    GameObject spawnedItem = Instantiate(item, transform, true);
                    
                    var currentItem = spawnedItem.GetComponent<MatchItem>();
                    if (currentItem == null)
                    {
                        currentItem = spawnedItem.AddComponent<MatchItem>();
                    }

                    var sprite = sprites[0];
                    var itemType = ItemType.RED;
                    var currentPos = i * levelData.gridWidth + j;
                    switch (levelData.grid[currentPos])
                    {
                        case 'r':
                            break;
                        case 'g':
                            sprite = sprites[1];
                            itemType = ItemType.GREEN;
                            break;
                        case 'b':
                            sprite = sprites[2];
                            itemType = ItemType.BLUE;
                            break;
                        case 'y':
                            sprite = sprites[3];
                            itemType = ItemType.YELLOW;
                            break;
                    }
                    
                    currentItem.Init(j,i,sprite, scale, itemType);
                    boardCell.Init(j,i, currentItem,scale);
                    spawnedCell.SetActive(true);
                    spawnedItem.SetActive(true);
                    cells[j, i] = boardCell;
                }
            }
        }

        public float FindScalingFactor(int levelHeight, int levelWidth)
        {
            var widthMultiplier = 0.85f;
            var heightMultiplier = 0.6f;
            if (Screen.orientation != ScreenOrientation.Portrait)
            {
                widthMultiplier = 0.4f;
                heightMultiplier = 1f;
            }
            
            var width = Screen.safeArea.width * widthMultiplier;
            var height = Screen.safeArea.height * heightMultiplier;
            var cellWidth = width / levelWidth;
            var cellHeight = height / levelHeight;
            var scale = Math.Min(cellWidth,cellHeight);
            var currentScale = width / 4f;
            return scale/currentScale;
        }

        public Cell GetCell(int x, int y, char direction)
        {
            try
            {
                switch (direction)
                {
                    case 'r':
                        return cells[x + 1,y];
                    case 'l':
                        return cells[x - 1,y];
                    case 'u':
                        return cells[x, y + 1];
                    case 'd':
                        return cells[x, y - 1];
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Out of bounds exception: x" + x + " y=" + y + " direction=" + direction);
                throw;
            }
            

            return null;
        }

        public void SwapCells(int x, int y, char direction)
        {
            var cell1 = cells[x, y];
            var cell2 = GetCell(x, y, direction);
            SwapItemsOfCells(cell1, cell2);
        }

        public void SwapAnimation(Cell cell1, Cell cell2)
        {
            var item1 = cell1.currentMatchItem;
            var item2 = cell2.currentMatchItem;
            var item1Pos = item1.transform.position;
            var item2Pos = item2.transform.position;

            var swapDuration = 0.15f;
            item1.SetOrderForSwap(120);
            item2.SetOrderForSwap(150);
            var sequence = DOTween.Sequence();
            sequence.Append(item1.transform.DOMove(item2Pos, swapDuration).SetEase(Ease.OutQuart));
            sequence.Join(item2.transform.DOMove(item1Pos, swapDuration).SetEase(Ease.OutQuart));
            sequence.OnComplete((() => OnCompleteSwapAnimation(cell1, cell2)));
            sequence.Play();
        }

        public void OnCompleteSwapAnimation(Cell cell1, Cell cell2)
        {
            var temp = cell1.currentMatchItem;
            cell1.SetItem(cell2.currentMatchItem);
            cell2.SetItem(temp);
            cell1.currentMatchItem.SetOrderForSwap(100);
            cell2.currentMatchItem.SetOrderForSwap(100);
            
            Debug.Log("Swapped cells: " + cell1.x + " " + cell1.y + " with " + cell2.x + " " + cell2.y);
            if (CheckRowMatch(cell1.y))
            {
                var itemType = cells[0, cell1.y].currentMatchItem.itemType;
                var point = CalculateRowMatchPoint(itemType);
                levelWinLoseManager.IncreaseScore(point);
                LockRow(cell1.y);
                
            }
            if (cell1.y != cell2.y && CheckRowMatch(cell2.y))
            {
                var itemType = cells[0, cell2.y].currentMatchItem.itemType;
                var point = CalculateRowMatchPoint(itemType);
                levelWinLoseManager.IncreaseScore(point);
                LockRow(cell2.y);

            }
            
            levelWinLoseManager.DecreaseMove();
        }
        
        public void SwapItemsOfCells(Cell cell1, Cell cell2)
        {
            if (!cell1.currentMatchItem.canBeSwiped || !cell2.currentMatchItem.canBeSwiped)
            {
                Debug.Log("One of the cells is locked");
                return;
            }
            
            SwapAnimation(cell1 ,cell2);
        }
        
        public bool CheckRowMatch(int row)
        {
            var firstItemType = cells[0, row].currentMatchItem.itemType;
            for (var i = 1; i < levelData.gridWidth; i++)
            {
                var currentItemType = cells[i, row].currentMatchItem.itemType;
                if (firstItemType != currentItemType)
                {
                    return false;
                }
            }
            Debug.Log("Row Match on row: " + row);
            return true;
        }

        public int CalculateRowMatchPoint(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.RED:
                    return 100;
                case ItemType.GREEN:
                    return 150;
                case ItemType.BLUE:
                    return 200;
                case ItemType.YELLOW:
                    return 250;
            }

            return 0;
        }
        
        public void LockRow(int row)
        {
            for (var i = 0; i < levelData.gridWidth; i++)
            {
                cells[i, row].currentMatchItem.PlayAnimation();
                cells[i, row].currentMatchItem.Lock();
            }
        }

        public bool CheckAutomaticLose(int remainingMove)
        {
            var availableRows = new bool[levelData.gridHeight];
            
            for (var i = 0; i < levelData.gridHeight; i++)
            {
                availableRows[i] = cells[0, i].currentMatchItem.canBeSwiped;
            }
            
            for (var i = 0; i < levelData.gridHeight; i++)
            {
                if (!availableRows[i]) { continue; }
                var startRow = i;
                while (i < levelData.gridHeight && availableRows[i])
                {
                    i++;
                }
                var endRow = i - 1;
                if (!CheckSmallBoard(startRow, endRow, remainingMove))
                {
                    return false;
                }
            }
            
            
            return true;
        }

        private bool CheckSmallBoard(int startRow, int endRow, int remainingMove)
        {
            for (var i = startRow; i <= endRow; i++)
            {
                var itemTypes = FindMaxItemInRow(i);
                foreach (var itemType in itemTypes)
                {
                    if (CheckIfEnoughItemsExist(startRow, i, endRow, remainingMove, itemType.Item1, itemType.Item2))
                    {
                        if (CheckIfEnoughMoves(startRow, i, endRow, remainingMove, itemType.Item1))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        private List<Tuple<ItemType,int>> FindMaxItemInRow(int row)
        {
            var result = new List<Tuple<ItemType,int>> ();
            var itemTypeCount = new int[4];
            for (var i = 0; i < levelData.gridWidth; i++)
            {
                var itemType = cells[i, row].currentMatchItem.itemType;
                itemTypeCount[(int) itemType]++;
            }

            var max = 0;
            for (var i = 0; i < 4; i++)
            {
                if (itemTypeCount[i] > max)
                {
                    max = itemTypeCount[i];
                    result.Clear();
                    var tuple = new Tuple<ItemType, int>((ItemType) i, max);
                    result.Add(tuple);
                }

                else if (itemTypeCount[i] == max)
                {
                    var tuple = new Tuple<ItemType, int>((ItemType) i, max);
                    result.Add(tuple);
                }
            }

            return result;
        }

        private bool CheckIfEnoughItemsExist(int startRow, int currentRow, int endRow, int remainingMoves, ItemType itemType, int count)
        {
            var remainingItemCount = levelData.gridWidth - count;
            if (remainingItemCount > remainingMoves)
            {
                return false;
            }
            
            //Debug.Log("Row" + currentRow + " needs " + remainingItemCount + " more items of type " + itemType + " in " + remainingMoves + " moves");
            
            for (var i = startRow; i <= endRow; i++)
            {
                if(i == currentRow){continue;}
                for (var j = 0; j < levelData.gridWidth; j++)
                {
                    if (cells[j, i].currentMatchItem.itemType == itemType)
                    {
                        remainingItemCount--;
                        if (remainingItemCount <= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        public bool CheckIfEnoughMoves(int startRow, int currentRow, int endRow, int remainingMoves, ItemType itemType)
        {

            var totalDistance = 0;
            var visitedCells = new List<Cell>();
            for (var i = 0; i < levelData.gridWidth; i++)
            {
                if (cells[i, currentRow].currentMatchItem.itemType == itemType)
                {
                    continue;
                }
                
                var minDistance = int.MaxValue;
                for (var j = startRow; j <= endRow; j++)
                {
                    if (j == currentRow) { continue; }
                    for (var k = 0; k < levelData.gridWidth; k++)
                    {
                        if (cells[k, j].currentMatchItem.itemType == itemType && !visitedCells.Contains(cells[k, j]))
                        {
                            var distance = Mathf.Abs(i - k) + Mathf.Abs(currentRow - j);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                visitedCells.Add(cells[k, j]);
                            }
                        }
                    }
                }

                totalDistance += minDistance;
            }
            
            return totalDistance <= remainingMoves;
        }
        
        public void DisableTouchOnBoard()
        {
            foreach (var cell in cells)
            {
                cell.currentMatchItem.Lock();
            }
        }
        
        public void ResetBoard()
        {
            foreach (var cell in cells)
            {
                Destroy(cell.currentMatchItem.gameObject);
                Destroy(cell.gameObject);
            }
        }
    }
}