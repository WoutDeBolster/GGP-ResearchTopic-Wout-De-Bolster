using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridController : MonoBehaviour
{
    public Vector2Int m_GridSize;
    public float m_CellRadius;
    public FlowField m_CurFlowField;

    private bool m_IsStarted = false;

    private void InitFlowField()
    {
        m_CurFlowField = new FlowField(m_CellRadius, m_GridSize);
        m_CurFlowField.CreateGrid();
    }

    private void Update()
    {
        // if we click everything starts
        if (Input.GetMouseButtonDown(0))
        {
            InitFlowField();

            m_CurFlowField.CreateCostField();

            m_IsStarted = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_IsStarted)
        {
            // drawGrid
            DrawGrid(m_GridSize, new Color(0f, 1f, 0f), m_CellRadius);

            // display cost
            foreach (Cell curCell in m_CurFlowField.m_Grid)
            {
                // using the unity editor class
                GUIStyle style = new GUIStyle(GUI.skin.label);
                Handles.Label(curCell.m_WorldPos, curCell.m_Cost.ToString(), style);
            }
        }
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

                //// using the unity editor class
                //GUIStyle style = new GUIStyle(GUI.skin.label);
                //Handles.Label(m_CurFlowField.m_Grid[x,y].m_WorldPos, m_CurFlowField.m_Grid[x, y].m_CostToString(), style);
            }
        }
    }
}
