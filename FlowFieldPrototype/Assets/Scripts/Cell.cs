using UnityEngine;

public class Cell
{
    public Vector3 m_WorldPos;
    public Vector2Int m_GridIdx;
    public byte m_Cost;

    public Cell(Vector3 wordlPos, Vector2Int gridIdx)
    {
        m_WorldPos = wordlPos;
        m_GridIdx = gridIdx;
        m_Cost = 1;
    }

    public void IncreaseCost(int amount)
    {
        if (m_Cost == byte.MaxValue)
        {
            return;
        }
        else if (m_Cost + amount > 255)
        {
            m_Cost = byte.MaxValue;
        }
        else
        {
            m_Cost += (byte)amount;
        }
    }
}
