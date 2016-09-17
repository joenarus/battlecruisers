using UnityEngine;
using System.Collections;

/*
 * A data object storing various information about the cells within the grid. 
 * 
 */
public class Cell : MonoBehaviour {

	public bool occupied; 
	public bool visible;
    public ShipComponent current_occupant;
    public int hi;

	//Various coordinate values of the cell numbered 1 - 6
	private int x; 
	private int y;
	private int z;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;

        occupied = false;
        visible = false;
    }
    void OnTriggerExit(Collider ship_component)
    {
        if(ship_component.tag == "ShipCollider")
        {
            current_occupant = null;
            occupied = false;
        }
    }
    void OnTriggerEnter(Collider ship_comp)
    {
        if(ship_comp.tag == "ShipCollider")
        {
            current_occupant = ship_comp.gameObject.GetComponent<ShipComponent>();
            occupied = true;
        }
    }

	public int get_x() {
		return x;
	}

	public int get_y() {
		return y;
	}

	public int get_z() {
		return z;
	}

}
