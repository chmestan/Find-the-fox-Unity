using UnityEngine;
using System.Collections.Generic;

public class PopulateGrid : MonoBehaviour
{
    [Header("Word to find")]
    internal string word = "fox";

    internal int fx, fy;
    private GenerateGrid generateGrid;

    [SerializeField, Space(10)] bool debug;

    public void PlaceWordAndFillGrid()
    {
        generateGrid = GetComponent<GenerateGrid>(); 
        PlaceFirstLetter();  
        FillRandomLetters(); 

        generateGrid.DrawGrid();
    }

    private void PlaceFirstLetter()
    {
        List<Vector2Int> validDirections;

        do 
        {
            fx = Random.Range(0, generateGrid.gridSize.x);
            fy = Random.Range(0, generateGrid.gridSize.y);
            
            if (debug) Debug.Log($"fx = {fx}; fy = {fy}");

            generateGrid.grid[fx, fy] = word[0]; 

            validDirections = GetValidWordDirections(fx, fy);
        } while (validDirections.Count <= 0);

        Vector2Int dir = validDirections[Random.Range(0, validDirections.Count)];
        PlaceOtherLetters(fx, fy, dir);

    }

    private List<Vector2Int> GetValidWordDirections(int fx, int fy)
    {
        List<Vector2Int> allDirections = new List<Vector2Int>();
        List<Vector2Int> validDirections = new List<Vector2Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) allDirections.Add(new Vector2Int(x, y));
            }
        }

        // Is equivalent to:
            // new Vector2Int(-1, -1), // Top-left
            // new Vector2Int(0, -1),  // Top
            // new Vector2Int(1, -1),  // Top-right
            // new Vector2Int(-1, 0),  // Left
            // new Vector2Int(1, 0),   // Right
            // new Vector2Int(-1, 1),  // Bottom-left
            // new Vector2Int(0, 1),   // Bottom
            // new Vector2Int(1, 1)    // Bottom-right

        foreach (Vector2Int dir in allDirections)
        {
            if (IsValidDirection(fx, fy, dir))
            {
                if (debug) Debug.Log($"[PopulateGrid] {dir} est valide");
                validDirections.Add(dir);
            }
            else if (debug) Debug.Log($"[PopulateGrid] {dir} n'est pas valide");
        }
        return validDirections;

    }

    private bool IsValidDirection(int fx, int fy, Vector2Int dir)
    {

        for (int i = 1; i < word.Length; i++)  // Start at 1 (from second letter)
        {
            int newX = fx + i * dir.x;
            int newY = fy + i * dir.y;

            if (newX < 0 || newY < 0 || newX >= generateGrid.gridSize.x || newY >= generateGrid.gridSize.y)
            {
                return false; 
            }
        }
        return true; 
    }

    private void PlaceOtherLetters(int fx, int fy, Vector2Int dir)
    {
        for (int i = 1; i < word.Length; i++) // Start at 1 (from second letter)
        {
            int letterPosX = fx + i * dir.x;
            int letterPosY = fy + i * dir.y;

            generateGrid.grid[letterPosX, letterPosY] = word[i];
        }

        if (debug) Debug.Log($"[PopulateGrid] First letter coordinates = ({fx}, {fy}) \nDirection = ({dir.x}, {dir.y})");
    }

    private void FillRandomLetters()
    {

        List<char> possibleLetters = new List<char>();
        foreach (char c in word)
        {
            possibleLetters.Add(c);
        }

        for (int x = 0; x < generateGrid.gridSize.x; x++)
        {
            for (int y = 0; y < generateGrid.gridSize.y; y++)
            {
                if (generateGrid.grid[x, y] == '.')  
                {
                    generateGrid.grid[x, y] = possibleLetters[Random.Range(0, possibleLetters.Count)];
                }
            }
        }
    }
}
