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

    Vector3 attackCoordinate;

    //GUI
    public GameObject attackCanvas;
    public Text status;  //Currently displays player's turn 

    public GameObject bullet;

	// Use this for initialization
	void Start () {
        status.text = "Turn: Player 1";
        player1 = new Player(1);
        player2 = new Player(2);
        currentPlayerTurn = player1;
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
    }

    public void moveForward()
    {
        if (currentPlayerTurn.actions != 0)
        {
            Ship moving_ship = battlefield.get_selected_ship();

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

}
