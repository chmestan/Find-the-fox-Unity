using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateGrid : MonoBehaviour
{

    [Header("Game Size")]
    [SerializeField] internal Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField] float gapX = 0.5f;
    [SerializeField] float gapY = 0.5f;

    [Header("Grid Elements")]
    [SerializeField] GameObject buttonPrefab; 
    [SerializeField] Transform parentCanvas; 

    private List<Button> gridButtons = new List<Button>();
    public char[,] grid;

    private PopulateGrid populateGrid;


    void Start()
    {
        populateGrid = GetComponent<PopulateGrid>(); 
        int wordLength = populateGrid.word.Length;

        if (gridSize.x >= wordLength && gridSize.y >= wordLength)
        {
            InitializeGrid(gridSize.x, gridSize.y);
            populateGrid.PlaceWordAndFillGrid();
        }
        else 
        {
            Debug.LogError("[GenerateGrid] Grid length and width need to be larger or equal to the word's length.");
        }
    }

    private void InitializeGrid(int sizeX, int sizeY)
    {
        grid = new char[sizeX,sizeY]; 
        GenerateEmptyGrid(); 
    }

    private void GenerateEmptyGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = '.';
            }
        }
    }

    internal void DrawGrid()
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
