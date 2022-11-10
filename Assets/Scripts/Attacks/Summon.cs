using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Summon : Enemy {
    protected override void Start() {
        if (GetComponent<CustomTag>().tags.Contains("DirectionalFire")) {
            DirectionalFire();
        } else if (GetComponent<CustomTag>().tags.Contains("FireAtCursor")) {
            FireAtCursor();
        }
        base.Start();
        enemiesList = new List<EnemyContainer>();
        sortedEnemiesList = new List<EnemyContainer>();
    }

    private GameObject[] enemyScan;
    private List<EnemyContainer> enemiesList;
    private List<EnemyContainer> sortedEnemiesList;
    private EnemyContainer targetEnemies;
    private bool collidingWithEnemy;

    protected override void FixedUpdate() {
        EnemyScan();
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
    protected virtual void EnemyScan() {
        if (GameObject.FindGameObjectWithTag("Fighter") != null) {
            // Gets a list of enemy objects, their positions, and their distance from the player
            enemyScan = GameObject.FindGameObjectsWithTag("Fighter");
            for (int i = 0; i < enemyScan.Length; i++) {
                targetEnemies = new EnemyContainer();
                targetEnemies.enemies = enemyScan[i];
                targetEnemies.enemyPosition = enemyScan[i].transform.position;
                targetEnemies.enemyDistanceFromPlayer = Vector3.Distance(playerTransform.position, enemyScan[i].transform.position);
                targetEnemies.enemyHitBox = enemyScan[i].GetComponent<BoxCollider2D>();
                enemiesList.Add(targetEnemies);
            }
            // Sorts those values by distance from the player
            sortedEnemiesList = enemiesList.OrderBy(targetEnemies => targetEnemies.enemyDistanceFromPlayer).ToList();

            // Are enemies in range?
            if (sortedEnemiesList[0].enemyDistanceFromPlayer < chaseLength) {
                chasing = true;
            }
            if (chasing && !collidingWithEnemy) {
                UpdateMotor((sortedEnemiesList[0].enemyPosition - transform.position).normalized);
            } else if (collidingWithEnemy) {

            } else {
                UpdateMotor(startingPosition - transform.position);
                chasing = false;
            }

            // Check for overlaps
            collidingWithEnemy = false;
            // Collision Update for object interaction
            boxCollider.OverlapCollider(filter, hits);
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i] == null) {
                    continue;
                }

                if (hits[i].tag == "Fighter" && hits[i].GetComponent<CustomTag>().tags.Contains("Enemy")) {
                    collidingWithEnemy = true;
                }

                // Clean up array
                hits[i] = null;
            }
            UpdateMotor(Vector3.zero);
            sortedEnemiesList.Clear();
            enemiesList.Clear();
        }
    }
    protected override void Death() {
        Destroy(gameObject);
    }
}
