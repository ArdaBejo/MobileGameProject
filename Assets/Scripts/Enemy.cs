using System;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;

    int health;
    EnemyPool pool;

    SpriteRenderer spriteRenderer;
    Color originalColor;
    CancellationTokenSource hitFeedback;

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
        if (!isActiveAndEnabled || health <= 0) return;
        health--;

        if (health <= 0)
        {
            Despawn();
            return;
        }

        if (spriteRenderer != null)
        {
            CancelHitFeedback();
            var feedback = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            hitFeedback = feedback;
            spriteRenderer.color = Color.white;
            try
            {
                await Awaitable.WaitForSecondsAsync(0.1f, feedback.Token);
                spriteRenderer.color = originalColor;
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (hitFeedback == feedback) hitFeedback = null;
                feedback.Dispose();
            }
        }
    }

    void OnDisable() => CancelHitFeedback();

    void CancelHitFeedback()
    {
        var feedback = hitFeedback;
        hitFeedback = null;
        feedback?.Cancel();
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
