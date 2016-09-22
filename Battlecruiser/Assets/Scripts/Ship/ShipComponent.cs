using UnityEngine;
using System.Collections;

public class ShipComponent : MonoBehaviour {

    // Keeps track if component has been destroyed or not
    public bool hit;

    public bool is_pivot;

    public int vision;

    // Type of component?
    public string weapon_type;

	// Use this for initialization
	void Start () {
        vision = 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
