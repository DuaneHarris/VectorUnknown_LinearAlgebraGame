using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour {

    public int columns = 5;
    public int rows = 5;

    public GameObject tile;
    public Transform gridHolder;

	// Use this for initialization
	void Start () {
        InitializeGrid();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeGrid()
    {
        gridHolder = new GameObject("grid").transform;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject instance = Instantiate(tile, new Vector3(i, 0f, j), Quaternion.identity) as GameObject;

                instance.transform.SetParent(gridHolder);
            }
        }
    }
}
