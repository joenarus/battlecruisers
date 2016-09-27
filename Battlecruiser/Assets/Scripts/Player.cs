using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private int _actions = 3;
    private List<Ship> _ships;
    public int id;

    public Player(int _id)
    {
        id = _id;
    }

    public int actions
    {
        get { return _actions; }
        set { _actions = value; }
    }
    
    public List<Ship> ships
    {
        get { return _ships; }
        set { _ships = value; }
    }

    public void addShip(Ship s)
    {
        _ships.Add(s);
    }

	// Use this for initialization
	void Start () {
        _ships = new List<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
