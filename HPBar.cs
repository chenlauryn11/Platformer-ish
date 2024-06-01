using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] NPC npc;

    [SerializeField] ECount count;

    //The HP Bar
    [SerializeField] GameObject HPBarObj;

    //The healing flowers
    [SerializeField] List<GameObject> flowers;

    [SerializeField] int index = 0;

    //Holds the individual hearts in the HP bar
    GameObject[] hearts;

    private int maxHealth;
    [SerializeField] private int health;
    public bool isDead = false;
    

    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        checkDeath();
    }

    public bool healthLow()
    {
        return health <= 2;
    }

    public bool canHeal()
    {
        return !isDead && health < maxHealth && count.healFlower > 0;
    }

    public bool Heal()
    {
        checkDeath();

        if (isDead) return false;

        //Don't do anything because the NPC is already at max health
        if (health >= maxHealth)
        {
            Debug.Log("Enemy already fully healed!");
            return false;
        }

        //If NPC has the potions to heal themselves ...
        if (count.healFlower > 0 && flowers.Count > 0)
        {
            //Increase the health by 1
            health++;
            HandleHearts();

            //Decrease healFlower by 1
            count.healFlower--;

            flowers[index].SetActive(false);
            flowers.RemoveAt(index);


            if (health >= maxHealth)
            {
                health = maxHealth;
                Debug.Log("Enemy at max health!");
            }
        }

        return true;
    }

    public bool Damage()
    {
        checkDeath();

        if (isDead) return false;

        health--;

        HandleHearts();

        checkDeath();

        return true;
    }

    public bool InstaKill()
    {
        checkDeath();

        if (isDead) return false;

        health = 0;
        HandleHearts();

        checkDeath();

        return true;
    }

    private bool noHealth()
    {
        return health <= 0;
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
        if (noHealth() && npc.healing == false && count.healFlower == 0) isDead = true;
        else isDead = false;
    }

    //Changes the color of the heart
    private void changeColor(GameObject heart, string color)
    {
        //Get the sprite renderer
        SpriteRenderer sr = heart.GetComponent<SpriteRenderer>();

        //Create the color gray
        Color g = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        //Create the color red
        Color r = new Color(255f, 0f, 0f, 255f);

        switch (color)
        {
            case "gray":
            case "grey":
                //Change the color of the heart to gray
                sr.color = g;
                break;
            default:
                //Change the color of the heart to red
                sr.color = r;
                break;
        }
    }
    
    private void initialize()
    {
        //Set the maximum health to 5
        maxHealth = 5;

        //Start the NPC on maximum health
        health = maxHealth;

        hearts = new GameObject[5];

        //Get each individual heart and put it in the array
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = HPBarObj.transform.GetChild(i).gameObject;
        }

        HandleHearts();
    }
}
