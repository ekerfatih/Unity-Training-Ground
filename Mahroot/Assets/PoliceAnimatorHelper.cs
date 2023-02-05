using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAnimatorHelper : MonoBehaviour {

    
    public void Close() {
        GetComponent<Animator>().SetBool("button", false);
    }
}
