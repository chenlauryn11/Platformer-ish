using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Game is not over
    public bool gameOver = false;

    public GameObject enemy2;

    [SerializeField] Inventory count;
    [SerializeField] Stats s;
    [SerializeField] Enemies e;
    [SerializeField] SoundControl sc;

    [SerializeField] GameObject gameCanvas;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sp;

    [SerializeField] PlayerHPBar hpBar;

    [SerializeField] Sprite right, left;

    [SerializeField] bool disabled = false;
    [SerializeField] bool touchingGround = true, touchingNPC = false;

    //Indicates if the flying powerups are running right not
    public bool fly = false;

    //Holds the arrow keyboard inputs
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;

    [SerializeField] float speed = 3f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float flyForce = 5f;
    [SerializeField] float passedTime = 0f, waitTime = 1f;
    public float flyTime = 0f;

    List<GameObject> touchingPlayer = new List<GameObject>();
    private bool called = false;

    // Start is called before the first frame update
    void Start()
    {
        gameCanvas.SetActive(true);

        sc.initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //q.print("Player: Distance: " + q.dist(transform.position, enemy2.transform.position));

        if (gameOver && !called)
        {
            called = true;

            endGame();
            return;
        }

        if (transform.position.y <= -7.1) transform.position = new Vector3(transform.position.x, -7.108577f, transform.position.z);
        if (!disabled)
        {
            float dTime = Time.deltaTime;
            passedTime += dTime;

            if (touchingPlayer.Count > 0 && passedTime >= waitTime) 
            {
                for (int i = 0; i < touchingPlayer.Count; i++)
                {
                    hpBar.Damage();
                    NPC n = touchingPlayer[i].GetComponent<NPC>();
                    n.damagePlayer();
                }
            }

            if (touchingPlayer.Count == 0) touchingNPC = false;

            if (fly)
            {
                flyTime += dTime;
            }

            if (flyTime >= 10f)
            {
                fly = false;
                flyTime = 0f;
            }

            passedTime %= waitTime;
            //Get the vertical component of movement
            verticalInput = Input.GetAxis("Vertical");

            if (fly)
            {
                //Get the horizontal component of movement
                horizontalInput = Input.GetAxis("Horizontal");

                if (horizontalInput > 0) sp.sprite = right;
                else if (horizontalInput < 0) sp.sprite = left;

                rb.AddForce(new Vector2(horizontalInput * speed, verticalInput * flyForce));
            }

            else
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && touchingGround)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            
        }
        
        
    }

    // Fixed Update is used for physics updates
    void FixedUpdate()
    {
        if (!disabled && !fly)
        {
            //Get the horizontal component of movement
            horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0) sp.sprite = right;
            else if (horizontalInput < 0) sp.sprite = left;

            Vector3 move = new Vector3(horizontalInput * speed, rb.velocity.y, 0f);
            rb.velocity = move;
        }
    }

    public void pause()
    {
        disabled = true;
    }

    public void resume()
    {
        disabled = false;
    }

    public void changeSpeed(float s)
    {
        StartCoroutine(ChangeSpeed(s + speed));
    }

    private IEnumerator ChangeSpeed(float s)
    {
        SetSpeed(s);

        yield return new WaitForSeconds(10f);

        SetSpeed(3f);
    }

    private void SetSpeed(float s)
    {
        speed = s;
    }

    public void flying()
    {
        //StartCoroutine(startFly());
        fly = true;
    }

    private IEnumerator startFly()
    {
        fly = true;
        yield return new WaitForSeconds(10f);
        fly = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (disabled) return;

        //Debug.Log("Player: Collided with " + other.gameObject.name + " (" + other.gameObject.tag + ")");
        if (other.gameObject.tag == "NPC")
        {
            hpBar.Damage();
            NPC n = other.gameObject.GetComponent<NPC>();
            n.damagePlayer();

            touchingNPC = true;
            touchingPlayer.Add(other.gameObject);
        }

        if (other.gameObject.tag == "Potion")
        {
            //Debug.Log("Collided with " + other.gameObject.name + "!");

            Powerup p = other.gameObject.GetComponent<Powerup>();
            Type t = p.type;

            count.addPotion(t);
            p.sendSpace();

            Destroy(other.gameObject);

            sc.playCollPotion();
        }

        if (other.gameObject.tag == "Ground")
        {
            touchingGround = true;
        }

        if (other.gameObject.tag == "Teleport")
        {
            teleport();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            touchingGround = false;
        }

        if (other.gameObject.tag == "NPC")
        {
            touchingPlayer.Remove(other.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "NPC")
        {
            // hpBar.Damage();
        }

        if (other.gameObject.tag == "Potion")
        {
            Debug.Log("Triggered!");
            
            
        }

    }

    private void teleport()
    {
        transform.position = new Vector3(q.getRandF(-9f, 70f), 6f, transform.position.z);
    }

    private void endGame()
    {
        disabled = true;
        e.pause();

        sc.endGame();

        if (hpBar.isDead)
        {
            sc.playLose();
        }

        else
        {
            sc.playWin();
        }

        s.showStats(hpBar.isDead);
        return;
    }
}
