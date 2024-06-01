using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] private Vector3 target;
    NavMeshAgent agent;

    //Holds whether the player is currently seen
    public bool seePlayer = false;

    public bool healing = false;

    [SerializeField] PlayerHPBar playerhp;
    [SerializeField] HPBar hpbar;
    [SerializeField] ECount count;
    [SerializeField] NewNPC nn;
    [SerializeField] Stats s;
    [SerializeField] SoundControl sc;
    [SerializeField] Enemies e;

    [SerializeField] Transform targetPos;

    [SerializeField] Sprite moveLeft, moveRight, happyLeft, happyRight;

    [SerializeField] GameObject step1, step2, step3;

    public int childIndex;

    [SerializeField] bool disabled = false, called = false;
    [SerializeField] bool left = true, right = false;

    [SerializeField] bool freeze = false, slow = false, frozen = false, notMoving = false;

    //Holds whether the player was seen before
    [SerializeField] bool seenPlayer = false;

    //Holds when the player was last seen
    [SerializeField] float lastSeenPlayer = 0f;

    [SerializeField] float transitionTime = 0f, wanderTime = 0f;

    //Holds the previous state
    [SerializeField] string prevState;

    //Holds the current state
    [SerializeField] string currState = "idle";

    //Holds whether the enemy is attacking or not
    [SerializeField] bool attacking;

    //Holds whether the enemy has reached its destination
    [SerializeField] bool destReached;

    //Holds the position of the enemy AI
    public Vector3 enemyPosition, prevEnemyPos;

    private SpriteRenderer sp;

    private string[] allStates = new string[] { "idle", "chase", "attack", "flee" };
    private float waitTime = 5f, wanderWaitTime = 11f;
    public float slowTime = 0f, freezeTime = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        sp = GetComponent<SpriteRenderer>();

        playerhp = GameObject.Find("Player").GetComponent<PlayerHPBar>();
        nn = GameObject.Find("NPCs").GetComponent<NewNPC>();
        s = GameObject.Find("Game Manager").GetComponent<Stats>();
        sc = GameObject.Find("Music Holder").GetComponent<SoundControl>();
        e = GameObject.Find("NPCs").GetComponent<Enemies>();
        targetPos = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //At first, destination has not been reached
        destReached = false;

        //Enemy is not attacking
        attacking = false;

        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen && !called && hpbar.isDead)
        {
            called = true;
            die();
            return;
        } 

        if (!disabled)
        {
            //If enemy doesn't have any life left ...
            if (hpbar.isDead)
            {
                //Enemy will die
                die();
                return;
            }

            prevEnemyPos = enemyPosition;

            //Make enemyPosition the same as the enemy's position
            enemyPosition = transform.position;

            //Holds whether the enemy's destination has been reached yet
            destReached = reachedDest();

            if (enemyHealthLow()) Healing();

            float dTime = Time.deltaTime;

            transitionTime += dTime;
            wanderTime += dTime;

            if (slow)
            {
                slowTime += dTime;
            }

            if (slowTime >= 5f)
            {
                slow = false;
                slowTime = 0f;
                SetSpeed(2);
            }

            if (freeze)
            {
                freezeTime += dTime;
            }

            if (freezeTime >= 5f)
            {
                freeze = false;
                freezeTime = 0f;
                Move(agent.speed);
            }


            if (getDist() > 5f)
            {
                //Don't see player right now
                seePlayer = false;
            }
            else
            {
                //Have seen player before
                seenPlayer = true;

                //Can see the player right not
                seePlayer = true;
            }

            //Add the time to lastSeenPlayer if have seen player before
            if (seenPlayer) lastSeenPlayer += dTime;

            if (notMoving) return;

            //Set the previous state to be the current state
            prevState = currState;

            //Get the next state
            currState = getNextState();

            //Switch the enemy's state
            switch (currState)
            {
                //If currState is idle ...
                case "idle":

                    //Enemy will idle
                    idle();

                    //Exit case
                    break;

                case "wander":

                    //Enemy will wander
                    wander();

                    //Exit case
                    break;

                //If currState is chase
                case "chase":

                    //Enemy will chase
                    chase();

                    //Exit case
                    break;

                //If currState is attack
                case "attack":

                    //Enemy will attack player
                    attack();

                    //Exit case
                    break;

                //If currState is flee
                case "flee":

                    //Enemy will run away
                    flee();

                    //Exit case
                    break;
            }
        }

        else
        {
            StopMoving();
        }
    }

    public void pause()
    {
        StopMoving();
        disabled = true;
    }

    public void resume()
    {
        StartMoving();
        disabled = false;
    }

    //Returns the enemy's next state as a string
    string getNextState()
    {
        //Get a random number from 1-100
        float rand = getProb();

        //If enemy can see the player ... 
        if (seePlayer)
        {
            //If the previous state was running away ...
            if (prevState == "flee")
            {
                //If the player's health is low ...
                if (playerHealthLow())
                {
                    //70% chance to chase
                    if (rand < 70) return "chase";

                    //30% chance to run away
                    return "flee";
                }

                //If player's health is not low ...
                else
                {
                    //20% chance to chase
                    if (rand < 20) return "chase";

                    //80% chance to run away
                    return "flee";
                }
            }

            //If previous state was not running away ... chase
            return "chase";
        }

        //If not enough time is spent on the current state, return the previous state
        if (transitionTime < waitTime) return prevState;

        transitionTime %= waitTime;

        //If the enemy is bored ... idle
        if (isBored(rand))
        {
            return "idle";
        }

        //If the enemy's health is low ...
        if (enemyHealthLow())
        {
            //90% chance of running away
            if (rand <= 90)
            {
                return "flee";
            }
        }

        //If none of the above conditions are met ... wander
        return "wander";
    }

    //Idling because enemy is bored
    void idle()
    {
        //Enemy stops moving around
        StopMoving();
    }

    void wander()
    {
        SetSpeed(2f);
        StartMoving();

        float x = q.getRandF(-9f, 70f);
        float y = q.getRandF(-6.5f, 5.5f);

        if (wanderTime >= wanderWaitTime - 1 || destReached)
        {
            target = new Vector3(x, y, 0f);
            wanderTime = 0f;
        }

        wanderTime %= wanderWaitTime;

        SetAgentPosition();
        TurnAgent();
    }

    //Chases the player
    void chase()
    {
        Move(2);
    }

    //Attacks the player
    void attack()
    {
        //Enemy is attacking
        attacking = true;

        chase();
    }

    //Runs away from the player
    void flee()
    {
        //Find opposite direction of player
        Vector3 direction = enemyPosition - targetPos.position;

        //Choose a random cell to run away to
        runAway(direction);
    }

    //Enemy dies
    void die()
    {
        //Wait some time before dying
        StartCoroutine(enemyDie(.75f));
    }

    //Chooses the place for enemy to run away to
    void runAway(Vector3 direction)
    {
        Move(4);
    }

    private IEnumerator enemyDie(float delay)
    {
        if (left)
        {
            sp.sprite = happyLeft;
        }

        else
        {
            sp.sprite = happyRight;
        }

        yield return new WaitForSeconds(delay);
        sendSpace();
        e.removeFromList(this.gameObject);

        Destroy(this.gameObject);
    }

    public void Healing()
    {
        if (!hpbar.canHeal()) return;
        healing = true;
        StartCoroutine(heal());
    }

    private IEnumerator heal()
    {
        healing = true;
        step1.SetActive(true);
        yield return new WaitForSeconds(1f);

        step1.SetActive(false);
        step2.SetActive(true);
        yield return new WaitForSeconds(1f);

        step2.SetActive(false);
        step3.SetActive(true);
        yield return new WaitForSeconds(1f);

        step3.SetActive(false);
        bool result = hpbar.Heal();

        healing = false;
    }

    public float getDist()
    {
        return q.dist(enemyPosition, targetPos.position);
    }

    //Returns if the enemy is bored
    bool isBored(float rand)
    {
        //If the previous state is idle ...
        if (prevState == "idle")
        {
            //90% chance of being bored; 10% chance not being bored
            return rand <= 90;
        }

        //Enemy is not bored
        return false;
    }

    //Returns if the player's health is low
    bool playerHealthLow()
    {
        return playerhp.healthLow();
    }

    //Returns is the enemy's health is low
    bool enemyHealthLow()
    {
        return hpbar.healthLow();
    }

    //Returns if the enemy's destination has been reached yet
    bool reachedDest()
    {
        return q.approx(enemyPosition.x, target.x) && q.approx(enemyPosition.y, target.y);
    }

    //Gets a random number from 0 to 100
    float getProb()
    {
        return q.getRandF(0f, 100f);
    }

    void SetTargetPosition()
    {
        target = targetPos.position;
    }

    void SetAgentPosition()
    {
        agent.destination = new Vector3(target.x, target.y, transform.position.z);
    }

    void StartMoving()
    {
        agent.isStopped = false;
        notMoving = false;
    }

    void StopMoving()
    {
        agent.isStopped = true;
        notMoving = true;
    }

    void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    void Move(float speed)
    {
        SetSpeed(speed);
        StartMoving();
        SetTargetPosition();
        SetAgentPosition();
        TurnAgent();
    }

    void TurnAgent()
    {
        if (prevEnemyPos.x > enemyPosition.x)
        {
            sp.sprite = moveLeft;
            left = true;
            right = false;
        }
        else if (prevEnemyPos.x < enemyPosition.x) 
        { 
            sp.sprite = moveRight;
            left = false;
            right = true;
        }
    }

    public void slowSpeed()
    {
        slow = true;
        SetSpeed(1);
    }

    public void freezePos()
    {
        freeze = true;
        StopMoving();
    }

    public void damagePlayer()
    {
        StartCoroutine(damPlayer());
    }

    private IEnumerator damPlayer()
    {
        StopMoving();
        yield return new WaitForSeconds(5f);
        StartMoving();
    }

    public void sendSpace()
    {
        nn.addToStack();
        s.enemiesKilled++;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (freeze)
            {
                sp.color = Color.green;
                disabled = true;
                frozen = true;
            }
        }
    }
}
