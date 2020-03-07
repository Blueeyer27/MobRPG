using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject player;
    public List<AIController> enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (enemies != null && enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.OnSelected += OnEnemySelected;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnemySelected(AIController enemyController)
    {
        player.GetComponentInChildren<ThirdPersonController>().target = enemyController.transform;
    }
}
