using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable {
    public int damage;
    public int pushForce;
    protected override void OnCollide(Collider2D coll) {

        // CHANGE THIS- IF ENEMY HITBOX COLLIDES W/ PLAYER OR SUMMON, THEN INITIATE ATTACK METHOD FOR A SPRITE ANIMATION WITH A COLLIDER2D ATTACHED

        Attack();


        if (coll.tag == "Player" || coll.tag == "Summon") {
            // Create a new damage object, before sending it to the player
            Damage dmg = new Damage {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }

    private void Attack() {

        // IF THE COLLIDER2D FROM THE SPRITE COLLIDES WITH THE PLAYER OR SUMMON, THEN SEND DAMAGE
    }
}
