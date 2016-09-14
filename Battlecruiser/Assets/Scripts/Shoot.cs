﻿using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    float speed;
    float lerpMoving;
    Vector3 startpos;
    Vector3 endpos;
    bool is_firing;
    bool reached_dest;
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
            Ship s = other.GetComponent<Ship>();
            if (s.player != player)
            {
                s.Hit(transform.position);
                Debug.Log("HIT");
                Destroy(transform.gameObject);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(lerpMoving < 1)
        {
            Move_Projectile();
        }
        
	}

    void Move_Projectile()
    {
        lerpMoving += Time.deltaTime;
        transform.position = Vector3.MoveTowards(startpos, endpos, speed * lerpMoving);
    }
}