using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int m_GridSize;
    public float m_CellRadius;
    public FlowField m_CurFlowField;

    private void InitFlowField()
    {
        m_CurFlowField = new FlowField(m_CellRadius, m_GridSize);
        m_CurFlowField.CreateGrid();
    }

    private void Update()
    {
        // if we click we init the flow field
        if (Input.GetMouseButtonDown(0))
        {
            InitFlowField();
        }
    }

    private void OnDrawGizmos()
    {
        // drawGrid
        DrawGrid(m_GridSize, new Color(0f, 1f, 0f), m_CellRadius);
    }

    private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    {
        Gizmos.color = drawColor;
        for (int x = 0; x < drawGridSize.x; x++)
        {
            for (int y = 0; y < drawGridSize.y; y++)
            {
                Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, 0, drawCellRadius * 2 * y + drawCellRadius);
                Vector3 size = Vector3.one * drawCellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
