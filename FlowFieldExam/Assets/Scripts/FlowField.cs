using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] m_Grid { get; private set; }
    public float m_CellRadius { get; private set; }
    public Vector2Int m_GridSize { get; private set; }
    public Cell m_DestinationCell;

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
                    hasIncreasedCost = true; // this is to so that you dont increase the cost twice
                }
            }
        }

    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        m_DestinationCell = _destinationCell;
 
        m_DestinationCell.m_Cost = 0;
        m_DestinationCell.m_BestCost = 0;
 
        // i do this with a queue of how it stores the elements and how you can dequeue it
        // more info: https://www.softwaretestinghelp.com/queue-in-cpp/
        Queue<Cell> cellsToCheck = new Queue<Cell>();
 
        cellsToCheck.Enqueue(m_DestinationCell); // filling up the q
    
        while(cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbors = GetNeighborCells(curCell.m_GridIdx, GridDirection.CardinalDirections);
            foreach (Cell curNeighbor in curNeighbors)
            {
                if (curNeighbor.m_Cost == byte.MaxValue) { continue; } // impasible cell check
                if (curNeighbor.m_Cost + curCell.m_BestCost < curNeighbor.m_BestCost) // checking the neiboring cells witch one is the best
                {
                    curNeighbor.m_BestCost = (ushort)(curNeighbor.m_Cost + curCell.m_BestCost); // add the best cost of the 
                    cellsToCheck.Enqueue(curNeighbor);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        foreach(Cell curCell in m_Grid)
        {
            List<Cell> curNeighbors = GetNeighborCells(curCell.m_GridIdx, GridDirection.AllDirections);
 
            int bestCost = curCell.m_BestCost;
 
            // getting the vector of the current cell to the bestCost NeighborCell
            foreach(Cell curNeighbor in curNeighbors)
            {
                if(curNeighbor.m_BestCost < bestCost)
                {
                    bestCost = curNeighbor.m_BestCost;
                    curCell.m_BestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.m_GridIdx - curCell.m_GridIdx);
                }
            }
        }
    }

    private List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighborCells = new List<Cell>();
 
        foreach (Vector2Int curDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;
 
        // to see if it is in the grid
        if (finalPos.x < 0 || finalPos.x >= m_GridSize.x || finalPos.y < 0 || finalPos.y >= m_GridSize.y)
        {
            return null;
        }
 
        else { return m_Grid[finalPos.x, finalPos.y]; }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        // transroming it to gridspace
        float percentX = worldPos.x / (m_GridSize.x * m_CellDiameter);
        float percentY = worldPos.z / (m_GridSize.y * m_CellDiameter);
 
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
 
        int x = Mathf.Clamp(Mathf.FloorToInt((m_GridSize.x) * percentX), 0, m_GridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((m_GridSize.y) * percentY), 0, m_GridSize.y - 1);
        return m_Grid[x, y];
    }
}
