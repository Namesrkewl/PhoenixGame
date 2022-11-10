using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage structure
    public int[] damage = { 1, 2, 3, 4 };
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.5f };

    // Upgrades
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    // Movement
    public float moveSpeed;
    protected float xSpeed, ySpeed;

    // Variables
    public int maxHits;
    public float duration = 1;
    protected float timeFired;
    protected int hitCount;
    protected List<string> EnemiesHit;

    protected override void Start() {
        if (GetComponent<CustomTag>().tags.Contains("DirectionalFire")) {
            DirectionalFire();
        } else if (GetComponent<CustomTag>().tags.Contains("FireAtCursor")) {
            FireAtCursor();
        }

        // Sets the time you shot
        timeFired = Time.time;

        // Creates a list for enemies hit to keep track of who was hit
        hitCount = 1;
        EnemiesHit = new List<string>();
        EnemiesHit.Add("");

        // Collidable start
        base.Start();
    }

    protected void FixedUpdate() {
        transform.position += new Vector3((1 * xSpeed) * moveSpeed, (1 * ySpeed) * moveSpeed, 0);
        if ((Time.time - timeFired) > duration) {
            Destroy(gameObject);
        }
    }

    protected override void OnCollide(Collider2D coll) {
        if (coll.tag == "Fighter") {
            if (coll.name != "Player") {
                // Puts the name of the enemy hit into the list of enemies hit
                EnemiesHit.Add(coll.name);
                // Checks if the enemy hit was hit before
                for (int i = 0; i < hitCount; i++) {
                    // If the enemy was hit, ignore the rest of this method. If not, continue
                    if (coll.name == EnemiesHit[i]) {
                        EnemiesHit.RemoveAt(i);
                        return;
                    }
                }
                // Increases the hit count
                hitCount++;

                // Create a new damage object, send it to the fighter we've hit
                Damage dmg = new Damage
                {
                    damageAmount = damage[weaponLevel],
                    origin = transform.position,
                    pushForce = pushForce[weaponLevel]
                };

                coll.SendMessage("ReceiveDamage", dmg);

                if (hitCount > maxHits) {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void UpgradeWeapon() {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
    
    public void SetWeaponLevel(int level) {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
    protected virtual void DirectionalFire() {
        Vector3 delta;
        float deltaInRadians, deltaInDegrees, deltaInFirstQuadrant, deltaRelativeToAngle;
        // Positions the object on the player
        transform.position = GameManager.instance.player.transform.position;

        // Gets the mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Gets the distance from the object to the mouse
        delta = new Vector3((mousePos.x - transform.position.x), (mousePos.y - transform.position.y), 0);

        // Converts delta into an arctagent angle in radians
        deltaInRadians = Mathf.Atan(delta.y / delta.x);

        // Converts the arctangent angle from radians to degrees
        deltaInDegrees = ((deltaInRadians / Mathf.PI) * 180);
        if (delta.x < 0) {
            deltaInDegrees += 180;
        } else if (delta.y < 0) {
            deltaInDegrees += 360;
        }

        deltaInFirstQuadrant = ((deltaInDegrees % 180) % 90);
        deltaRelativeToAngle = (deltaInFirstQuadrant % 45);

        // Rotates the sprite based on direction of fire
        transform.Rotate(Vector3.forward * deltaInDegrees);

        // Gives speed values for the object based on the angle of fire
        if (delta.x < 0 && delta.y < 0 || delta.x > 0 && delta.y > 0) {
            if (deltaInFirstQuadrant > 45) {
                ySpeed = 1 + (deltaRelativeToAngle / 45);
                xSpeed = 1 - (deltaRelativeToAngle / 45);
            } else if (deltaInFirstQuadrant < 45) {
                ySpeed = (deltaRelativeToAngle / 45);
                xSpeed = 2 - (deltaRelativeToAngle / 45);
            } else {
                ySpeed = 1f;
                xSpeed = 1f;
            }
        } else if (delta.x < 0 && delta.y > 0 || delta.x > 0 && delta.y < 0) {
            if (deltaInFirstQuadrant > 45) {
                xSpeed = 1 + (deltaRelativeToAngle / 45);
                ySpeed = 1 - (deltaRelativeToAngle / 45);
            } else if (deltaInFirstQuadrant < 45) {
                xSpeed = (deltaRelativeToAngle / 45);
                ySpeed = 2 - (deltaRelativeToAngle / 45);
            } else {
                ySpeed = 1f;
                xSpeed = 1f;
            }
        }


        // Makes values positive or negative based on quadrant of fire
        if (delta.x < 0 && delta.y < 0) {
            xSpeed = xSpeed * -1;
            ySpeed = ySpeed * -1;
        } else if (delta.x < 0) {
            xSpeed = xSpeed * -1;
        } else if (delta.y < 0) {
            ySpeed = ySpeed * -1;
        }
    }
    protected virtual void FireAtCursor() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 delta = new Vector3((mousePos.x - playerPos.x), (mousePos.y - playerPos.y), 0);
        transform.position = playerPos + delta;
    }
}
