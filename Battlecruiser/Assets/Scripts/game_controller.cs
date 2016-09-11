using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class game_controller : MonoBehaviour {

    public int player_turn = 0; //1 = player 1, 2 = player 2
    public Grid battlefield;
    public List<Ship> ships;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int turn_change()
    {
        if (player_turn == 1)
            player_turn = 2;
        else
            player_turn = 1;
        return player_turn;
    }

}
