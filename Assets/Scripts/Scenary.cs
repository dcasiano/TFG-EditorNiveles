using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenary : MonoBehaviour
{
    [SerializeField]
    private int totalColumns = 20;
    [SerializeField]
    private int totalRows = 10;
    public const float gridSize = 1.0f;

    private readonly Color normalColor = Color.grey;
    private readonly Color selectedColor = Color.Lerp(Color.cyan, Color.blue, 0.5f);
    private readonly Color selectedFrameColor = Color.blue;
    public int TotalColumns
    {
        get { return totalColumns; }
        set { totalColumns = value; }
    }
    public int TotalRows
    {
        get { return totalRows; }
        set { totalRows = value; }
    }

    // Draws a rectangle which represents the frame of the grid
    private void GridFrameGizmo(int cols, int rows)
    {
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, rows * gridSize, 0));
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(cols * gridSize, 0, 0));
        Gizmos.DrawLine(new Vector3(cols * gridSize, 0, 0), new Vector3(cols * gridSize, rows * gridSize, 0));
        Gizmos.DrawLine(new Vector3(0, rows * gridSize, 0), new Vector3(cols * gridSize, rows * gridSize, 0));
    }

    // Draws the vertical and horizontal lines of the grid
    private void GridGizmo(int cols, int rows)
    {
        for (int i = 1; i < cols; i++)
            Gizmos.DrawLine(new Vector3(i * gridSize, 0, 0), new Vector3(i * gridSize, rows * gridSize, 0));
        
        for (int i = 1; i < rows; i++)
            Gizmos.DrawLine(new Vector3(0, i * gridSize, 0), new Vector3(cols * gridSize, i * gridSize, 0));
    }

    private void OnDrawGizmos()
    {
        Color prevColor = Gizmos.color;

        Gizmos.color = normalColor;
        GridGizmo(totalColumns, totalRows);
        GridFrameGizmo(totalColumns, totalRows);

        Gizmos.color = prevColor;
    }

    private void OnDrawGizmosSelected()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = selectedFrameColor;
        GridFrameGizmo(totalColumns, totalRows);
        Gizmos.color = selectedColor;
        GridGizmo(totalColumns, totalRows);

        Gizmos.color = oldColor;
    }

    // Converts Unity world coordinates to our grid coordinates. We assume z world coordinate is zero
    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(
        (int)((point.x - transform.position.x) / gridSize),
        (int)((point.y - transform.position.y) / gridSize), 0.0f);
        return gridPoint;
    }

    // Converts our grid coordinates to Unity world coordinates. We assume z world coordinate is zero
    public Vector3 GridToWorldCoordinates(int col, int row)
    {
        Vector3 worldPoint = new Vector3(
        transform.position.x + (col * gridSize + gridSize / 2.0f),
        transform.position.y + (row * gridSize + gridSize / 2.0f), 0.0f);
        return worldPoint;
    }

    public bool IsInsideGridBounds(Vector3 point)
    {
        float minX = transform.position.x;
        float maxX = minX + totalColumns * gridSize;
        float minY = transform.position.y;
        float maxY = minY + totalRows * gridSize;
        return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
    }

    public bool IsInsideGridBounds(int col, int row)
    {
        return (col >= 0 && col < totalColumns && row >= 0 && row < totalRows);
    }
}