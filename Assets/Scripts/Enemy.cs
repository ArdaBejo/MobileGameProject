using System;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 4;
    [SerializeField] int coinReward = 7;
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

        if (GreyboxLane.Instance != null && GreyboxLane.Instance.Player != null && spriteRenderer != null)
        {
            GreyboxLane.Instance.ConfigureEnemy(spriteRenderer);
        }

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    public async void Hit()
    {
        if (!isActiveAndEnabled || health <= 0) return;

        int damage = 1;

        if (Wallet.Instance != null)
            damage += Wallet.Instance.Level;

        health -= damage;

        if (health <= 0)
        {
            GreyboxLane.Instance?.EnemyKilled();
            Wallet.Instance?.Earn(coinReward);
            Despawn();
            return;
        }

        if (spriteRenderer != null)
        {
            CancelHitFeedback();
            var feedback = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            hitFeedback = feedback;
            spriteRenderer.color = Color.red;
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
        if (GreyboxLane.Instance == null) Hit();
    }

    void Update()
    {
        var lane = GreyboxLane.Instance;
        if (lane == null || !lane.Playing || health <= 0) return;
        transform.position = Vector2.MoveTowards(transform.position, lane.Player.position, lane.EnemySpeed * Time.deltaTime);
        if (((Vector2)transform.position - (Vector2)lane.Player.position).sqrMagnitude < 0.5f * 0.5f)
            lane.DamagePlayer();
    }
}
