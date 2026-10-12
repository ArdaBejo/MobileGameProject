using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] int size = 20;

    readonly Stack<Enemy> pool = new();
    readonly HashSet<Enemy> activeEnemies = new();
    public int ActiveCount => activeEnemies.Count;

    public Enemy Nearest(Vector2 position)
    {
        Enemy nearest = null;
        float distance = float.PositiveInfinity;
        foreach (var enemy in activeEnemies)
        {
            float candidate = ((Vector2)enemy.transform.position - position).sqrMagnitude;
            if (candidate < distance) { nearest = enemy; distance = candidate; }
        }
        return nearest;
    }

    public Enemy FirstHit(Vector2 from, Vector2 to, float radius)
    {
        Vector2 segment = to - from;
        float lengthSquared = segment.sqrMagnitude;
        float first = float.PositiveInfinity;
        Enemy hit = null;
        foreach (var enemy in activeEnemies)
        {
            Vector2 position = enemy.transform.position;
            float t = lengthSquared > 0 ? Mathf.Clamp01(Vector2.Dot(position - from, segment) / lengthSquared) : 0;
            if ((position - (from + segment * t)).sqrMagnitude <= radius * radius && t < first)
            {
                first = t;
                hit = enemy;
            }
        }
        return hit;
    }

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
        activeEnemies.Add(enemy);
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    public void Release(Enemy enemy)
    {
        if (enemy == null || !activeEnemies.Remove(enemy)) return;
        enemy.gameObject.SetActive(false);
        pool.Push(enemy);
    }
}
