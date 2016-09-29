using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ship : MonoBehaviour {

    [SerializeField]
    public ShipComponent[] ship_components;

    [SerializeField]
    public Text ship_information;

    [SerializeField]
    public coordinate[] ship_dimensions;

    // Bool: Ship able to move
    public bool can_move = true;

    // Bool: Ship able to attack
    private bool can_attack;

    // Int: The player who owns the ship
    public int player;

    // Int: The number of the ship 
    public int ship_number;

    // string: The way the ship is facing
    public string ship_facing;

    public bool can_select;

    public bool selected = false;

    //Stores the size of the object
    public Vector3 size;

    public GameObject shipInfoCanvas;

    public int health = 100; //Temp thing for debugging attack;


    // Ship's dimensions
    public struct coordinate
    {
        int x, y, z;
    }

    // Ship's Move property
    bool Can_Move
    {
        get { return can_move; }
        set { can_move = value;  }
    }

    // Ship's Attack property
    bool Can_Attack
    {
        get { return can_attack; }
        set { can_attack = value; }
    }

    // Have ship give grid requested attack
    public void Attack(ShipComponent weapon)
    {
        // how do we know which weapon attacks? 
       
        // Select weapon from grid in UI -Joe       
    }

    //Ships control when they get hit
    public void Hit(Vector3 hit_pos)
    {
        Debug.Log(hit_pos);
    }

    // Click a ship
    public void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(0))
        
            //shipInfoCanvas.SetActive(true);

          //  ship_information.text = "This ship belongs to player " + player;
            //selected = true;
        
    }

    public void Initialize(int _player, Vector3 ship)
    {
        //shipInfoCanvas = GameObject.Find()
        size = ship;
        player = _player;
       // ship_facing = _facing;
        selected = false;
        can_move = true;
        ship_information = GameObject.FindGameObjectWithTag("ShipInfoText").GetComponent<Text>();
        
    }

    public void Attack()
    {
       
    }


	// Update is called once per frame
	void Update () {

	}
}
