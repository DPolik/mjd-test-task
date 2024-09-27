using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private List<Layer> layers;
    private int layerWidth;
    private int layerHeight;
    private int cellTypesAmount;
    private int totalCellCount;
    private int totalMatchedCells;
    private Cell currentSelectedCell;
    private bool gameIsRunning = false;

    private IGameBoardView boardView;
    [SerializeField] private GameObject boardViewPrefab;

    public Action onMatchedCells;
    public Action onGameFinished;

    public void Init(int layerCount, int layerWidth, int layerHeight, int cellTypesAmount)
    {
        totalCellCount = layerCount * layerWidth * layerHeight;
        Debug.Assert(totalCellCount > cellTypesAmount * 2, "Not enough cells to fill all types");
        Debug.Assert(totalCellCount % 2 == 0, "Number of cells must be even");

        this.layerHeight = layerHeight;
        this.layerWidth = layerWidth;
        layers = new List<Layer>(layerCount);
        while (layerCount > 0)
        {
            layers.Add(new Layer(layerWidth, layerHeight));
            --layerCount;
        }

        this.cellTypesAmount = cellTypesAmount;
        totalMatchedCells = 0;

        boardView = Instantiate(boardViewPrefab).GetComponent<IGameBoardView>();

        InputManager.onDrag += OnInputDrag;
    }

    private void OnInputDrag(int hDirection)
    {
        if(gameIsRunning)
        {
            boardView.RotateBoard(hDirection);
        }
    }

    public void ShowBoard()
    {
        var remainingTypes = GetCellsPerType();  
        for (int i = 0; i < layers.Count; i++)
        {
            for (int j = 0; j < layerWidth * layerHeight; j++)
            {
                Cell newCell = new Cell(PopRandomType(remainingTypes), i, j);
                layers[i].AddCell(newCell);
                newCell.onCellClicked += OnCellClicked;
            }
            layers[i].InitState();
        }

        boardView.Init(layers, totalCellCount, cellTypesAmount);
    }

    private int PopRandomType(List<(int, int)> remainingTypes)
    {
        int typeIndex = UnityEngine.Random.Range(0, remainingTypes.Count - 1); // Random type per cell
        var type = remainingTypes[typeIndex];
        remainingTypes[typeIndex] = (type.Item1, --type.Item2);
        if (type.Item2 <= 0)
        {
            remainingTypes.Remove(type);
        }
        return type.Item1;
    }

    private List<(int,int)> GetCellsPerType()
    {
        List<(int, int)> remainingTypes = new List<(int, int)>(cellTypesAmount); // Aux list of (celltype, amount) to randomize attribution of types
        int totalCellsPerType = totalCellCount / cellTypesAmount; // need at least a pair of cells per type
        for (int i = 0; i < cellTypesAmount; i++)
        {
            remainingTypes.Add((i, totalCellsPerType));
        }
        remainingTypes[0] = (remainingTypes[0].Item1, remainingTypes[0].Item2 + (totalCellCount % (cellTypesAmount * 2))); // remainder is attributed to the first type (lazy solution)

        return remainingTypes;
    }

    private void ResetBoard()
    {
        var remainingTypes = GetCellsPerType();
        for (int i = 0; i < layers.Count; i++)
        {
            for (int j = 0; j < layerWidth * layerHeight; j++)
            {
                Cell cell = layers[i].CellList[j];
                cell.CellType = PopRandomType(remainingTypes);
            }
            layers[i].InitState();
        }

        boardView.Reset(layers);
        totalMatchedCells = 0;
    }

    public void StartGame()
    {
        gameIsRunning = true;
    }

    private void OnCellClicked(Cell cell)
    {
        if (!cell.IsFree)
            return;
        if (!gameIsRunning)
            return;

        if(currentSelectedCell != null)
        {
            if(CellsMatch(currentSelectedCell, cell))
            {
                HandleMatchingCells(currentSelectedCell, cell);
                currentSelectedCell = null;
            }
            else
            {
                currentSelectedCell.ResetSelected();
                currentSelectedCell = cell;
                currentSelectedCell.ShowSelected();
            }
        }
        else
        {
            currentSelectedCell = cell;
            currentSelectedCell.ShowSelected();
        }
    }

    private void HandleMatchingCells(Cell cell1, Cell cell2)
    {
        cell1.Deactivate();
        cell2.Deactivate();
        layers[cell1.LayerIndex].UpdateNeighbours(cell1.CellIndex);
        layers[cell2.LayerIndex].UpdateNeighbours(cell2.CellIndex);

        onMatchedCells?.Invoke();

        totalMatchedCells += 2;
        if(totalMatchedCells == totalCellCount)
        {
            onGameFinished?.Invoke();
        }

    }

    private bool CellsMatch(Cell cell1, Cell cell2)
    {
        return cell1 != cell2 && cell1.CellType == cell2.CellType;
    }

    private void UpdateState()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].UpdateState();
        }
    }

    public void StopGame()
    {
        gameIsRunning = false;
        if(currentSelectedCell != null)
        {
            currentSelectedCell.ResetSelected();
            currentSelectedCell = null;
        }
    }

    public void Reset()
    {
        StopGame();
        ResetBoard();
    }

    // Debug print board
    public override string ToString()
    {
        var output = "";
        for (int i = 0; i < layers.Count; i++)
        {
            output += layers[i] + "\n";
            output += "=================";
        }

        return output;
    }
}

