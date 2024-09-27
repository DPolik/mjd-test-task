using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    private int height;
    public int Height { get => height; }

    private int width;
    public int Width { get => width; }

    private List<Cell> cellList;
    public List<Cell> CellList { get => cellList; }

    public Layer(int width, int height)
    {
        this.height = height;
        this.width = width;
        cellList = new List<Cell>(height * width);
    }

    public void AddCell(Cell cell)
    {
        cellList.Add(cell);
    }

    public override string ToString() // Debug method
    {
        string output = "";
        for (int i = 0; i < cellList.Count; i++)
        {
            string cell = "";
            if (!cellList[i].IsActive)
                cell = "X";
            else
                cell = cellList[i].IsFree ? "F" : "B";
            output += cell + " ";

            if ((i + 1) % width == 0)
            {
                output += "\n";
            }
        }

        return output;
    }

    public void UpdateState()
    {
        // for each cell, check neighbours if active and update isFree
        for (int i = 0; i < cellList.Count; i++)
        {
            UpdateState(i);
        }
    }

    public void InitState()
    {
        // for each cell, check you are a corner and update isFree
        for (int i = 0; i < cellList.Count; i++)
        {
            cellList[i].IsActive = true;
            if (i == 0  || i == width-1 || i == cellList.Count-1 || i == cellList.Count - width)
            {
                cellList[i].IsFree = true;
            }
            else
            {
                cellList[i].IsFree = false;
            }
        }
    }

    private void UpdateState(int index)
    {
        var cellToUpdate = cellList[index];
        if (!cellToUpdate.IsActive)
            return;

        var leftActive = false;
        var rightActive = false;
        var topActive = false;
        var bottomActive = false;
        if (index > 0 && index % width != 0) // left edges don't have left neighbours
        {
            leftActive = cellList[index - 1].IsActive;
        }

        if (leftActive) // check if right is also active to leave sooner
        {
            if ((index + 1) % width != 0) // right edges don't have right neighbours
            {
                rightActive = cellList[index + 1].IsActive;
            }

            if (leftActive && rightActive) // no need to check other neighbours, this cell is blocked
            {
                cellToUpdate.IsFree = false;
                return;
            }
        }

        // at this point, either left or right neighbour is inactive

        if (index - width >= 0) // top edges don't have top neighbours
        {
            topActive = cellList[index - width].IsActive;
        }

        if (!topActive) // 2 near neighbours are inactive, so the cell is free
        {
            cellToUpdate.IsFree = true;
            return;
        }

        if (index + width < cellList.Count) // bottom edges don't have top neighbours
        {
            bottomActive = cellList[index + width].IsActive;
        }

        cellToUpdate.IsFree = !bottomActive;
    }

    public void UpdateNeighbours(int cellIndex)
    {
        if (cellIndex - 1 >= 0)
            UpdateState(cellIndex - 1);
        if (cellIndex + 1 < cellList.Count)
            UpdateState(cellIndex + 1);
        if (cellIndex - width >= 0)
            UpdateState(cellIndex - width);
        if (cellIndex + width < cellList.Count)
            UpdateState(cellIndex + width);
    }

    public void Reset()
    {
        for(int i = 0; i < cellList.Count; i++)
        {
            cellList[i].Reset();
        }
    }
}


