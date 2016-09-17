using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour {

	public List<Cell> cells;
    public grid_overlay highlights;

    public GameObject cell;
    public GameObject shipType1;
    public GameObject shipType2;
    public GameObject shipType3;

    // Keeps track of the number of ships placed
    public int ships_placed;

    // Determines if the diagonals are considered neighbors or not
    [SerializeField]
    private bool DiagonalsAreNeighbors = false;

    // Object type used by "find" methods 
    public struct Coordinate
    {
        public int x;
        public int y;
        public int z;

        public Coordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Coordinate))
            {
                return false;
            }

            var coord = (Coordinate)other;

            return Equals(coord);
        }

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        {
            return !c1.Equals(c2);
        }

        override public int GetHashCode()
        {
            return (x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode());
        }
    }

    public Cell[,,] cell_list;

    public List<Ship> ships;

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
                    GameObject newCell = UnityEngine.Object.Instantiate(cell, new Vector3(x,y,z), Quaternion.identity, gameObject.transform) as GameObject;
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

        placeShip(1, 0, 0, 0, "forward", shipType1);
        placeShip(2, 1, 1, 1, "forward", shipType2);
        placeShip(2, 3, 3, 3, "forward", shipType3);



        //ships[0].transform.position = cell_list[0, 0, 0].transform.position;
        //ships[1].transform.position = cell_list[0, 1, 1].transform.position;
    }

    void placeShip(int owner, int posX, int posY, int posZ, string direction, GameObject shipPrefab)
    {
        GameObject newShip = UnityEngine.Object.Instantiate(shipPrefab, new Vector3(posX + 0.5f, posY + 0.5f, posZ + 0.5f), Quaternion.identity, GameObject.Find("ShipYard").transform) as GameObject;
        Ship tempS = newShip.GetComponent<Ship>();
        Vector3 dimensions = shipPrefab.GetComponentInChildren<MeshRenderer>().bounds.size;
        tempS.Initialize(owner, direction, dimensions);
        ships.Add(tempS);
        tempS.ship_number = ships_placed++;
        tempS.name = "Player " + tempS.player + " Number: " + tempS.ship_number;
        /*
        for(int i = 0; i < dimensions.x - .5; i++)
        {
            for(int j = 0; j < dimensions.y - .5; j++)
            {
                for(int k = 0; k < dimensions.z - .5; k ++)
                {
                    // We have to talk about this.
                    cell_list[posX + i, posY + j, posZ + k].occupied = true; //Iterates through dimensions and occupies the proper cells.
                }
            }
        }     */
        //TODO: Add other dimensions to occupied
    }

    public Ship get_selected_ship()
    {
        foreach (Ship a in ships) {
            if (a.selected)
            {
                a.selected = false;
                return a;
            }
        }
        return null;
        
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
        if (x >= x_columns || y >= y_columns || z >= z_columns || x < 0 || y < 0 || z < 0)
            return false;
        return true;
    }

    public IEnumerable<Coordinate> GetShipOccupation(int ship_number)
    {
        HashSet<Coordinate> to_return = new HashSet<Coordinate>();
        foreach(Cell cell in cell_list)
        {
            if(cell.current_occupant != null) 
            {
                if (cell.current_occupant.GetComponentInParent<Ship>().ship_number == ship_number)
                {
                    // Set value of coordinates of ship
                    Coordinate temp = new Coordinate((int)char.GetNumericValue(cell.name[1]), 
                        (int)char.GetNumericValue(cell.name[3]), (int)char.GetNumericValue(cell.name[5]));
                    to_return.Add(temp);
                }
            }
        }
        return to_return;
    }

    public IEnumerable<Coordinate> GetNeighbors(Coordinate coord)
    {
        HashSet<Coordinate> neighbors = new HashSet<Coordinate>();
        if(Contains(coord.x, coord.y, coord.z))
        {
            // Right
            if (Contains(coord.x + 1, coord.y, coord.z))
                neighbors.Add(new Coordinate(coord.x + 1, coord.y, coord.z));
            // Left
            if (Contains(coord.x - 1, coord.y, coord.z))
                neighbors.Add(new Coordinate(coord.x - 1, coord.y, coord.z));
            // Up
            if (Contains(coord.x, coord.y + 1, coord.z))
                neighbors.Add(new Coordinate(coord.x, coord.y + 1, coord.z));
            // Down
            if (Contains(coord.x, coord.y - 1, coord.z))
                neighbors.Add(new Coordinate(coord.x, coord.y - 1, coord.z));
            // Forward
            if (Contains(coord.x, coord.y, coord.z + 1))
                neighbors.Add(new Coordinate(coord.x, coord.y, coord.z + 1));
            // Backward
            if (Contains(coord.x, coord.y, coord.z -1))
                neighbors.Add(new Coordinate(coord.x, coord.y, coord.z -1));

            if(DiagonalsAreNeighbors)
            {
                // Right-Up
                if (Contains(coord.x + 1, coord.y + 1, coord.z))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y + 1, coord.z));
                // Left-Up
                if (Contains(coord.x - 1, coord.y + 1, coord.z))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y + 1, coord.z));
                // Right-Down
                if (Contains(coord.x + 1, coord.y - 1, coord.z))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y - 1, coord.z));
                // Left-Down
                if (Contains(coord.x - 1, coord.y -1, coord.z))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y - 1, coord.z));
                // Right-Forward
                if (Contains(coord.x + 1, coord.y, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y, coord.z + 1));
                // Right-Backward
                if (Contains(coord.x + 1, coord.y, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y, coord.z - 1));
                // Left-Forward
                if (Contains(coord.x - 1, coord.y, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y, coord.z + 1));
                // Left-Backward
                if (Contains(coord.x - 1, coord.y, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y, coord.z - 1));
                // Up-Forward
                if (Contains(coord.x, coord.y + 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x, coord.y + 1, coord.z + 1));
                // Up-Backward
                if (Contains(coord.x, coord.y + 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x, coord.y + 1, coord.z - 1));
                // Down-Forward
                if (Contains(coord.x, coord.y - 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x, coord.y - 1, coord.z + 1));
                // Down-Backward
                if (Contains(coord.x, coord.y - 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x, coord.y - 1, coord.z - 1));
                // Right-Up-Forward
                if (Contains(coord.x + 1, coord.y + 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y + 1, coord.z + 1));
                // Right-Up-Backward
                if (Contains(coord.x + 1, coord.y + 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y + 1, coord.z - 1));
                // Right-Down-Forward
                if (Contains(coord.x + 1, coord.y - 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y - 1, coord.z + 1));
                // Right-Down-Backward
                if (Contains(coord.x + 1, coord.y - 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x + 1, coord.y - 1, coord.z - 1));
                // Left-Up-Forward
                if (Contains(coord.x - 1, coord.y + 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y + 1, coord.z + 1));
                // Left-Up-Backward
                if (Contains(coord.x - 1, coord.y + 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y + 1, coord.z - 1));
                // Left-Down-Forward
                if (Contains(coord.x - 1, coord.y - 1, coord.z + 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y - 1, coord.z + 1));
                // Left-Down-Backward
                if (Contains(coord.x - 1, coord.y - 1, coord.z - 1))
                    neighbors.Add(new Coordinate(coord.x - 1, coord.y - 1, coord.z - 1));
            }
            return neighbors;
        }
        return null;
    }

    public IEnumerable<Coordinate> GetCellsInRange(Coordinate coord, Func<Coordinate, bool> isAsscessable, int range)
    {
        HashSet<Coordinate> to_return = new HashSet<Coordinate>();     
        HashSet<Coordinate> neighbor_list = new HashSet<Coordinate>();

        foreach(Coordinate c in GetNeighbors(coord))
        {
            neighbor_list.Add(c);
        }
        
        for(int i = 0; i < range; i++)
        {
            foreach (Coordinate c in neighbor_list)
            {
                to_return.Add(c);
            }

            neighbor_list.Clear();

            foreach(Coordinate c in neighbor_list)
            {
                neighbor_list.Add(c);
            }
        }
        return to_return;
    }
}