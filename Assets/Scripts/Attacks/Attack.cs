using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Weapon
{
    protected override void Start() {
        if (GetComponent<CustomTag>().tags.Contains("Summon")) {
            Debug.Log(true);
        }
        if (GetComponent<CustomTag>().tags.Contains("Summon")) {
            Debug.Log(true);
        }
    }
    protected override void Update() {
        
    }
}
