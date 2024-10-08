using System.Collections.Generic;
using UnityEngine;

public class CheckGrid : MonoBehaviour
{
    private GenerateGrid generateGrid;
    private PopulateGrid populateGrid;
    private string word = "fox";  

    [SerializeField, Space(10)] private bool debug;


    internal void CheckAndFixFoxOccurrences()
    {
        if(debug) Debug.Log("[CheckGrid] Entering CheckGrid script");

        generateGrid = GetComponent<GenerateGrid>();
        populateGrid = GetComponent<PopulateGrid>();

        int counter;
        do
        {
            counter = 0;  
            for (int x = 0; x < generateGrid.gridSize.x; x++)
            {
                for (int y = 0; y < generateGrid.gridSize.y; y++)
                {
                    if (generateGrid.grid[x, y] == 'f')
                    {
                        if (debug) Debug.Log($"[CheckGrid] Letter F at ({x},{y})");
                        List<Vector2Int> directions = populateGrid.GetAllDirections();

                        foreach (Vector2Int dir in directions)
                        {
                            if (CheckWordInDirection(x, y, dir))
                            {
                                counter++;
                                if (debug) Debug.Log($"[CheckGrid] Fox Counter = {counter}");

                                if (x != populateGrid.fx || y != populateGrid.fy)
                                {
                                    ReplaceExtraWord(x, y, dir);
                                }
                            }
                        }
                    }
                }
            }

        } while (counter > 1);  

        generateGrid.DrawGrid();
    }



    private bool CheckWordInDirection(int startX, int startY, Vector2Int dir)
    {
        if (!populateGrid.IsValidDirection(startX, startY, dir)) return false;

        for (int i = 1; i < word.Length; i++)
        {
            int newX = startX + i * dir.x;
            int newY = startY + i * dir.y;

            if (generateGrid.grid[newX, newY] != word[i])
            {
                return false;
            }
        }

        return true; 
    }

    private void ReplaceExtraWord(int startX, int startY, Vector2Int dir)
    {
        for (int i = 1; i < word.Length; i++)
        {
            int newX = startX + i * dir.x;
            int newY = startY + i * dir.y;

            if (newX >= 0 && newY >= 0 && newX < generateGrid.gridSize.x && newY < generateGrid.gridSize.y)
            {
                if (debug) Debug.Log($"[CheckGrid] Letter {generateGrid.grid[newX,newY]} at ({newX},{newY})");
                generateGrid.grid[newX, newY] = GetRandomLetter();
                if (debug) Debug.Log($"Has been replaced by {generateGrid.grid[newX,newY]} at ({newX},{newY})");
            }
        }

        if (debug) Debug.Log($"[CheckGrid] Extra 'fox' replaced at ({startX}, {startY}) in direction ({dir.x}, {dir.y})");
    }

    private char GetRandomLetter()
    {
        List<char> possibleLetters = new List<char>();
        foreach (char c in word)
        {
            possibleLetters.Add(c);
        }

        return possibleLetters[Random.Range(0, possibleLetters.Count)];
    }
}
