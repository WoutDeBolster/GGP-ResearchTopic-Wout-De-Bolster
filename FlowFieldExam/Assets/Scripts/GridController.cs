using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum FlowFieldDisplayType 
{ 
    None, 
    CostField, 
    IntegrationField
};

public class GridController : MonoBehaviour
{
    public Vector2Int m_GridSize;
    public float m_CellRadius;
    public FlowField m_CurFlowField;

    private bool m_IsStarted = false;
    public FlowFieldDisplayType m_CurDisplayType;

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

            // making the 0 cell
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = m_CurFlowField.GetCellFromWorldPos(worldMousePos);

            m_CurFlowField.CreateIntegrationField(destinationCell);

            m_CurFlowField.CreateFlowField();

            m_IsStarted = true;
        }
    }
    
    public void ClearCellDisplay()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);

        if (m_IsStarted)
        {
            DrawGrid(m_GridSize, new Color(0f, 1f, 0f), m_CellRadius);
        }

        switch (m_CurDisplayType)
        {
            case FlowFieldDisplayType.CostField:
 
                foreach (Cell curCell in m_CurFlowField.m_Grid)
                {
                    Handles.Label(curCell.m_WorldPos, curCell.m_Cost.ToString(), style);
                }
                break;
                
            case FlowFieldDisplayType.IntegrationField:
 
                foreach (Cell curCell in m_CurFlowField.m_Grid)
                {
                    Handles.Label(curCell.m_WorldPos, curCell.m_BestCost.ToString(), style);
                }
                break;
                
            default:
                break;
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
            }
        }
    }
}
