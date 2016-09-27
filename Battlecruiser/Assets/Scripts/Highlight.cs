using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {
    public Camera mainCam;
	// Use this for initialization
	void Start () {
        mainCam = GameObject.Find("Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnMouseOver()
    {
        
    }
   
}
