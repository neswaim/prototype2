using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymovement : MonoBehaviour
{
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private float characterVelocity = 10f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private int enemyCount = 1;
    public GameObject wintext;


    void Start()
    {
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
        StartCoroutine(timer());
    }

    void calcuateNewMovementVector()
    {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }

    void Update()
    {
        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            calcuateNewMovementVector();
        }

        //move enemy: 
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
        transform.position.y + (movementPerSecond.y * Time.deltaTime));

        if (enemyCount == 0)
        {
            Application.Quit();
        }
    }

    void OnCollision2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            movementPerSecond = movementDirection * characterVelocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Player")
        {
            enemyCount = enemyCount - 1;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}