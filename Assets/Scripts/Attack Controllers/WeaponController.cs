using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float cooldown = 0.5f;
    private float lastAttack;
    public GameObject weapon;

    protected void Update() {
        if (GameManager.instance.Pause()) {
            return;
        }
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift)) {
            if (Time.time - lastAttack > cooldown) {
                Instantiate(weapon);
                lastAttack = Time.time;
            }
        }
    }
}
