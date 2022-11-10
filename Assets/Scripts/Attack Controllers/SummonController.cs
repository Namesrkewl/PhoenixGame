using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonController : MonoBehaviour
{
    private void Awake() {
        summonCount = new List<List<int>>();
        lastUsed = new List<List<float>>();
        SummonQueue();
    }

    // Ability structure
    public int[] goldCost = { 20 };
    public int[] summonCharge = { 1 };
    public int[] maxAbilityCharge = { 1 };
    public float[] cooldown = { 5.0f };
    public GameObject[] summon = { };
    private List<List<int>> summonCount;
    private List<List<float>> lastUsed;

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (GameManager.instance.gold >= goldCost[0] && summonCharge[0] > 0) {
                Instantiate(summon[0]);
                GameManager.instance.gold -= goldCost[0];
                summonCharge[0]--;
                summonCount[0][0]++;
                lastUsed[0].Add(Time.time);
            }
        }
        if (summonCharge[0] < maxAbilityCharge[0]) {
            SummonCooldown(0);
        }
    }

    private void SummonCooldown(int input) {
        if (Time.time - lastUsed[input][0] > cooldown[input]) {
            summonCharge[input]++;
            lastUsed[input].RemoveAt(0);

            if (lastUsed[input].Count != 0) {
                lastUsed[input][0] = Time.time;
            }
        }
    }

    private void SummonQueue() {
        for (int i = 0; i < summon.Length; i++) {
            lastUsed.Add(new List<float>(i));
            summonCount.Add(new List<int>(i));
            summonCount[i].Add(0);
        }
    }
}
