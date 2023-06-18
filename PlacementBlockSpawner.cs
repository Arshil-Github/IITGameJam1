using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBlockSpawner : MonoBehaviour
{
    public int rows;
    public int cols;
    public float cellSize = 2;
    public GameObject pf_gridcell;
    public GameObject cellHolder;

    public GameObject originOfGrid;
    public void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject cell = Instantiate(pf_gridcell, cellHolder.transform);

                float posX = col * cellSize;
                float posY = row * cellSize;

                cell.transform.localScale = new Vector3(cellSize, cellSize, cell.transform.localScale.z);

                cell.transform.position = new Vector2(posX + originOfGrid.transform.position.x, posY + originOfGrid.transform.position.y);
            }
        }
    }
}
