using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMaster : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool = null;
    [SerializeField] private ObjectPool motherShipObjectPool = null;
    public GameObject bulletPrefab;


    private float width;
    //[SerializeField] Player _playerSC;
    Player _playerSC;


    private Vector3 hMoveDistance = new Vector3 (0.05f, 0, 0);
    private Vector3 vMoveDistance = new Vector3 (0, 0.15f, 0);

    //private const float MAX_LEFT = -2;
    //private const float MAX_RIGHT = 2;
    private const float MAX_MOVE_SPEED = 0.02f;

    public static List <GameObject> allAliens = new List<GameObject>();

    private bool movingRight;
    private float moveTimer = 0.01f;
    private float moveTime = 0.005f;

    private float shootTimer = 3f;
    private const float shootTime = 3f;

    public GameObject motherShipPrefab;
    private Vector3 motherShipSpawnPos = new Vector3(6, 5.5f, 0);
    private float motherShipTimer = 2f;
    private const float MOTHERSHIP_MIN = 15f;
    private const float MOTHERSHIP_MAX = 60f;

    private const float START_Y = 1.7f;
    private bool entering = true;


    void Start()
    {
        _playerSC = GameObject.Find("PlayerShip").GetComponent<Player>();

        width = _playerSC.width - 0.15f;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Alien"))
        {
            allAliens.Add(go);
        }
    }


    void Update()
    {
        if (entering)
        {
            transform.Translate(Vector2.down * Time.deltaTime * 10);

            if (transform.position.y <= START_Y)
            {
                entering = false;
            }
        }
        else
        {
            if (moveTimer <= 0)
            {
                MoveEnemies();
            }

            if (shootTimer <= 0)
            {
                Shoot();
            }

            if (motherShipTimer <= 0)
            {
                SpawnMotherShip();
            }
            moveTimer -= Time.deltaTime;
            shootTimer -= Time.deltaTime;
            motherShipTimer -= Time.deltaTime;
        }
    }

    private void MoveEnemies()
    {
        int hitMax = 0;
        for (int i = 0; i < allAliens.Count; i++)
        {
            if (movingRight)
            {
                allAliens[i].transform.position += hMoveDistance;
            }
            else
            {
                allAliens[i].transform.position -= hMoveDistance;
            }
            if (allAliens[i].transform.position.x > width || allAliens[i].transform.position.x < -width)
            {
                hitMax++;
            }
        }

        if (hitMax > 0)
        {
            for (int i = 0; i < allAliens.Count; i++)
            {
                allAliens[i].transform.position -= vMoveDistance;
            }
            movingRight = !movingRight;
        }
        moveTimer = GetMovedSpeed();
    }

    private void Shoot()
    {
        Vector2 pos = allAliens[Random.Range(0, allAliens.Count)].transform.position;

        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = pos;

        shootTimer = shootTime;
    }

    private void SpawnMotherShip()
    {
        GameObject obj = motherShipObjectPool.GetPooledObject();
        obj.transform.position = motherShipSpawnPos;
        motherShipTimer = Random.Range(MOTHERSHIP_MIN, MOTHERSHIP_MAX);
    }

    private float GetMovedSpeed()
    {
        float f = allAliens.Count * moveTime;

        if (f < MAX_MOVE_SPEED)
        {
            return MAX_MOVE_SPEED;
        }
        else
        {
            return f;
        }
    }
}
