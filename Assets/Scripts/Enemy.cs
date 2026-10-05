using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;

    int health;
    EnemyPool pool;

    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public Enemy Init(EnemyPool p)
    {
        pool = p;
        return this;
    }

    void OnEnable()
    {
        health = maxHealth;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    public async void Hit()
    {
        health--;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            await Awaitable.WaitForSecondsAsync(0.1f);
            spriteRenderer.color = originalColor;
        }

        if (health <= 0)
        {
            Despawn();
        }
    }

    public void Despawn()
    {
        if (pool != null)
            pool.Release(this);
        else
            gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        Hit();
    }
}