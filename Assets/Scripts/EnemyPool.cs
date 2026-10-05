using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] int size = 20;

    readonly Stack<Enemy> pool = new();

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            Enemy enemy = Create();
            enemy.gameObject.SetActive(false);
            pool.Push(enemy);
        }
    }

    Enemy Create()
    {
        Enemy enemy = Instantiate(enemyPrefab, transform);
        enemy.Init(this);
        return enemy;
    }

    public Enemy Spawn(Vector3 position)
    {
        Enemy enemy = pool.Count > 0 ? pool.Pop() : Create();

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    public void Release(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Push(enemy);
    }
}