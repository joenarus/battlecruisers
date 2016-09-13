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
        }
        else
        {
            status.text = "Turn: Player 1";
            player_turn = 1;
        }
    }

    public void moveForward()
    {
        
        Ship moving_ship = battlefield.get_selected_ship();
        if (moving_ship != null)
            moving_ship.transform.Translate(1,0,0);
    }

}
