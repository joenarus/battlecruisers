using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ship : MonoBehaviour {

    List<ShipComponent> ship_components;
    [SerializeField]
    private Text ship_information;

    // Bool: Ship able to move
    private bool can_move;

    // Bool: Ship able to attack
    private bool can_attack;

    // Int: The player who owns the ship
    public int player;

    // string: The way the ship is facing
    public string ship_facing;

    // Ship's dimensions
    struct coordinate
    {
        int x, y, z;
    }

    private coordinate ship_dimensions;

    coordinate Ship_Dimensions
    { get { return ship_dimensions; } }


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

    // Click a ship
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ship_information.text = "This ship belongs to player " + player;
        }
    }


	// Update is called once per frame
	void Update () {

	}
}
