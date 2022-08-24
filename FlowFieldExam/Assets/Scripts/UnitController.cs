using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GridController m_GridController;
    public GameObject m_unitPrefab;
    public int m_NumUnitsPerSpawn;
    public float m_MoveSpeed;
 
    private List<GameObject> m_UnitsInGame;
 
    private void Awake()
    {
        m_UnitsInGame = new List<GameObject>();
    }
 
    void Update()
    {
        // key inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnits();
        }
 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DestroyUnits();
        }
    }
 
    private void FixedUpdate()
    {
        if (m_GridController.m_CurFlowField == null) 
        { 
            return;
        }
        // getting the pos of the cell the unit is standing on and then moving in the direction of the best cell
        foreach (GameObject unit in m_UnitsInGame)
        {
            Cell cellBelow = m_GridController.m_CurFlowField.GetCellFromWorldPos(unit.transform.position);
            Vector3 moveDirection = new Vector3(cellBelow.m_BestDirection.m_Vector.x, 0, cellBelow.m_BestDirection.m_Vector.y);
            Rigidbody unitRB = unit.GetComponent<Rigidbody>();
            unitRB.velocity = moveDirection * m_MoveSpeed;
        }
    }
 
    private void SpawnUnits()
    {
        // spawinging them randomly on the grid and not on the water
        Vector2Int gridSize = m_GridController.m_GridSize;
        float nodeRadius = m_GridController.m_CellRadius;
        Vector2 maxSpawnPos = new Vector2(gridSize.x * nodeRadius * 2 + nodeRadius, gridSize.y * nodeRadius * 2 + nodeRadius);
        int colMask = LayerMask.GetMask("Impassible", "Units");
        Vector3 newPos;
        for (int i = 0; i < m_NumUnitsPerSpawn; i++)
        {
            GameObject newUnit = Instantiate(m_unitPrefab);
            newUnit.transform.parent = transform;
            m_UnitsInGame.Add(newUnit);
            do
            {
                newPos = new Vector3(Random.Range(0, maxSpawnPos.x), 0, Random.Range(0, maxSpawnPos.y));
                newUnit.transform.position = newPos;
            }
            while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0); // makin sure they dont spawn on water
        }
    }
 
    private void DestroyUnits()
    {
        foreach (GameObject go in m_UnitsInGame)
        {
            Destroy(go);
        }
        m_UnitsInGame.Clear();
    }
}
