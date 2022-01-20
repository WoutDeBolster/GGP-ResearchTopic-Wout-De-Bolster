using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] m_Grid { get; private set; }
    public float m_CellRadius { get; private set; }
    public Vector2Int m_GridSize { get; private set; }

    private float m_CellDiameter;

    public FlowField(float cellRadius, Vector2Int gridSize)
    {
        m_CellRadius = cellRadius;
        m_CellDiameter = cellRadius * 2f;
        m_GridSize = gridSize;
    }

    public void CreateGrid()
    {
        m_Grid = new Cell[m_GridSize.x, m_GridSize.y];

        for (int x = 0; x < m_GridSize.y; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                // for each cell define the wold pos and than adding them in the array (cell radius for an offset to get in the middle)
                Vector3 worldPos = new Vector3(m_CellDiameter * x + m_CellRadius, 0, m_CellDiameter * y + m_CellRadius);
                m_Grid[x, y] = new Cell(worldPos, new Vector2Int(x,y));
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * m_CellRadius;
        int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain");
        // going true every cell and if the cell overlaps with a mud ore water block give it the right cost
        foreach (Cell curCell in m_Grid)
        {
            Collider[] obstacles = Physics.OverlapBox(curCell.m_WorldPos, cellHalfExtents, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if (col.gameObject.layer == 8) // water
                {
                    curCell.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9) // mud
                {
                    curCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }

    }
}
