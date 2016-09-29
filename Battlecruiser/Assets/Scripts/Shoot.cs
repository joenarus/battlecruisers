using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour {

    float speed;
    float lerpMoving;
    Vector3 startpos;
    Vector3 endpos;
    bool is_firing;
    public bool reached_dest;
    int player = 0;

	// Use this for initialization
	void Start () {
        is_firing = false;
	}

    public void Initialize(int _speed, Vector3 start, Vector3 end, int _player)
    {
        speed = _speed;
        startpos = start;
        endpos = end;
        is_firing = true;
        lerpMoving = 0;
        player = _player;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Ship"))
        {
            if (player != other.GetComponentInParent<Ship>().player)
            {
                Debug.Log(other.name);
                Debug.Log("HIT");
                 //Temporary until we flip it... REGISTERS HIT TO THE GIVEN SHIP COMPONENT\
                Debug.Log(other.transform.position); //Position of HIT (subtract .5 from x,y, and z to get the correct coordinate)
                if (!other.GetComponent<ShipComponent>().hit)
                {
                    Destroy(transform.gameObject);

                    

                }
                other.GetComponent<ShipComponent>().hit = true;
                GameObject temp = other.transform.parent.gameObject;
                ShipComponent[] a = temp.GetComponentsInChildren<ShipComponent>();
                int count = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    if(a[i].hit)
                    {
                        count++;
                    }
                    if(count == a.Length)
                    {
                        temp.GetComponent<Ship>().can_move = false;
                    }
                }
                GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdatePlayerVision();
                GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdateCellViewValues();
            }

        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(lerpMoving < 1)
        {
            Move_Projectile();
        }
        if(lerpMoving >= 1 && !reached_dest)
        {
            reached_dest = true;
            GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdatePlayerVision();
            GameObject.Find("Game_Controller").GetComponent<game_controller>().UpdateCellViewValues();
        }
	}

    void Move_Projectile()
    {
        lerpMoving += Time.deltaTime;
        transform.position = Vector3.MoveTowards(startpos, endpos, speed * lerpMoving);
    }
}
