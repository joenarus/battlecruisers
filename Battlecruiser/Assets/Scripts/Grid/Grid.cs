using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour {

	public List<Cell> cells;

    public GameObject cell;
    public GameObject shipType1;
    public GameObject shipType2;
    public GameObject shipType3;
    public GameObject shipType4;

    public GameObject gridPlane;
    public Camera maincam;

    public GameObject selected_level;

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

        public bool Equals(Coordinate other)
        {
            bool areEqual = (x == other.x) && (y == other.y) && (z == other.z);
            return areEqual;
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

        override public int GetHashCode()
        {
            return (x ^ y ^ z);
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

        placeShip(1, 4, 4, 4, "forward", shipType1);
        placeShip(1, 3, 2, 5, "forward", shipType2);
        placeShip(1, 4, 6, 3, "forward", shipType3);
        placeShip(1, 3, 0, 0, "forward", shipType4);

        placeShip(2, 3, 3, 8, "backward", shipType1);
        placeShip(2, 3, 2, 9, "backward", shipType2);
        placeShip(2, 4, 6, 10, "backward", shipType3);
        placeShip(2, 5, 5, 13, "backward", shipType4);

    }

    void placeShip(int owner, int posX, int posY, int posZ, string direction, GameObject shipPrefab)
    {
        GameObject newShip = UnityEngine.Object.Instantiate(shipPrefab, new Vector3(posX + 0.5f, posY + 0.5f, posZ + 0.5f), Quaternion.identity, GameObject.Find("ShipYard").transform) as GameObject;
        Ship tempS = newShip.GetComponent<Ship>();
        Vector3 dimensions = shipPrefab.GetComponentInChildren<MeshRenderer>().bounds.size;
        if(direction == "backward")
        {
            newShip.transform.Rotate(0,180,0);
        }
        if(direction == "right")
        {
            newShip.transform.Rotate(0, 90, 0);
        }
        if (direction == "left")
        {
            newShip.transform.Rotate(0, -90, 0);
        }
        tempS.Initialize(owner, dimensions);
        ships.Add(tempS);
        tempS.ship_number = ships_placed++;
        tempS.name = "Player " + tempS.player + " Number: " + tempS.ship_number;
        
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

        if (Input.GetKeyDown("up"))
            if (gridPlane.transform.position.y < 7)
            {
                float prev_level = gridPlane.transform.position.y;
                gridPlane.transform.position += Vector3.up;
               // highlight_level(gridPlane.transform.position.y, prev_level);
            }
        if (Input.GetKeyDown("down"))
            if (gridPlane.transform.position.y > 0)
                gridPlane.transform.position += Vector3.down;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (gridPlane.transform.position.y < 7)
                gridPlane.transform.position += Vector3.up;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (gridPlane.transform.position.y > 0)
                gridPlane.transform.position += Vector3.down;
        }
        
    }
    // Properties?
    /* Ex:
    public int X_Columns
    {
        get { return x_columns; }
        set { x_colums = value; }
    }
    */

    public void highlight_level(float grid_level, float previous_level)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 16; j++) {
                string name = "(" + i + "," + grid_level + "," + j + ")";
                GameObject tempCell = GameObject.Find("(" + i + "," + grid_level + ","+j + ")");
                Debug.Log(tempCell.name);
                
                Renderer temp_cell_renderer = tempCell.GetComponentInChildren<Renderer>();
                Debug.Log(temp_cell_renderer);
                temp_cell_renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                //selected_ship_render = selected_ship.GetComponentInChildren<Renderer>();
                //selected_ship_render.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            }
        }
        
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
                    Coordinate temp = new Coordinate((int)cell.transform.position.x, 
                        (int)cell.transform.position.y, (int)cell.transform.position.z);
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

    public IEnumerable<Coordinate> GetCellsInRange(Coordinate coord, int range)
    {
        HashSet<Coordinate> to_return = new HashSet<Coordinate>();
        Queue<Coordinate> neighbor_list = new Queue<Coordinate>();

        neighbor_list.Enqueue(coord);
        while(neighbor_list.Count > 0)
        {
            var current = neighbor_list.Dequeue();
            to_return.Add(current);
            range--;
            foreach(Coordinate c in GetNeighbors(current))
            {
                to_return.Add(c);
                if(range > 0)
                    neighbor_list.Enqueue(c);
            }        
        }
        return to_return;
    }
}