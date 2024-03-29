using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;

    Camera cam;
    public float width;
    //private float speed = 3f;

    bool isShooting = false;
    //private float coolDown = 0.5f;

    [SerializeField] private ObjectPool objectPool = null;

    public ShipStats shipStats;
    private Vector2 offScreenPos = new Vector2(0, -20);
    private Vector2 startPos = new Vector2(0, -6);
    private float dirx;

    private void Awake()
    {
        cam = Camera.main;
        width = ((1/(cam.WorldToViewportPoint(new Vector3(1,1,0)).x - .5f) / 2) -0.25f);
    }
    void Start()
    {
        shipStats.currentHealth = shipStats.maxHealth;
        shipStats.currentLifes = shipStats.maxLifes;
        transform.position = startPos;
        UIManager.UpdateHealthBar(shipStats.currentHealth);
        UIManager.UpdateLives(shipStats.currentLifes);
    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A) && transform.position.x > -width)
        {
            transform.Translate(Vector2.left * Time.deltaTime * shipStats.shipSpeed);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < width)
        {
            transform.Translate(Vector2.right * Time.deltaTime * shipStats.shipSpeed);
        }
        if(Input.GetKey(KeyCode.Space) && !isShooting)
        {
            StartCoroutine(Shoot());
        }

#endif

        dirx = Input.acceleration.x;
        //Debug.Log(dirx);
        if (dirx <= -0.1f && transform.position.x > -width)
        {
            transform.Translate(Vector2.left * Time.deltaTime * shipStats.shipSpeed);
        }
        if (dirx >= 0.1f && transform.position.x < width)
        {
            transform.Translate(Vector2.right * Time.deltaTime * shipStats.shipSpeed);
        }
    }

    public void ShootButton()
    {
        if (!isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    public void AddHealth()
    {
        if (shipStats.currentHealth == shipStats.maxHealth)
        {
            UIManager.UpdateScore(250);
        }
        else
        {
            shipStats.currentHealth++;
            UIManager.UpdateHealthBar(shipStats.currentHealth);
        }
    }

    public void AddLife()
    {
        if (shipStats.currentLifes == shipStats.maxLifes)
        {
            UIManager.UpdateScore(500);
        }
        else
        {
            shipStats.currentLifes++;
            UIManager.UpdateLives(shipStats.currentLifes);
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        //Instantiate(bulletPrefab,transform.position, Quaternion.identity);
        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = gameObject.transform.position;
        yield return new WaitForSeconds(shipStats.fireRate);
        isShooting = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("enemy bullet hit");
            collision.gameObject.SetActive(false);
            TakeDamage();
        }
    }

    private IEnumerator Respawn()
    {
        transform.position = offScreenPos;

        yield return new WaitForSeconds(2);

        shipStats.currentHealth = shipStats.maxHealth;

        transform.position = startPos;
        UIManager.UpdateHealthBar(shipStats.currentHealth);
    }

    private void TakeDamage()
    {
        shipStats.currentHealth--;
        UIManager.UpdateHealthBar(shipStats.currentHealth);

        if (shipStats.currentHealth <= 0)
        {
            shipStats.currentLifes--;
            UIManager.UpdateLives(shipStats.currentLifes);

            if (shipStats.currentLifes <= 0)
            {
                Debug.Log("Game Over");
            }
            else
            {
                //Debug.Log("Respawn");
                StartCoroutine(Respawn());
            }
        }
    }
}
