using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // player stats
    public float speed = 1.0f;
    public int maxHealth = 2000;
    float x = 0;
    float count = 0;
    public int currentHealth;
    float currentSpeed;

    // UI
    public TextMeshProUGUI healthText;
    public GameObject keySlot1;
    public GameObject keySlot2;
    public GameObject keySlot3;
    public GameObject potionSlot1;
    public GameObject potionSlot2;
    GameObject explosionObject;

    // collectables
    public int keyAmount;
    public int potionAmount;

    // tracking player movement & position
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    public float posX;
    public float posY;

    // audio
    public AudioSource musicSource;
    AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip popClip;

    // animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public GameObject projectilePrefab; // SET PROJECTILE
    public GameObject explosionPrefab;

    void Start()
    {
        // set base variables
        currentHealth = maxHealth;
        currentSpeed = speed;
        keyAmount = 0;
        potionAmount = 0;
        posX = 0; // SET VALUE
        posY = -3; // SET VALUE
        healthText.text = "HEALTH: " + currentHealth.ToString();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // empty items
        keySlot1.SetActive(false);
        keySlot2.SetActive(false);
        keySlot3.SetActive(false);
        potionSlot1.SetActive(false);
        potionSlot2.SetActive(false);


        // background music
        audioSource = GetComponent<AudioSource>();
        musicSource.clip = backgroundMusic; // SET BACKGROUND MUSIC
        musicSource.Play();
    }

    void Update()
    {
        // animation states
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = transform.position;
        posX = position.x;
        posY = position.y;

        // firing projectiles
        if (Input.GetKeyDown(KeyCode.X))
        {
            Launch();
            currentSpeed = 0;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            currentSpeed = speed;
        }

        // esc to exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if ((Input.GetKeyDown(KeyCode.Z)) && (potionAmount > 0))
        {
            explosionObject = Instantiate(explosionPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            ChangePotions(-1);
            x += Time.deltaTime;
        }

        if (x > 0)
        {
            x += Time.deltaTime;
            if (x >= 0.2)
            {
                Destroy(explosionObject);
                x = 0;
            }       
        }

        // health decay
        if (count >= 0.5)
        {
            count = 0;
            ChangeHealth(-5); // SET VALUE
        }
        else
        {
            count += Time.deltaTime;
        }


    }
    void FixedUpdate()
    {
        // movements
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 position = transform.position;
        position.x = position.x + currentSpeed * horizontal * Time.deltaTime;
        position.y = position.y + currentSpeed * vertical * Time.deltaTime;
        transform.position = position;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // key collectable
        if (other.gameObject.CompareTag("Key")) // SET TAG
        {
            ChangeKeys(1);
            Destroy(other.gameObject);
        }
        // health collectable
        if (other.gameObject.CompareTag("Health")) // SET TAG
        {
            ChangeHealth(150); // SET VALUE
            Destroy(other.gameObject);
        }
        // potion collectable
        if (other.gameObject.CompareTag("Potion")) // SET TAG
        {
            ChangePotions(1);
            Destroy(other.gameObject);
        }
        // treasure collectable
        if (other.gameObject.CompareTag("Treasure")) // SET TAG
        {
            FindObjectOfType<ScoreManager>().AddPoint(); // SET VALUE
            Destroy(other.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // enemy contact
        if (other.gameObject.CompareTag("Enemy")) // SET TAG
        {
            ChangeHealth(-2); // SET VALUE
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // doors
        if (other.gameObject.CompareTag("Door")) // SET TAG
        {
            if (keyAmount > 0)
            {
                Destroy(other.gameObject);
                ChangeKeys(-1);
            }
        }
        // exit
        if (other.gameObject.CompareTag("Exit")) // SET TAG
        {
            transform.position = new Vector3(-0.7f, -26f, 0f);
        }
        
        // ending
        if (other.gameObject.CompareTag("Ending")) // SET TAG
        {
            SceneManager.LoadScene("Win");
        }
    }
    void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        healthText.text = "HEALTH: " + currentHealth.ToString();
    }

    void ChangeKeys(int amount)
    {
        keyAmount += amount;

        if (keyAmount == 0)
        {
        keySlot1.SetActive(false);
        keySlot2.SetActive(false);
        keySlot3.SetActive(false);
        }

        if (keyAmount == 1)
        {
        keySlot1.SetActive(true);
        keySlot2.SetActive(false);
        keySlot3.SetActive(false);
        }

        if (keyAmount == 2)
        {
        keySlot1.SetActive(true);
        keySlot2.SetActive(true);
        keySlot3.SetActive(false);
        }

        if (keyAmount == 3)
        {
        keySlot1.SetActive(true);
        keySlot2.SetActive(true);
        keySlot3.SetActive(true);
        }
    }
    void ChangePotions(int amount)
    {
        potionAmount += amount;

        if (potionAmount == 0)
        {
        potionSlot1.SetActive(false);
        potionSlot2.SetActive(false);
        }

        if (potionAmount == 1)
        {
        potionSlot1.SetActive(true);
        potionSlot2.SetActive(false);
        }

        if (potionAmount == 2)
        {
        potionSlot1.SetActive(true);
        potionSlot2.SetActive(true);
        }
    }

    // fires a projectile
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        // PlaySound(throwSound); // SET LAUNCH CLIP
    }

    // plays a clip
    public void PlaySound()
    {
        audioSource.PlayOneShot(popClip);
    }
}