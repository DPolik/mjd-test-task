using System;

public class Cell
{
    private int cellType;
    public int CellType { get => cellType; set => cellType = value; }

    private bool isFree = false;
    public bool IsFree { get => isFree; set => isFree = value; }

    private bool isActive = true;
    public bool IsActive { get => isActive; set => isActive = value; }

    private ICellView cellView;
    public ICellView CellView { get => cellView; set => cellView = value; }

    public int LayerIndex { get; }
    public int CellIndex { get; }
    public Action<Cell> onCellClicked;
    

    public Cell(int cellType, int layerIndex, int cellIndex)
    {
        this.cellType = cellType;
        LayerIndex = layerIndex;
        CellIndex = cellIndex;
    }

    public void OnCellViewClicked()
    {
        onCellClicked?.Invoke(this);
    }

    public void ShowSelected()
    {
        cellView.ShowSelected();
    }

    public void ResetSelected()
    {
        cellView.ResetSelected();
    }

    public void Deactivate()
    {
        cellView.Deactivate();
        isActive = false;
    }

    public void Reset()
    {
        isActive = true;
    }
}
