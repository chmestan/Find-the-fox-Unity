using UnityEngine;
using System.Collections.Generic;

public class PopulateGrid : MonoBehaviour
{
    [Header("Word to find")]
    internal string word = "fox";

    internal int fx, fy;
    private GenerateGrid generateGrid;
    private CheckGrid checkGrid;

    [SerializeField, Space(10)] private bool debug;

    public void PlaceWordAndFillGrid()
    {
        generateGrid = GetComponent<GenerateGrid>(); 
        checkGrid = GetComponent<CheckGrid>();
        PlaceFirstLetter();  
        FillRandomLetters(); 

        checkGrid.CheckAndFixFoxOccurrences();
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
        List<Vector2Int> allDirections = GetAllDirections();
        List<Vector2Int> validDirections = new List<Vector2Int>();

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

    internal List<Vector2Int> GetAllDirections()
    {
        List<Vector2Int> directions = new List<Vector2Int>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 || dy != 0) directions.Add(new Vector2Int(dx, dy));
            }
        }
        return directions;
    }

    internal bool IsValidDirection(int fx, int fy, Vector2Int dir)
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
