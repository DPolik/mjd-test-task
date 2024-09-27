using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CellView : MonoBehaviour, ICellView
{
    private Renderer myRenderer;

    [SerializeField] private float cellSize = 0.5f;
    public float CellSize { get => cellSize; }

    private Action onCellViewClicked;

    public void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    public void Init(Material mat)
    {
        myRenderer.sharedMaterial = mat;
    }

    private void OnMouseUpAsButton()
    {
        onCellViewClicked?.Invoke();
    }

    public void ShowSelected()
    {
        transform.localScale *= 0.8f;
    }

    public void ResetSelected()
    {
        transform.localScale = Vector3.one;
    }

    public void Deactivate()
    {
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void AddOnCellViewClickedListener(Action listener)
    {
        onCellViewClicked += listener;
    }

    public void RemoveOnCellViewClickedListener(Action listener)
    {
        onCellViewClicked -= listener;
    }
}
