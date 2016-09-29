using UnityEngine;
using System.Collections;


public class AI : Player {
    public bool is_turn = false;

    public AI(int _id) : base(_id)
    {
        base.id = _id;
    }

    public float[] getCoordinates()
    {
        float[] a = new float[3];

        float x = Random.Range(0, 8);
        float y = Random.Range(0, 8);
        float z = Random.Range(0, 16);
        a[0] = x;
        a[1] = y;
        a[2] = z;
        
        return a;
    }

    // Use this for initialization
    void Start () {
	    
	}
	

    
}
