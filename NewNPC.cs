using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNPC : MonoBehaviour
{
    /*
     * Vector3[] that holds all positions that potions can be instantiated
     * List<GameObject> that holds all next potions to be instantiated
     * After certain amount of time (specific to each type of potion), a potion of that type is added to List<GameObject>
     * 
     * when a space for a potion to be instantiated and a List<GameObject>.Count != 0, instantiate a potion at that position
     * players could technically just wait at an area and continue collecting potions at that space, decide if want to keep or not.
     */
    [SerializeField] GameObject ghost;
    [SerializeField] GameObject NPCs;
    [SerializeField] int stackLength;
    [SerializeField] Enemies enemy;
    int numEnemy = 6;

    private List<int> stack;
    private float passedTime = 0, waitTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        stack = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;

        passedTime += time;

        if (isRoom() && passedTime > waitTime && enemy.validCount < 5)
        {
            GameObject inst = Instantiate(ghost/*, getFreeSpace(), Quaternion.identity*/);
            inst.transform.SetParent(NPCs.transform/*, false*/);
            inst.transform.position = new Vector3(460, -10f, 0f);
            inst.tag = "NPC";
            inst.name = "NPC (" + numEnemy + ")";
            numEnemy++;

            enemy.addToList(inst);
            passedTime %= waitTime;
        }

        stackLength = stack.Count;
    }

    public void addToStack()
    {
        stack.Add(0);
    }

    private bool isRoom()
    {
        return stack.Count > 0;
    }

    private Vector3 getFreeSpace()
    {
        if (isRoom())
        {
            return new Vector3(460f, -10f, 0f);
        }

        return Vector3.zero;
    }
}
