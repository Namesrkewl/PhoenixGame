using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private void Start() {
        lookAt = GameObject.Find("Player").transform;
    }

    public Transform lookAt;
    /*public float boundX = 0.3f;
    public float boundY = 0.15f;*/

    private void LateUpdate()
    {
        if (GameManager.instance.Pause()) {
            return;
        }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 delta = new Vector3((mousePos.x - playerPos.x) / 4, (mousePos.y - playerPos.y) / 4, -10f);
            transform.position = playerPos + delta;
    }
}
