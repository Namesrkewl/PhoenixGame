using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonHitbox : Collidable {

    public float cooldown = 0.5f;
    private float lastAttack;
    public GameObject attack;
    protected override void OnCollide(Collider2D coll) {
        if (coll.tag == "Fighter" && coll.GetComponent<CustomTag>().tags.Contains("Enemy")) {
            Attack(coll);

        };

    }
    private void Attack(Collider2D coll) {
        if (Time.time - lastAttack > cooldown) {
            lastAttack = Time.time;
            attack.transform.position = coll.transform.position;
            Instantiate(attack);
        }
    }
}
