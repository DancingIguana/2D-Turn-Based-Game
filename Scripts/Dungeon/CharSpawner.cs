using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    [SerializeField] GameObject character;
    [SerializeField] GameObject enemyChar;
    private GameObject player;
    private GameObject enemy;
    void Start()
    {
        player = Instantiate(character);
        player.transform.position = Characters.instance.playerPos;
        for(int i = 0; i < Characters.instance.enemyList.EnemyList.Count; i++)
        {
            enemy = Instantiate(enemyChar);
            enemy.transform.position = Characters.instance.enemyPos[i];
            var enemyData = enemy.GetComponent<Enemy>();
            enemyData.player = player.transform;
            enemyData.enemyFighter = Characters.instance.enemyList.EnemyList[i];
        }

        mainCamera.GetComponent<followPlayer>().player = player.transform;
    }
}
