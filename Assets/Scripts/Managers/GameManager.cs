using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private void Awake() {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }

        if (deleteSave) {
            PlayerPrefs.DeleteAll();
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    // Resources
    public List<Sprite> playerSprites; 
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public Ability ability;
    public Summon summon;
    public FloatingTextManager floatingTextManager;

    // Logic -- keeping track of value
    public int gold;
    public int experience;
    public int heat;
    public int maxHeat = 100;
    public float heatRegen = 0.2f;
    private float lastGeneratedHeat = 0;
    public bool deleteSave;

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Upgrade Weapon
    public bool TryUpgradeWeapon() {
        // Is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel) {
            return false;
        } else if (gold >= weaponPrices[weapon.weaponLevel]) {
            gold -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Experience System
    public int GetCurrentLevel() {
        int r = 0;
        int add = 0;

        while (experience >= add) {

            if (r == xpTable.Count) /* Max level */ {
                return r;
            }

            add += xpTable[r];
            r++;
        }

        return r;
    }
    public int GetXpToLevel(int level) {
        int r = 0;
        int xp = 0;

        while (r < level) {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp) {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel()) {
            OnLevelUp();
        }
    }
    public void GrantGold(int goldValue) {
        gold += goldValue;
    }
    public void GenerateHeat() {
        if (heat < maxHeat) {
            if (Time.time - lastGeneratedHeat > heatRegen) {
                heat++;
                lastGeneratedHeat = Time.time;
} 
        }
    }

    public void OnLevelUp() {
        Debug.Log("Level Up!");
        player.OnLevelUp();
    }

    // Save state
    /*
     * INT preferedSkin
     * INT gold
     * INT experience
     * INT weaponLevel
     */
    public void SaveState() {
        Debug.Log("SaveState");

        string s = "";

        s += "0" + "|";
        s += gold.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode) {
        if (!PlayerPrefs.HasKey("SaveState")) {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //player.playerSprite = int.Parse(data[0]);
        gold = int.Parse(data[1]);        
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel() - 1);
        weapon.SetWeaponLevel(int.Parse(data[3]));
        // Teleport to spawn point
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;

        Debug.Log("LoadState");
    }

    public bool Pause() {
        if (GameObject.Find("Menu").GetComponent<Animator>().GetBool("hide")) {
            Time.timeScale = 1;
            return false;
        } else if (GameObject.Find("Menu").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("menu_showing")) {
            Time.timeScale = 0;
            return true;
        }
        return false;
    }

    private void Update() {
        Pause();
        //Debug.Log();
    }
}
