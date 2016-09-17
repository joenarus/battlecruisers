using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class game_controller : MonoBehaviour {

    public Grid battlefield;

    public int gamePhase = 0; // 1 = Ship placement, 2 = mainphase

    Player player1;
    Player player2;

    Player currentPlayerTurn;
    
    // Only type that changes is "hot seat" atm...
    public string game_type;

    Vector3 attackCoordinate;

    //GUI
    public GameObject attackCanvas;
    public Text status;  //Currently displays player's turn 

    public GameObject bullet;

	// Use this for initialization
	void Start () {
        status.text = "Turn: Player 1";
        /*http://answers.unity3d.com/questions/653904/you-are-trying-to-create-a-monobehaviour-using-the-2.html
        player1 = new Player(1);
        player2 = new Player(2);*/
        player1 = gameObject.AddComponent<Player>();
        player1.id = 1;
        player2 = gameObject.AddComponent<Player>();
        player2.id = 2;

        currentPlayerTurn = player1;
        // Firt turn needs actions or they just have to end turn
        currentPlayerTurn.actions = 3;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void turn_change()
    {
        if (currentPlayerTurn.id == 1)
        {
            status.text = "Turn: Player 2";
            
            currentPlayerTurn = player2;
            currentPlayerTurn.actions = 3;

        }
        else
        {
            status.text = "Turn: Player 1";
            currentPlayerTurn = player1;
            currentPlayerTurn.actions = 3;
        }

        // Hides information from player, still needs to disable clicking on ships you can't see and
        //  an inbetween ok button to toggle correctly
        if (game_type == "hot seat")
        {
            Hide_Everything();
            Change_Player_View(currentPlayerTurn.id);
        }
    }

    public void Hide_Everything()
    {
        foreach(Ship ship in battlefield.ships)
        {
            ship.transform.Find("Shape").gameObject.SetActive(false);
        }
    }

    public void Change_Player_View(int player)
    {
        foreach(Ship ship in battlefield.ships)
        {
            if(ship.player == player)
                ship.transform.Find("Shape").gameObject.SetActive(true);
        }
    }
    

    public void moveForward()
    {
        if (currentPlayerTurn.actions != 0)
        {
            Ship moving_ship = battlefield.get_selected_ship();

            if (Can_Move(new Vector3(0, 0, 1), moving_ship))
            {
                if (moving_ship != null)
                {
                    Vector3 forward = moving_ship.transform.forward; //Grabs the direction //basically useless, but may come in handy 
                    if (moving_ship.player == currentPlayerTurn.id)
                    {
                        moving_ship.transform.Translate(0, 0, 1); //Always moves in the Z-axis

                        currentPlayerTurn.actions--;
                    }
                    else
                    {
                        moving_ship.ship_information.text = "That is not your ship.";
                    }
                }
            }
        }
    }

    // Right: x = 1
    // Left: x = 2
    public void turnShip(int x)
    {
        if (currentPlayerTurn.actions != 0)
        {
            Ship moving_ship = battlefield.get_selected_ship();
            if (moving_ship != null)
            {
                if (moving_ship.player == currentPlayerTurn.id)
                {
                    if (x == 1)
                    {
                        moving_ship.transform.Rotate(0, 90, 0);
                    }
                    if (x == 2)
                    {
                        moving_ship.transform.Rotate(0, -90, 0);
                    }

                    currentPlayerTurn.actions--;

                }
                else
                {
                    moving_ship.ship_information.text = "That is not your ship.";
                }

            }
        }
    }
    //x = 1: Mine
    //x = 2: Probe
    public void Attack(int x)
    {
        attackCanvas.SetActive(true);
        if (currentPlayerTurn.actions != 0)
        {
            Ship attack_ship = battlefield.get_selected_ship();
            if (attack_ship != null && attack_ship.player == currentPlayerTurn.id)
            {
                attackCoordinate = attack_ship.transform.position;
                Debug.Log(attackCoordinate);
            }
        }
        else
        {
            //TODO: NO more actions allowed this turn
        }
    }

    public void LaunchAttack()
    {
        Text xcoordinate = GameObject.Find("X_attack").GetComponentInChildren<Text>();
        Text ycoordinate = GameObject.Find("Y_attack").GetComponentInChildren<Text>();
        Text zcoordinate = GameObject.Find("Z_attack").GetComponentInChildren<Text>();
        Debug.Log(xcoordinate.text + " " + ycoordinate.text + " " + zcoordinate.text);
        int x = -1;
        int y = -1;
        int z = -1;

        int.TryParse(xcoordinate.text, out x);
        int.TryParse(ycoordinate.text, out y);
        int.TryParse(zcoordinate.text, out z);

        if(battlefield.Contains(x,y,z))
        {

            GameObject temp = Object.Instantiate(bullet, new Vector3(), Quaternion.identity, gameObject.transform) as GameObject;

            temp.GetComponent<Shoot>().Initialize(10,attackCoordinate, new Vector3(x + .5f,y+.5f,z+.5f), currentPlayerTurn.id);
            temp.name = "currentlyActiveMine";
        }
        else
        {
            //TODO: "COORDINATES ARE NOT IN RANGE"
        }
        attackCanvas.SetActive(false);
    }
    public bool Can_Move(Vector3 destination, Ship ship)
    {
        // Check each moving block
        foreach(Grid.Coordinate coord in battlefield.GetShipOccupation(ship.ship_number))
        {
            // Destination
            Grid.Coordinate moving_to = new Grid.Coordinate(coord.x + (int)destination.x, coord.y + (int)destination.y, coord.z + (int)destination.z);
            // Check if destination exists on Grid: in case it might move off the grid
            if (battlefield.Contains(moving_to.x, moving_to.y, moving_to.z))
            {
                // Check if ship moving does not bump into a component of a different ship
                if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant != null)
                {
                    // Check if ship number is the same, if not illegal move would happen.
                    if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant.GetComponentInParent<Ship>().ship_number != ship.ship_number)
                        return false;
                }
                // If Cell doesn't have another ship component, still check if it is occupied: Maybe a mine or a prob for instance
                if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].occupied == true && battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant == null)
                    return false;
            }
            else
                return false;
        }
        return true;
    }
}
