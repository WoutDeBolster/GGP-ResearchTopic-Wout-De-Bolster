using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum FlowFieldDisplayType 
{ 
    None, 
    AllIcons, 
    DestinationIcon, 
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

    private Sprite[] ffIcons;

    private void Start()
    {
        ffIcons = Resources.LoadAll<Sprite>("Sprites/FFicons");
    }

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

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = m_CurFlowField.GetCellFromWorldPos(worldMousePos);

            m_CurFlowField.CreateIntegrationField(destinationCell);

            m_CurFlowField.CreateFlowField();
            DrawFlowField();

            m_IsStarted = true;
        }
    }
    
    // DRAWING PART
    // ************
    public void DrawFlowField()
    {
        ClearCellDisplay();
 
        switch (m_CurDisplayType)
        {
            case FlowFieldDisplayType.AllIcons:
                DisplayAllCells();
                break;
 
            case FlowFieldDisplayType.DestinationIcon:
                DisplayDestinationCell();
                break;
 
            default:
                break;
        }
    }
 
    private void DisplayAllCells()
    {
        if (m_CurFlowField == null) { return; }
        foreach (Cell curCell in m_CurFlowField.m_Grid)
        {
            DisplayCell(curCell);
        }
    }
 
    private void DisplayDestinationCell()
    {
        if (m_CurFlowField == null) { return; }
        DisplayCell(m_CurFlowField.m_DestinationCell);
    }
 
    // sprites
    private void DisplayCell(Cell cell)
    {
        GameObject iconGO = new GameObject();
        SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
        iconGO.transform.parent = transform;
        iconGO.transform.position = cell.m_WorldPos;
 
        if (cell.m_Cost == 0)
        {
            iconSR.sprite = ffIcons[3];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_Cost == byte.MaxValue)
        {
            iconSR.sprite = ffIcons[2];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.North)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.South)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.East)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.West)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.NorthEast)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.NorthWest)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.SouthEast)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.m_BestDirection == GridDirection.SouthWest)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
        }
        else
        {
            iconSR.sprite = ffIcons[0];
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
            // drawGrid
            DrawGrid(m_GridSize, new Color(0f, 1f, 0f), m_CellRadius);

            // // display cost
            // foreach (Cell curCell in m_CurFlowField.m_Grid)
            // {
            //    // using the unity editor class
            //    Handles.Label(curCell.m_WorldPos, curCell.m_Cost.ToString(), style);
            // }
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

                //// using the unity editor class
                //GUIStyle style = new GUIStyle(GUI.skin.label);
                //Handles.Label(m_CurFlowField.m_Grid[x,y].m_WorldPos, m_CurFlowField.m_Grid[x, y].m_CostToString(), style);
            }
        }
    }
}
