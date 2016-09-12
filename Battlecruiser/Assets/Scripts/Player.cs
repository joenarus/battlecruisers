using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public bool moved
    {
        get { return moved; }
        set { moved = value; }
    }

    public bool attacked
    {
        get { return attacked; }
        set { attacked = value; }
    }

    private List<Ship> ships
    {
        get { return ships; }
        set { ships = value; }
    }

    public void addShip(Ship s)
    {
        ships.Add(s);
    }

	// Use this for initialization
	void Start () {
        ships = new List<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}






}
