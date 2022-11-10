using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private void Awake() {
        abilityCount = new List<List<int>>();
        lastUsed = new List<List<float>>();
        AbilityQueue();
    }

    // Ability structure
    public int[] heatCost = { 20 };
    public int[] abilityCharge = { 1 };
    public int[] maxAbilityCharge = { 1 };
    public float[] cooldown = { 5.0f };
    public GameObject[] ability = { };
    private List<List<int>> abilityCount;
    private List<List<float>> lastUsed;

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (GameManager.instance.heat >= heatCost[0] && abilityCharge[0] > 0) {
                Instantiate(ability[0]);
                GameManager.instance.heat -= heatCost[0];
                abilityCharge[0]--;
                abilityCount[0][0]++;
                lastUsed[0].Add(Time.time);
            }
        }
        if (abilityCharge[0] < maxAbilityCharge[0]) {
            AbilityCooldown(0);
        }
    }

    private void AbilityCooldown(int input) {
        if (Time.time - lastUsed[input][0] > cooldown[input]) {
            abilityCharge[input]++;
            lastUsed[input].RemoveAt(0);

            if (lastUsed[input].Count != 0) {
                lastUsed[input][0] = Time.time;
            }
        }
    }

    private void AbilityQueue() {
        for (int i = 0; i < ability.Length; i++) {
            lastUsed.Add(new List<float>(i));
            abilityCount.Add(new List<int>(i));
            abilityCount[i].Add(0);
        }
    }
}
