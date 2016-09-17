using UnityEngine;
using System.Collections;

public class onTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER");
    }
}
