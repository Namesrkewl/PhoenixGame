using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitPoint;
    public int maxHitPoint;
    public float pushRecoverySpeed = 0.25f;

    // Immunity Frames
    public float immuneTime = 0.5f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage / Die
    protected virtual void ReceiveDamage(Damage dmg) {
        if (Time.time - lastImmune > immuneTime) {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.up * 100, 0.40f);

            if (hitPoint <= 0) {
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death() {

    }
}
