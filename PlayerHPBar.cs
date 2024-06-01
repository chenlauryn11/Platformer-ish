using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public string type = "";

    [SerializeField] PlayerControl player;
    [SerializeField] Inventory count;
    [SerializeField] SoundControl sc;

    //The HP Bar that is the child of the player
    [SerializeField] GameObject healthObj;

    //Holds the individual hearts in the HP bar
    GameObject[] hearts;

    private int maxHealth;
    private int health;
    public bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        initialize();

        HandleHearts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool healthLow()
    {
        return health <= 2;
    }

    public bool Heal(int num)
    {
        //Don't do anything because the player is already at max health
        if (health >= maxHealth)
        {
            Debug.Log("You're already fully healed!");
            return false;
        }

        sc.playGainHealth();

        //If player has the potions to heal themselves ...
        if (num == 1)
        {
            //Increase the health by 1
            health++;
        }
        
        //If player has the potions to heal themselves ...
        else if (num == 2)
        {
            //Increase the health by 2
            health+=2;
        }
        
        //If player has the potions to heal themselves ...
        else if (num == 3)
        {
            //Increase the health by 3
            health+=3;
        }

        HandleHearts();

        if (health >= maxHealth)
        {
            health = maxHealth;
            Debug.Log("You're at max health!");
        }

        return true;
    }

    public bool Damage()
    {
        checkDeath();

        if (isDead) return false;

        sc.playLoseHealth();
        health--;
        HandleHearts();

        checkDeath();

        return true;
    }

    public bool MegaHeal()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
            Debug.Log("You're at max health!");
            return false;
        }

        //Completely heal player
        health = maxHealth;

            
        if (health >= maxHealth)
        {
            health = maxHealth;
            Debug.Log("You're at max health!");
        }

        HandleHearts();
        return true;
    }

    public bool IncreaseMaxHealth()
    {
        if (maxHealth >= 7) return false;

        maxHealth++;

        hearts[maxHealth - 1].SetActive(true);

        HandleHearts();

        return true;
    }

    private bool noHealth()
    {
        return health == 0;
    }

    private void HandleHearts()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < health) changeColor(hearts[i], "red");
            else changeColor(hearts[i], "grey");
        }
    }

    private void checkDeath()
    {
        isDead = noHealth();

        if (isDead)
        {
            player.gameOver = true;
            return;
        }
    }

    //Changes the color of the heart
    private void changeColor(GameObject heart, string color)
    {
        //Get the sprite renderer
        Image im = heart.GetComponent<Image>();

        //Create the color gray
        Color32 g = new Color32(149, 149, 149, 172);

        //Create the color red
        Color32 r = new Color32(255, 15, 15, 172);

        switch (color)
        {
            case "gray":
            case "grey":
                //Change the color of the heart to gray
                im.color = g;
                break;
            default:
                //Change the color of the heart to red
                im.color = r;
                break;
        }
    }

    private void initialize()
    {
        //Set the maximum health to 5
        maxHealth = 5;

        //Start the player on maximum health
        health = maxHealth;

        hearts = new GameObject[7];

        //Get each individual heart and put it in the array
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = healthObj.gameObject.transform.GetChild(i).gameObject;

            if (i >= 5) hearts[i].SetActive(false);
        }
    }
}
