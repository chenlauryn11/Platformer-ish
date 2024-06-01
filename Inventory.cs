using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] PlayerControl player;
    [SerializeField] PlayerHPBar playerhp;
    [SerializeField] Enemies enemy;

    [SerializeField] GameObject slotObj;
    [SerializeField] Sprite[] icons = new Sprite[17];

    private int[] bag;
    private string[] arr;
    
    /*public int damPotion1 { get { return bag[1]; } set { bag[1] = value; } }
    public int damPotion2 { get { return bag[5]; } set { bag[5] = value; } }
    public int damPotion3 { get { return bag[9]; } set { bag[9] = value; } }
    public int damPotionMax { get { return bag[13]; } set { bag[13] = value; } }

    public int healPotion1 { get { return bag[2]; } set { bag[2] = value; } }
    public int healPotion2 { get { return bag[6]; } set { bag[6] = value; } }
    public int healPotion3 { get { return bag[10]; } set { bag[10] = value; } }
    public int healPotionMax { get { return bag[14]; } set { bag[14] = value; } }

    public int speedPotion5 { get { return bag[3]; } set { bag[3] = value; } }
    public int speedPotion1 { get { return bag[7]; } set { bag[7] = value; } }
    public int speedPotion15 { get { return bag[11]; } set { bag[11] = value; } }
    public int speedPotion2 { get { return bag[15]; } set { bag[15] = value; } }

    public int slow { get { return bag[0]; } set { bag[0] = value; } }
    public int increaseMax { get { return bag[4]; } set { bag[4] = value; } }
    public int freeze { get { return bag[8]; } set { bag[8] = value; } }
    public int fly { get { return bag[12]; } set { bag[12] = value; } }*/


    // Start is called before the first frame update
    void Start()
    {
        /* Potion Type          Index
         * slow                 0
         * damPotion1           1
         * healPotion1          2
         * speedPotion5         3
         * 
         * increaseMax          4
         * damPotion2           5
         * healPotion2          6
         * speedPotion1         7
         * 
         * freeze               8
         * damPotion3           9
         * healPotion3          10
         * speedPotion15        11
         * 
         * fly                  12
         * damPotionMax         13
         * healPotionMax        14
         * speedPotion2         15
         */
        bag = new int[16];
        arr = new string[16] { "Slow: ", "Damage +1: ", "Heal +1: ", "Speed +0.5: ", "Max +1: ", "Damage +2: ", "Heal +2: ", "Speed +1: ", "Paralyze: ", "Damage +3: ", "Heal +3: ", "Speed +1.5: ", "Fly: ", "Insta-kill: ", "Full Heal: ", "Speed +2 ", };

        updateBag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPotion(Type type)
    {
        switch(type)
        {
            case Type.heal1:
                bag[2]++;
                break;

            case Type.heal2:
                bag[6]++; 
                break;
            case Type.heal3:
                bag[10]++;
                break;
            case Type.healMax:
                bag[14]++; 
                break;

            case Type.dam1:
                bag[1]++;
                break;
            case Type.dam2:
                bag[5]++;
                break;
            case Type.dam3:
                bag[9]++;
                break;
            case Type.damMax:
                bag[13]++;
                break;

            case Type.speed5:
                bag[3]++; 
                break;
            case Type.speed1:
                bag[7]++;
                break;
            case Type.speed15:
                bag[11]++;
                break;
            case Type.speed2:
                bag[15]++;
                break;

            case Type.slow:
                bag[0]++;
                break;
            case Type.increase:
                bag[4]++; 
                break;
            case Type.freeze:
                bag[8]++;
                break;
            case Type.fly:
                bag[12]++; 
                break;
        }

        updateBag();
    }

    private void updateBag()
    {
        for (int i = 0; i < bag.Length; i++)
        {
            Image im = slotObj.transform.GetChild(i).GetChild(0).GetComponent<Image>();

            if (bag[i] == 0) im.sprite = icons[16];
            else im.sprite = icons[i];

            TextMeshProUGUI txt = slotObj.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            txt.text = arr[i] + bag[i];
        }
    }

    public void heal1()
    {
        if (bag[2] <= 0) return;

        bool success = playerhp.Heal(1);

        if (success) bag[2]--;

        updateBag();
    }

    public void heal2()
    {
        if (bag[6] <= 0) return;

        bool success = playerhp.Heal(2);

        if (success) bag[6]--;

        updateBag();
    }

    public void heal3()
    {
        if (bag[10] <= 0) return;

        bool success = playerhp.Heal(3);

        if (success) bag[10]--;

        updateBag();
    }

    public void healMax()
    {
        if (bag[14] <= 0) return;

        bool success = playerhp.MegaHeal();

        if (success) bag[14]--;

        updateBag();
    }

    public void dam1()
    {
        if (bag[1] <= 0) return;

        bool success = enemy.dam(1);

        if (success) bag[1]--;

        updateBag();
    }

    public void dam2()
    {
        if (bag[5] <= 0) return;

        bool success = enemy.dam(2);

        if (success) bag[5]--;

        updateBag();
    }

    public void dam3()
    {
        if (bag[9] <= 0) return;

        bool success = enemy.dam(3);

        if (success) bag[9]--;

        updateBag();
    }

    public void damMax()
    {
        if (bag[13] <= 0) return;

        bool success = enemy.MaxDam();

        if (success) bag[13]--;

        updateBag();
    }

    public void speed5()
    {
        if (bag[3] <= 0) return;

        player.changeSpeed(0.5f);

        bag[3]--;

        updateBag();
    }

    public void speed1()
    {
        if (bag[7] <= 0) return;

        player.changeSpeed(1f);

        bag[10]--;

        updateBag();
    }

    public void speed15()
    {
        if (bag[11] <= 0) return;

        bag[11]--;
        player.changeSpeed(1.5f);

        updateBag();
    }

    public void speed2()
    {
        if (bag[15] <= 0) return;

        bag[15]--;
        player.changeSpeed(2f);

        updateBag();
    }

    public void slowDown()
    {
        if (bag[0] <= 0) return;

        bag[0]--;
        enemy.SlowDown();

        updateBag();
    }

    public void increaseMaxHP()
    {
        if (bag[4] <= 0) return;

        bag[4]--;
        playerhp.IncreaseMaxHealth();

        updateBag();
    }

    public void paralyze()
    {
        if (bag[8] <= 0) return;

        bag[8]--;
        enemy.freeze();

        updateBag();
    }

    public void flying()
    {
        if (bag[12] <= 0) return;

        bag[12]--;
        player.flying();

        updateBag();
    }


}
