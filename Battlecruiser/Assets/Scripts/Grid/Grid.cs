using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public List<Cell> cells;
    public grid_overlay highlights;

    public GameObject cell;

	//Stores how big the grid is
	public int x_columns = 1;
	public int y_columns = 1;
	public int z_columns = 1;

	// Use this for initialization
	void Start () {
        highlights.gridSizeX = x_columns;
        highlights.gridSizeY = y_columns;
        highlights.gridSizeZ = z_columns;

        for(int i = 0; i < x_columns; i ++)
        {
            for (int j = 0; j < y_columns; j++)
            {
                for (int k = 0; k < z_columns; k++)
                {
                    float x = i + .5f;
                    float y = j + .5f;
                    float z = k + .5f;
                   GameObject newCell = Object.Instantiate(cell, new Vector3(x,y,z), Quaternion.identity) as GameObject;
                   Cell temp = newCell.GetComponent<Cell>();
                   temp.Initialize(i, j, k);
                }
            }
        }



	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void set_x_columns(int _x)
    {
        x_columns = _x;
    }
    public int get_x_colums()
    {
        return x_columns;
    }
    public void set_y_columns(int _y)
    {
        y_columns = _y;
    }
    public int get_y_colums()
    {
        return y_columns;
    }
    public void set_z_columns(int _z)
    {
        z_columns = _z;
    }
    public int get_z_colums()
    {
        return z_columns;
    }

}
