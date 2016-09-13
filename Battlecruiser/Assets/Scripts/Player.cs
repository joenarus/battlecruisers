using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private bool _moved;
    private bool _attacked;
    private List<Ship> _ships;

    public bool moved
    {
        get { return _moved; }
        set { _moved = value; }
    }

    public bool attacked
    {
        get { return _attacked; }
        set { _attacked = value; }
    }

    private List<Ship> ships
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
