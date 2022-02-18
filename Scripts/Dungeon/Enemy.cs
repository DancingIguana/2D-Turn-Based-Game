using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5;
    [SerializeField] AudioClip spottedSound;
    public Fighter enemyFighter;
    
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;
    private bool seen = true;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        Vector3 direction = player.position - transform.position;
        //Debug.Log(direction);
        if(direction.x < 0) {
            sprite.flipX = false;
        } else
        {
            sprite.flipX = true;
        }
        direction.Normalize();
        movement = direction;
    }
    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        if(Vector2.Distance((Vector2)transform.position, (Vector2)player.position) < 1) {
            if(seen == false) {
                SoundManagerScript.PlaySound(spottedSound);
                seen = true;
            }
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        } else {
            seen = false;
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Sword")
        {
            Debug.Log("EnemyWon!");
            Characters.instance.enemyList.EnemyList.RemoveAt(0);
            Debug.Log("Removed enemy in position " + Characters.instance.enemyPos[0]);
            Characters.instance.enemyPos.RemoveAt(0);
            Characters.instance.playerPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            
            StoreEnemy.instance.enemy = enemyFighter;
        }
    }    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Sword")
        {
            Debug.Log("Player Won!");
            Characters.instance.enemyList.EnemyList.RemoveAt(0);
            Debug.Log("Removed enemy in position " + Characters.instance.enemyPos[0]);
            Characters.instance.enemyPos.RemoveAt(0);
            Characters.instance.playerPos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            
            StoreEnemy.instance.enemy = enemyFighter;
        }

    }
}
