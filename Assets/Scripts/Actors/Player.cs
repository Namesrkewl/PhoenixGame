using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingSize = transform.localScale;
        MovementCooldownQueue = new List<float>();
        DontDestroyOnLoad(gameObject);
    }

    private SpriteRenderer spriteRenderer;
    private Vector3 startingSize;
    private float lastMove;
    private List<float> MovementCooldownQueue;  
    public float cooldown = 5.0f;
    public int maxTeleportCharge = 5;
    public int teleportCharge = 5;

    protected override void UpdateMotor(Vector3 input) {
        transform.position = input;
    }

    private void Update() {
        if (GameManager.instance.Pause()) {
            return;
        } 
        GameManager.instance.GenerateHeat();
        Movement();
    }

    public void Movement() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 teleport = new Vector3(mousePos.x, mousePos.y, 0f);
        if (teleport.x > transform.position.x) {
            transform.localScale = startingSize;
        } else if (teleport.x < transform.position.x) {
            transform.localScale = new Vector3(startingSize.x * -1, startingSize.y, startingSize.y);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            Time.timeScale = 0.25f;
            if (teleportCharge > 0 && Input.GetMouseButtonDown(0)) {
                UpdateMotor(teleport);
                teleportCharge--;
                lastMove = Time.time;
                MovementCooldownQueue.Add(lastMove);
            }
        } else {
            Time.timeScale = 1f;
        }

        if (teleportCharge < maxTeleportCharge && Time.time - MovementCooldownQueue[0] > cooldown) {
            teleportCharge++;
            MovementCooldownQueue.RemoveAt(0);

            if (MovementCooldownQueue.Count != 0) {
                MovementCooldownQueue[0] = Time.time;
            }
        }
    }
    public void SwapSprite(int skinId) {
        Debug.Log(skinId);
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp() {
        maxHitPoint++;
        hitPoint = maxHitPoint;
    }

    public void SetLevel(int level) {
        for (int i = 0; i < level; i++) {
            OnLevelUp();
        }
    }
}
