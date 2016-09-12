using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public List<Cell> cells;
    public grid_overlay highlights;

    public GameObject cell;

    public Cell[,,] cell_list;

    [SerializeField]
    private Ship[] ships;

	//Stores how big the grid is
	public int x_columns = 1;
	public int y_columns = 1;
	public int z_columns = 1;

	// Use this for initialization
	void Start () {
        highlights.gridSizeX = x_columns;
        highlights.gridSizeY = y_columns;
        highlights.gridSizeZ = z_columns;

        // list of all the cells
        cell_list = new Cell[x_columns, y_columns, z_columns];

        for (int i = 0; i < x_columns; i ++)
        {
            for (int j = 0; j < y_columns; j++)
            {
                for (int k = 0; k < z_columns; k++)
                {
                    float x = i + .5f;
                    float y = j + .5f;
                    float z = k + .5f;
                    GameObject newCell = Object.Instantiate(cell, new Vector3(x,y,z), Quaternion.identity, gameObject.transform) as GameObject;
                    Cell temp = newCell.GetComponent<Cell>();
                    temp.Initialize(i, j, k);
                    temp.gameObject.name = "(" + i + "," + j + "," + k + ")";
                    //cell_list is new as of 9/11/16. Contains a list of all the cells
                    cell_list[i, j, k] = temp;
                }
            }
        }
        // Placement of ships
        // Example of how ships can be placed
        ships[0].transform.position = cell_list[0, 0, 0].transform.position;
        ships[1].transform.position = cell_list[0, 1, 1].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Properties?
    /* Ex:
    public int X_Columns
    {
        get { return x_columns; }
        set { x_colums = value; }
    }
    */

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

    // returns whether or not a cell is in the range of the grid
    // Contains(What to pass in?  just 3 ints? or an struct of 3 ints?)
    public bool Contains(int x, int y, int z)
    {
        if (x >= x_columns || y >= y_columns || z >= z_columns)
            return false;
        return true;
    }
}