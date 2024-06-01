using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPotions : MonoBehaviour
{
    /*
     * Vector3[] that holds all positions that potions can be instantiated
     * List<GameObject> that holds all next potions to be instantiated
     * After certain amount of time (specific to each type of potion), a potion of that type is added to List<GameObject>
     * 
     * when a space for a potion to be instantiated and a List<GameObject>.Count != 0, instantiate a potion at that position
     * players could technically just wait at an area and continue collecting potions at that space, decide if want to keep or not.
     */
    [SerializeField] GameObject Potions;
    [SerializeField] GameObject[] allPotions;
    [SerializeField] Transform potionHolder;
    [SerializeField] int stackLength;

    private List<string> stack;
    private List<Vector3> freeSpace;
    private float passedTime = 0, waitTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        stack = new List<string>();
        freeSpace = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;

        passedTime += time;

        if (isRoom() && stack.Count > 0 && passedTime > waitTime)
        {
            string name = stack[0];
            GameObject inst = Instantiate(allPotions[getIndex(name)], getFreeSpace(), Quaternion.identity);
            inst.transform.SetParent(potionHolder.transform);
            stack.RemoveAt(0);

            passedTime %= waitTime;
        }

        stackLength = stack.Count;
    }

    public void addPotion(string str)
    {
        stack.Add(str);
    }

    public void addSpace(Vector3 v)
    {
        freeSpace.Add(v);
    }

    private bool isRoom()
    {
        return freeSpace.Count > 0;
    }

    private Vector3 getFreeSpace()
    {
        if (isRoom())
        {
            int rand = q.getRandI(0, freeSpace.Count - 1);
            Vector3 v = freeSpace[rand];
            freeSpace.RemoveAt(rand);
            return v;
        }

        return Vector3.zero;
    }

    private int getIndex(string potionName)
    {
        switch(potionName)
        {
            case "dam1":
                return 0;
            case "dam2":
                return 1;
            case "dam3":
                return 2;
            case "damMax":
                return 3;
            case "heal1":
                return 4;
            case "heal2":
                return 5;
            case "heal3":
                return 6;
            case "healMax":
                return 7;
            case "speed5":
                return 8;
            case "speed1":
                return 9;
            case "speed15":
                return 10;
            case "speed2":
                return 11;
            case "slow":
                return 12;
            case "increase":
                return 13;
            case "freeze":
                return 14;
            case "fly":
                return 15;
        }

        return -1;
    }
}
