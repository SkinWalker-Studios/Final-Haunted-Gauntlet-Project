using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // enemy stats
    public float speed = 1.0f;
    public int maxHealth = 100;
    int currentHealth;

    float count;

    Rigidbody2D rigidbody2D;

    public GameObject enemyObject; // SET ENEMY

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        count = 0;

        Spawn();
    }

    void Update()
    {
        if (count >= 15)
        {
            count = 0;
            Spawn();
        }
        count += Time.deltaTime;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // damage from player
        if (other.gameObject.CompareTag("Player")) // SET TAG
        {
            ChangeHealth(-2); // SET VALUE
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile")) // SET TAG
        {
            ChangeHealth(-50); // SET VALUE
            Destroy(other.gameObject);
        }
    }

    /* void OnTriggerStay2D(Collider2D other)
    {
        // player contact
        if (other.gameObject.CompareTag("Player")) // SET TAG
        {
            ChangeHealth(0); // SET VALUE
        }
    }*/

    void Spawn()
    {
        enemyObject = Instantiate(enemyObject, rigidbody2D.position + Vector2.up * 1f, Quaternion.identity);

        //PlaySound(throwSound); // SET SPAWN CLIP
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            gameObject.active = false;
        }
    }
}