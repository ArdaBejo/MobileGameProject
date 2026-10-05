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
        cts.Cancel();
        cts.Dispose();
    }

    async Awaitable RunAsync(CancellationToken ct)
{
    try
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(
                UnityEngine.Random.Range(-4f, 4f),
                UnityEngine.Random.Range(-3f, 3f),
                0f
            );

            pool.Spawn(spawnPos);

            await Awaitable.WaitForSecondsAsync(interval, ct);
        }
    }
    catch (OperationCanceledException)
    {
    }
}
}