using UnityEngine;

public class Cell
{
    public Vector3 m_WorldPos;
    public Vector2Int m_GridIdx;

    public Cell(Vector3 wordlPos, Vector2Int gridIdx)
    {
        m_WorldPos = wordlPos;
        m_GridIdx = gridIdx;
    }
}
