using UnityEngine;

public sealed class RunWeapons : MonoBehaviour
{
    sealed class Rocket
    {
        public SpriteRenderer Renderer;
        public Vector2 Velocity;
        public float Life;
        public int Damage;
        public float Radius;
    }

    GreyboxLane lane;
    EnemyPool pool;
    RunProgression progression;
    readonly Rocket[] rockets = new Rocket[4];
    LineRenderer sword, blast, shield;
    float nextSword, nextRocket, swordUntil, blastUntil;

    public void Initialize(GreyboxLane arena, EnemyPool enemies, RunProgression upgrades)
    {
        lane = arena;
        pool = enemies;
        progression = upgrades;
        for (int i = 0; i < rockets.Length; i++)
        {
            var renderer = lane.Block("Rocket", Vector2.zero, new Vector2(0.18f, 0.38f),
                new Color(1, 0.43f, 0.18f), 7);
            renderer.gameObject.SetActive(false);
            rockets[i] = new Rocket { Renderer = renderer };
        }
        sword = Ring("LaserSweep", new Color(0.35f, 1, 1), 0.12f);
        blast = Ring("RocketBlast", new Color(1, 0.55f, 0.16f), 0.14f);
        shield = Ring("Shield", new Color(0.4f, 0.7f, 1), 0.05f);
    }

    LineRenderer Ring(string name, Color color, float width)
    {
        var root = new GameObject(name);
        root.transform.SetParent(transform, false);
        var line = root.AddComponent<LineRenderer>();
        line.sharedMaterial = lane.Player.GetComponentInChildren<SpriteRenderer>().sharedMaterial;
        line.useWorldSpace = false;
        line.loop = true;
        line.widthMultiplier = width;
        line.positionCount = 32;
        line.startColor = line.endColor = color;
        line.sortingOrder = 7;
        for (int i = 0; i < line.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2 / line.positionCount;
            line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0));
        }
        line.enabled = false;
        return line;
    }

    void Update()
    {
        if (lane == null || !lane.Playing) return;
        var position = lane.Player.position;
        shield.enabled = progression.ShieldCharges > 0;
        shield.transform.position = position;
        shield.transform.localScale = Vector3.one * 0.85f;
        sword.enabled = Time.time < swordUntil;
        sword.transform.position = position;
        blast.enabled = Time.time < blastUntil;

        if (progression.SwordLevel > 0 && Time.time >= nextSword)
        {
            float radius = 1.35f + progression.SwordLevel * 0.15f;
            sword.transform.localScale = Vector3.one * radius;
            swordUntil = Time.time + 0.18f;
            sword.enabled = true;
            pool.DamageInRadius(position, radius, CombatBalance.Sword(progression.SwordLevel, progression.DamageLevel));
            nextSword = Time.time + Mathf.Max(0.5f, 1.7f - progression.SwordLevel * 0.15f);
        }
        if (progression.RocketLevel > 0 && Time.time >= nextRocket)
        {
            FireRocket();
            nextRocket = Time.time + Mathf.Max(1.4f, 3.5f - progression.RocketLevel * 0.3f);
        }
        foreach (var rocket in rockets)
        {
            if (rocket.Life <= 0) continue;
            Vector2 from = rocket.Renderer.transform.position;
            Vector2 to = from + rocket.Velocity * Time.deltaTime;
            rocket.Life -= Time.deltaTime;
            var enemy = pool.FirstHit(from, to, 0.35f);
            rocket.Renderer.transform.position = to;
            if (enemy != null)
            {
                Vector3 center = enemy.transform.position;
                rocket.Life = 0;
                blast.transform.position = center;
                blast.transform.localScale = Vector3.one * rocket.Radius;
                blastUntil = Time.time + 0.22f;
                blast.enabled = true;
                pool.DamageInRadius(center, rocket.Radius, rocket.Damage);
            }
            if (rocket.Life <= 0) rocket.Renderer.gameObject.SetActive(false);
        }
    }

    void FireRocket()
    {
        var enemy = pool.Nearest(lane.Player.position);
        if (enemy == null) return;
        foreach (var rocket in rockets)
        {
            if (rocket.Life > 0) continue;
            Vector2 direction = ((Vector2)enemy.transform.position - (Vector2)lane.Player.position).normalized;
            rocket.Renderer.transform.position = lane.Player.position;
            rocket.Renderer.transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
            rocket.Velocity = direction * 6;
            rocket.Life = 2.5f;
            rocket.Damage = CombatBalance.Rocket(progression.RocketLevel, progression.DamageLevel);
            rocket.Radius = 1.1f + progression.RocketLevel * 0.2f;
            rocket.Renderer.gameObject.SetActive(true);
            return;
        }
    }
}
