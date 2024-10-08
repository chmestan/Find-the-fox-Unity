using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Word")]
    [SerializeField] string[] possibleLetters = new string[] { "f", "o", "x" };

    [Header("Game Size")]
    [SerializeField] Vector2 gridSize = new Vector2(5, 5);
    [SerializeField] float gapX = 0.5f;
    [SerializeField] float gapY = 0.5f;

    [Header("Grid Elements")]
    [SerializeField] GameObject buttonPrefab; 
    [SerializeField] Transform parentCanvas; 

    private List<Button> gridButtons = new List<Button>();
    public char[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new char[(int)gridSize.x, (int)gridSize.y]; // explicit conversion necessary
        GenerateGrid(); // grid with placeholder characters
        InstantiateGrid(); // grid represented by buttons
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = '.';
                // gridData[x, y] = possibleLetters[Random.Range(0, possibleLetters.Length)][0];
            }
        }
    }

    private void InstantiateGrid()
    {

        RectTransform buttonRect = buttonPrefab.GetComponent<RectTransform>();
        Vector2 buttonSize = buttonRect.rect.size;

        float totalGridWidth = (gridSize.x * buttonSize.x) + (gridSize.x - 1) * gapX;
        float totalGridHeight = (gridSize.y * buttonSize.y) + (gridSize.y - 1) * gapY;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject newButton = Instantiate(buttonPrefab, parentCanvas);
                Button button = newButton.GetComponent<Button>();
                gridButtons.Add(button);

                // given button's anchor at its center:
                float posX = (x * (buttonSize.x + gapX)) - totalGridWidth / 2 + buttonSize.x / 2; 
                float posY = (y * (buttonSize.y + gapY)) - totalGridHeight / 2 + buttonSize.y / 2;

                newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, -posY); // local to parent Canvas

                TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
                buttonText.text = grid[x, y].ToString();
            }
        }
    }
}
