using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GenerateGrid : MonoBehaviour {

    public int rows = 5;
    public int columns = 5;

    public char[,] tileMapped;
    public GameObject[,] tileGrid;

    public GameObject tile;

    public Transform gridHolder;

    public TextAsset textInput;

    public Camera mainCamera;

	// Use this for initialization
	void Start () {
        tileMapped = new char[rows, columns];
        ReadTextLevels();
        InitializeGrid();

        mainCamera.transform.position = new Vector3(rows/2,rows,columns/2);
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
                GameObject instance = Instantiate(tile, 
                    new Vector3(i, 0f, j), Quaternion.identity) as GameObject;

                instance.transform.SetParent(gridHolder);

                TileMouseOver tileData = instance.GetComponent<TileMouseOver>();
                char type = tileMapped[i, j];

                tileData.tileType = type;

                // setting tile colors
                if (tileData.tileType == '0')
                {
                    tileData.baseColor = Color.black;
                }
                else if (tileData.tileType == 't')
                {
                    tileData.baseColor = Color.gray;
                }
                else if (tileData.tileType == 'g')
                {
                    tileData.baseColor = Color.green;
                }

            }
        }
    }

    void ReadTextLevels()
    {
        string text = textInput.text;

        // remove newlines and returns

        for (int i = text.Length-1; i >= 0; i--)
        {
            if (text[i] == '\n' || text[i] == '\r')
            {
                string newText = text.Remove(i, 1);
                text = newText;
            }
            
        }

        // convert text into 2D array
        // must be correct size

        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                tileMapped[i, j] = text[index];
                index++;
            }
        }

        Debug.Log(tileMapped.ToString());
    }
}
