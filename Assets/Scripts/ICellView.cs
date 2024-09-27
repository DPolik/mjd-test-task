using System;
using UnityEngine;

public interface ICellView
{
    float CellSize
    {
        get;
    }

    void Init(Material mat);

    void ShowSelected();

    void ResetSelected();

    void Deactivate();

    GameObject GetGameObject();

    void AddOnCellViewClickedListener(Action listener);

    void RemoveOnCellViewClickedListener(Action listener);
}
