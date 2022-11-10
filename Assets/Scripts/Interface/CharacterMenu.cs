using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    private static CharacterMenu instance;
    private void Awake() {
        if (CharacterMenu.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Text fields
    public Text levelText, hitpointText, goldText, upgradeCostsText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Character Selection
    public void OnArrowClick(bool right) {
        if (right) {
            currentCharacterSelection++;

            // If we went to far in array (no more characters in selection)
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count) {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        } else {
            currentCharacterSelection--;

            // If we went to far in array (no more characters in selection)
            if (currentCharacterSelection < 0) {
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }

            OnSelectionChanged();
        }
    }

    // Change Sprite
    private void OnSelectionChanged() {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon Upgrade
    public void OnUpgradeClick() {
        if (GameManager.instance.TryUpgradeWeapon()) {
            UpdateMenu();
        }
    }

    // Update Character Information
    public void UpdateMenu() {

        // Character

        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameObject.Find("WeaponController").GetComponent<WeaponController>().weapon.GetComponent<Weapon>().weaponLevel];
        if (GameObject.Find("WeaponController").GetComponent<WeaponController>().weapon.GetComponent<Weapon>().weaponLevel >= GameManager.instance.weaponPrices.Count) {
            upgradeCostsText.text = "MAX";
        } else {
            upgradeCostsText.text = GameManager.instance.weaponPrices[GameObject.Find("WeaponController").GetComponent<WeaponController>().weapon.GetComponent<Weapon>().weaponLevel].ToString();
        }

        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoint.ToString() + " / " + GameManager.instance.player.maxHitPoint.ToString();
        goldText.text = GameManager.instance.gold.ToString();

        // xp Bar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count) /* Max Level */ {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points.";
            xpBar.localScale = Vector3.one;
        } else {
            int previousLevelXp = GameManager.instance.GetXpToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXpToLevel(currentLevel);
            int difference = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;
            float completionRatio = (float)currentXpIntoLevel / (float)difference;
            xpText.text = (GameManager.instance.experience.ToString()) + " / " + (GameManager.instance.GetXpToLevel(currentLevel).ToString());
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
        }
    }
}
