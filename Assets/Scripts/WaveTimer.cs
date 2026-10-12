using System;
using System.Threading;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    [SerializeField] float interval = 2f;
    [SerializeField] EnemyPool pool;

    CancellationTokenSource cts;

    void OnEnable()
    {
        cts = CancellationTokenSource.CreateLinkedTokenSource(
            Application.exitCancellationToken);

        _ = RunAsync(cts.Token);
    }

    void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }

    async Awaitable RunAsync(CancellationToken ct)
    {
        try
        {
            // Let scene Start methods build the player and arena before spawning.
            await Awaitable.NextFrameAsync(ct);
            while (true)
            {
                var lane = GreyboxLane.Instance;
                if (lane != null && !lane.Playing)
                {
                    await Awaitable.NextFrameAsync(ct);
                    continue;
                }

                if (lane == null)
                    pool.Spawn(new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-3f, 3f), 0));
                else if (pool.ActiveCount < 60)
                {
                    Vector2 bounds = lane.HalfSize - Vector2.one * 0.3f;
                    Vector2 position;
                    switch (UnityEngine.Random.Range(0, 4))
                    {
                        case 0: position = new Vector2(-bounds.x, UnityEngine.Random.Range(-bounds.y, bounds.y)); break;
                        case 1: position = new Vector2(bounds.x, UnityEngine.Random.Range(-bounds.y, bounds.y)); break;
                        case 2: position = new Vector2(UnityEngine.Random.Range(-bounds.x, bounds.x), -bounds.y); break;
                        default: position = new Vector2(UnityEngine.Random.Range(-bounds.x, bounds.x), bounds.y); break;
                    }
                    // Avoid contact damage immediately on spawn near a boundary.
                    if ((position - (Vector2)lane.Player.position).sqrMagnitude < 2.25f) position = -position;
                    pool.Spawn(position);
                }

                await Awaitable.WaitForSecondsAsync(lane != null ? lane.SpawnInterval : interval, ct);
            }
        }
        catch (OperationCanceledException) { }
    }
}
