using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class game_controller : MonoBehaviour {

    public int player_turn = 0; //1 = player 1, 2 = player 2
    public Grid battlefield;

    public int gamePhase = 0; // 1 = Ship placement, 2 = mainphase

    public Text status;  //Currently displays player's turn 

    Player player1;
    Player player2;

	// Use this for initialization
	void Start () {
        status.text = "Turn: Player 1";
        player_turn = 1;
        player1 = new Player();
        player2 = new Player();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void turn_change()
    {
        if (player_turn == 1)
        {
            status.text = "Turn: Player 2";
            player_turn = 2;
            player1.moved = true;
            player1.attacked = true;

            player2.moved = false;
            player2.attacked = false;

        }
        else
        {
            status.text = "Turn: Player 1";
            player_turn = 1;
            player1.moved = false;
            player1.attacked = false;

            player2.moved = true;
            player2.attacked = true;
        }
    }

    public void moveForward()
    {
        
        Ship moving_ship = battlefield.get_selected_ship();
        Vector3 forward = moving_ship.transform.forward;
        Debug.Log(forward);
        if (moving_ship != null)
        {
            moving_ship.transform.Translate(1, 0, 0);

            if (player_turn == 1)
            {
                player1.moved = true;
            }
            else
                player2.moved = true;
            {

            }
        }
    }

    // Right: x = 1
    // Left: x = 2
    public void turnShip(int x)
    {
        
        Ship moving_ship = battlefield.get_selected_ship();
        if (moving_ship != null)
        {
            if (x == 1)
            {
                moving_ship.transform.Rotate(0, 90, 0);
            }
            if (x == 2)
            {
                moving_ship.transform.Rotate(0, -90, 0);
            }

            if (player_turn == 1)
            {
                player1.moved = true;
            }
            if (player_turn == 2)
                player2.moved = true;
            {

            }
        }
    }

}
