using UnityEngine;
using System.Collections;

/*
 * A data object storing various information about the cells within the grid. 
 * 
 */
public class Cell : MonoBehaviour {

	public bool occupied;
    // records 0 for not visible, or 1 and 2 to depict the player who can see it on their turn 
	public int visible;
    public ShipComponent current_occupant;
    public int hi;
    public bool highlighted = false;

    public Material when_hit;
	//Various coordinate values of the cell numbered 1 - 6
	private int x; 
	private int y;
	private int z;
    MeshRenderer rend;

    // Use this for initialization
    void Start () {
        rend = gameObject.transform.Find("Cube").transform.GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if(current_occupant != null)
        {
            if (current_occupant.hit)
            {
                rend.material.shader = Shader.Find("Standard");
                rend.material.SetColor("_Color", Color.red);
                rend.material.SetColor("_EmissionColor", Color.red);
            }
            else if (!current_occupant.hit)
            {

                rend.material.shader = Shader.Find("Standard");
                rend.material.SetColor("_Color", Color.blue);
                rend.material.SetColor("_EmissionColor", Color.blue);
            }
        }


    }

    public void toggle_highlight(int newY, int prevY)
    {
        if (newY == y)
        {
            highlighted = true;
        }
        if (prevY == y)
        {
            highlighted = false;
        }
    }

    public void Initialize(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;

        occupied = false;
        visible = 0;
    }
    void OnTriggerExit(Collider ship_component)
    {
       
        if (ship_component.tag == "ShipCollider")
        {
         
            current_occupant = null;
            occupied = false;
        }
    }
    void OnTriggerEnter(Collider ship_comp)
    {

        if (ship_comp.tag == "ShipCollider")
        {
            
            current_occupant = ship_comp.gameObject.GetComponent<ShipComponent>();
            occupied = true;
            GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdatePlayerVision();
            GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdateCellViewValues();
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
