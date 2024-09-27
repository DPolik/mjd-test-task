using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardView : MonoBehaviour, IGameBoardView
{
    private CellPool cellPool;
    private List<ICellView> cells;
    private Coroutine rotationCoroutine;

    [SerializeField] private Material[] cellTypes;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float rotateSpeed = 2;

    public void Init(List<Layer> layers, int cellCount, int cellTypesAmount)
    {
        Debug.Assert(cellTypes.Length >= cellTypesAmount);

        cellPool = new CellPool(cellCount, cellPrefab);
        cells = new List<ICellView>(cellCount);
        for(int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            for(int j = 0; j < layer.CellList.Count; j++)
            {
                var cell = layer.CellList[j];
                var cellView = cellPool.GetCell();
                var cellGameObject = cellView.GetGameObject();
                cellGameObject.SetActive(layer.CellList[j].IsActive);
                cellView.Init(cellTypes[cell.CellType]);
                cell.CellView = cellView;
                cellView.AddOnCellViewClickedListener(cell.OnCellViewClicked);

                // calculations to reposition cells to center with the parent (board), for easier rotations
                float posX = j / layer.Width - (layer.Width/2f) + cellView.CellSize; 
                float posY = i - (layers.Count / 2f) + cellView.CellSize;
                float posZ = j % layer.Width - (layer.Height/2f) + cellView.CellSize;

                cellGameObject.transform.SetParent(transform);
                cellGameObject.transform.position = new Vector3(posX, posY, posZ);
                cells.Add(cellView);
            }
        }

        transform.Rotate(Vector3.right, -20); // prettier 3d view of the board
    }

    public void OnDestroy()
    {
        foreach(ICellView cell in cells)
        {
            Destroy(cell.GetGameObject());
        }
    }

    public void Reset(List<Layer> layers)
    {
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            for (int j = 0; j < layer.CellList.Count; j++)
            {
                var cellView = cells[i*layers[i].CellList.Count + j];
                cellView.GetGameObject().SetActive(layer.CellList[j].IsActive);
                cellView.Init(cellTypes[layer.CellList[j].CellType]);
            } 
        }
    }

    public void RotateBoard(int hDirection, Action onFinishRotationCallback = null)
    {
        if(rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(RotateBoardCoroutine(hDirection, onFinishRotationCallback));
        }
    }

    private IEnumerator RotateBoardCoroutine(int hDirection, Action onFinishRotationCallback)
    {
        var startRotation = transform.localRotation;
        var endRotation = transform.localRotation * Quaternion.Euler(0, hDirection * 90, 0);
        var i = 0f;
        while(transform.localRotation != endRotation)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, i);
            i += Time.deltaTime * rotateSpeed;
            yield return 0;
        }

        onFinishRotationCallback?.Invoke();
        rotationCoroutine = null;
    }

    private class CellPool
    {
        private int cellCount;
        private GameObject cellPrefab;
        private List<ICellView> cellPool;

        public CellPool(int cellCount, GameObject cellPrefab)
        {
            this.cellCount = cellCount;
            cellPool = new List<ICellView>(cellCount);

            while (cellCount > 0)
            {
                var cell = UnityEngine.Object.Instantiate(cellPrefab);
                cell.SetActive(false);
                cellPool.Add(cell.GetComponent<ICellView>());
                --cellCount;
            }
        }

        public ICellView GetCell()
        {
            for(int i = 0; i < cellPool.Count; i++)
            {
                if(!cellPool[i].GetGameObject().activeInHierarchy)
                {
                    return cellPool[i];
                }
            }

            return null;
        }
    }
}

