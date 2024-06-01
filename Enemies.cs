using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemies : MonoBehaviour
{
    [SerializeField] GameObject obj;

    public List<GameObject> allEnemies;
    public List<NPC> allNPCs;
    public List<HPBar> allHPs;

    public int validCount;

    public float slowTime = 0f, freezeTime;

    // Start is called before the first frame update
    void Start()
    {
        initialize();

        slowTime = getMaxSlow();
        freezeTime = getMaxFreeze();
    }

    // Update is called once per frame
    void Update()
    {
        validCount = this.gameObject.transform.childCount;

        slowTime = getMaxSlow();
        freezeTime = getMaxFreeze();
    }

    private void initialize()
    {
        validCount = this.gameObject.transform.childCount;

        allEnemies = new List<GameObject>();
        allNPCs = new List<NPC>();
        allHPs = new List<HPBar>();

        for (int i = 0; i < validCount; i++)
        {
            allEnemies.Add(obj.transform.GetChild(i).gameObject);
            allNPCs.Add(allEnemies[i].GetComponent<NPC>());
            allHPs.Add(allEnemies[i].transform.GetChild(0).gameObject.GetComponent<HPBar>());
        }
    }

    private float getMaxSlow()
    {
        float max = -1f;

        for (int i = 0; i < allNPCs.Count; i++)
        {
            if (allNPCs[i].slowTime > max)
            {
                max = allNPCs[i].slowTime;
            }
        }

        return max;
    }

    private float getMaxFreeze()
    {
        float max = -1f;

        for (int i = 0; i < allNPCs.Count; i++)
        {
            if (allNPCs[i].freezeTime > max)
            {
                max = allNPCs[i].freezeTime;
            }
        }

        return max;
    }

    public void pause()
    {
        for (int i = 0; i < allNPCs.Count; i++)
        {
            allNPCs[i].pause();
        }
    }

    public void resume()
    {
        for (int i = 0; i < allNPCs.Count; i++)
        {
            allNPCs[i].resume();
        }
    }

    public void addToList(GameObject obj)
    {
        allEnemies.Add(obj);
        allNPCs.Add(obj.GetComponent<NPC>());
        allHPs.Add(obj.transform.GetChild(0).gameObject.GetComponent<HPBar>());
    }

    public void removeFromList(GameObject obj)
    {
        allEnemies.Remove(obj);
        allNPCs.Remove(obj.GetComponent<NPC>());
        allHPs.Remove(obj.transform.GetChild(0).gameObject.GetComponent<HPBar>());
    }

    public bool dam(int num)
    {
        int index = getIndex();

        if (index == -1)
        {
            Debug.Log("Enemies: No enemies remaining!");
            return false;
        }

        bool result = false;

        for (int i = 0; i < num; i++)
        {
            result = allHPs[index].Damage();

            if (i == 0 && result == false) return false;
            if (allHPs[index].isDead)
            {
                break;
            }
        }

        return true;
    }

    public bool MaxDam()
    {
        int index = getIndex();

        if (index == -1)
        {
            Debug.Log("Enemies: No enemies remaining!");
            return false;
        }

        bool result = allHPs[index].InstaKill();

        return result;
    }

    public void SlowDown()
    {
        if (validCount == 0)
        {
            Debug.Log("Enemies: No enemies remaining!");
            return;
        }

        for (int i = 0; i < allNPCs.Count; i++)
        {
            allNPCs[i].slowSpeed();
        }
    }

    public void freeze()
    {
        if (validCount == 0)
        {
            Debug.Log("Enemies: No enemies remaining!");
            return;
        }

        for (int i = 0; i < allNPCs.Count; i++)
        {
            allNPCs[i].freezePos();
        }
    }

    private int getIndex()
    {
        validCount = this.gameObject.transform.childCount;

        if (validCount == 0) return -1;

        List<int> arr = new List<int>();

        for (int i = 0; i < validCount; i++)
        {
            if (allNPCs[i].seePlayer)
            {
                arr.Add(i);
            }
        }

        if (arr.Count == 0) return getRandValidIndex();

        int index = 0;
        float min = (float) Int32.MaxValue;
        for (int i = 0; i < arr.Count; i++)
        {
            int ind = arr[i];

            if (allNPCs[ind].getDist() < min)
            {
                min = allNPCs[ind].getDist();
                index = ind;
            }
        }

        return index;
    }

    private int getRandValidIndex()
    {
        if (validCount == 0) return -1;

        int rand = q.getRandI(0, validCount - 1);
        return rand;
    }
}
