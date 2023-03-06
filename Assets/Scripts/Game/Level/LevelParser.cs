using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Level
{
    public static class LevelParser
    {
        public static List<LevelData> levels;
        private static int offlineLevelsCount = 10;
        private static int onlineLevelsCountA = 5;
        private static int onlineLevelsCountB = 10;

        public static void ParseLevels()
        {
            levels = new List<LevelData>();
            for (var i = 1; i <= offlineLevelsCount; i++)
            {
                string filename = "RM_A" + i;
                var level = ParseLevel(filename);
                levels.Add(level);
            }
            
            Debug.Log("Levels parsed: " + levels.Count);
        }
        
        private static LevelData ParseLevel(String filename)
        {
            var levelData = new LevelData();
            var filePath = Application.dataPath + "/Levels/"+filename;
            if (!File.Exists(filePath + ".txt"))
            {
                if (File.Exists(filePath))
                {
                    string newFilePath = Path.ChangeExtension(filePath, ".txt");
                    File.Move(filePath, newFilePath);
                    filePath = newFilePath;
                }
                else
                {
                    Debug.LogError("File not found: " + filePath);
                    return null;
                }
            }
            else
            {
                filePath += ".txt";
            }

            //Debug.Log("File path: " + filePath);

            StreamReader reader = new StreamReader(filePath);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line.StartsWith("level_number:"))
                {
                    levelData.levelNo = int.Parse(line.Substring(14));
                }
                else if (line.StartsWith("grid_width:"))
                {
                    levelData.gridWidth = int.Parse(line.Substring(11));
                }
                else if (line.StartsWith("grid_height:"))
                {
                    levelData.gridHeight = int.Parse(line.Substring(12));
                }
                else if (line.StartsWith("move_count:"))
                {
                    levelData.moveCount = int.Parse(line.Substring(11));
                }
                else if (line.StartsWith("grid:"))
                {
                    string[] gridLineData = line.Substring(5).Split(',');

                    int i = 0;
                    levelData.grid = new char[gridLineData.Length];
                    foreach (string gridItem in gridLineData)
                    {
                        levelData.grid[i++] = gridItem.Trim()[0];
                    }
                }
            }

            reader.Close();
            return levelData;
        }

        private static string FindFileName(int i)
        {
            if (i <= onlineLevelsCountA)
            {
                return "RM_A" + (i + offlineLevelsCount);
            }
            else
            {
                return "RM_B" + (i - onlineLevelsCountA);
            }
        }

        public static IEnumerator DownloadAndParseLevels()
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Waiting for internet connection...");
                yield return new WaitForSeconds(1f);
            }
            
            for (var i = 1; i <= onlineLevelsCountA + onlineLevelsCountB; i++)
            {
                var filename = FindFileName(i);
                var url = "https://row-match.s3.amazonaws.com/levels/" + filename;
                
                var filePath = Application.dataPath + "/Levels/" + filename;

                if (File.Exists(filePath + ".txt"))
                {
                    var level = ParseLevel(filename);
                    levels.Add(level);
                }
                else
                {
                    var www = UnityWebRequest.Get(url);
                    yield return www.SendWebRequest();
 
                    if(www.result != UnityWebRequest.Result.Success) {
                        Debug.Log("Error downloading level: " + www.error);
                        yield return new WaitForSeconds(1f);
                    }
                    else {
                        File.WriteAllBytes(filePath, www.downloadHandler.data);
                        Debug.Log("Level downloaded to " + filePath);
                
                        var level = ParseLevel(filename);
                        levels.Add(level);
                    }
                }
            }
            
            Debug.Log("Levels parsed after download: " + levels.Count);
        }
    }
}