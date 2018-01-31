using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseOver : MonoBehaviour
{
    public char tileType = '0';
    public Color highlightColor = Color.blue;
    public Color baseColor = Color.red;

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            GetComponent<Renderer>().material.color = highlightColor;
        }
        else
        {
            GetComponent<Renderer>().material.color = baseColor;
        }

    }
}

