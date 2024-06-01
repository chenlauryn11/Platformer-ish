using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { heal1, heal2, heal3, healMax, dam1, dam2, dam3, damMax, speed5, speed1, speed15, speed2, slow, increase, freeze, fly };

public class Powerup : MonoBehaviour
{
    public Type type;

    [SerializeField] NewPotions np;
    [SerializeField] float respawnTime;
    [SerializeField] float time;

    // Start is called before the first frame update
    void Start()
    {
        np = GameObject.Find("Potions").GetComponent<NewPotions>();
        initializeTime();
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.deltaTime;

        time += timePassed;

        if (time > respawnTime && respawnTime > 0)
        {
            np.addPotion(type + "");
            time %= respawnTime;
        }
    }

    private void initializeTime()
    {
        switch(type)
        {
            case Type.heal1:
                respawnTime = 3f;
                break;
            case Type.heal2:
                respawnTime = 6f;
                break;
            case Type.heal3:
                respawnTime = 9f;
                break;
            case Type.healMax:
                respawnTime = 12f;
                break;
            case Type.dam1:
                respawnTime = 3f;
                break;
            case Type.dam2:
                respawnTime = 6f;
                break;
            case Type.dam3:
                respawnTime = 9f; 
                break;
            case Type.damMax:
                respawnTime = 12f;
                break;
            case Type.speed5:
                respawnTime = 5f;
                break;
            case Type.speed1:
                respawnTime = 10f;
                break;
            case Type.speed15:
                respawnTime = 15f;
                break;
            case Type.speed2:
                respawnTime = 20f;
                break;
            case Type.slow:
                respawnTime = 60f;
                break;
            case Type.increase:
                respawnTime = -1f;
                break;
            case Type.freeze:
                respawnTime = 60f;
                break;
            case Type.fly:
                respawnTime = 60f;
                break;
            default:
                respawnTime = -1f;
                break;

        }
    }

    public void sendSpace()
    {
        np.addSpace(transform.position);
    }
}
