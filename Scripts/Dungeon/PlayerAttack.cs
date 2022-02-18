using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    void Update()
    {
        if(timeBtwAttack <= 0) {
            if(Input.GetKey(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                if(enemiesToDamage.Length > 0) {
                    SceneManager.LoadScene(0);
                }
            }

            timeBtwAttack = startTimeBtwAttack;
        } else {
            timeBtwAttack -= Time.deltaTime;
        }

    }
}


