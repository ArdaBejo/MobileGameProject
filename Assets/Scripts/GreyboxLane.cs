using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GreyboxLane : MonoBehaviour
{
    public static GreyboxLane Instance { get; private set; }
    [SerializeField] GameManager manager;
    [SerializeField] EnemyPool pool;
    [SerializeField] Material spriteMaterialTemplate;
    [SerializeField] float duration = 120f;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] float shotInterval = 0.4f;
    public Vector2 HalfSize { get; } = new Vector2(4.6f, 6.4f);
    public Transform Player { get; private set; }
    public bool Playing => Player != null && manager.CurrentState == GameManager.State.Playing && !LifecycleGuard.IsPaused && !finished;
    public int Wave => Mathf.Min(3, 1 + Mathf.FloorToInt(elapsed / 40f));
    public float SpawnInterval => Wave == 1 ? 2f : Wave == 2 ? 1.35f : 0.85f;
    public float EnemySpeed => Wave == 1 ? 1f : Wave == 2 ? 1.25f : 1.5f;

    Sprite square;
    Material spriteMaterial;
    SpriteRenderer playerRenderer;
    Camera gameplayCamera;
    int dragTouchId = -1;
    bool draggingMouse;
    readonly List<RaycastResult> uiHits = new();
    TMP_Text status;
    TMP_Text instruction;
    AudioSource audioSource;
    AudioClip click;
    int health;
    int kills;
    float elapsed;
    float nextShot;
    float immunityUntil;
    bool finished;
    int lastSecond = -1;
    readonly Shot[] shots = new Shot[32];

    class Shot
    {
        public SpriteRenderer Renderer;
        public Vector2 Velocity;
        public float Life;
    }

    void Awake()
    {
        Instance = this;
        health = maxHealth;
    }

    void Start()
    {
        square = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), new Vector2(0.5f, 0.5f), Texture2D.whiteTexture.width);
        spriteMaterial = new Material(spriteMaterialTemplate);
        var camera = Camera.main;
        gameplayCamera = camera;
        camera.orthographic = true;
        camera.transform.position = new Vector3(0, 0, -10);
        camera.orthographicSize = Mathf.Max(9f, (HalfSize.x + 0.7f) / camera.aspect);
        camera.backgroundColor = new Color(0.025f, 0.035f, 0.05f);
        Block("Floor", Vector2.zero, HalfSize * 2, new Color(0.1f, 0.12f, 0.15f), -10);
        Block("TopBoundary", new Vector2(0, HalfSize.y), new Vector2(HalfSize.x * 2 + 0.2f, 0.15f), Color.gray, -5);
        Block("BottomBoundary", new Vector2(0, -HalfSize.y), new Vector2(HalfSize.x * 2 + 0.2f, 0.15f), Color.gray, -5);
        Block("LeftBoundary", new Vector2(-HalfSize.x, 0), new Vector2(0.15f, HalfSize.y * 2), Color.gray, -5);
        Block("RightBoundary", new Vector2(HalfSize.x, 0), new Vector2(0.15f, HalfSize.y * 2), Color.gray, -5);
        playerRenderer = Block("Player", Vector2.zero, Vector2.one * 0.45f, new Color(0.2f, 0.7f, 1f), 5);
        Player = playerRenderer.transform;
        for (int i = 0; i < shots.Length; i++)
        {
            var renderer = Block("PistolShot", Vector2.zero, Vector2.one * 0.13f, new Color(1, 0.85f, 0.3f), 6);
            renderer.gameObject.SetActive(false);
            shots[i] = new Shot { Renderer = renderer };
        }
        CreateAudio();
        CreateUI();
        RefreshHUD();
    }

    SpriteRenderer Block(string name, Vector2 position, Vector2 size, Color color, int order)
    {
        var obj = new GameObject(name, typeof(SpriteRenderer));
        obj.transform.SetParent(transform, false);
        obj.transform.position = position;
        obj.transform.localScale = new Vector3(size.x, size.y, 1);
        var renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = square;
        renderer.sharedMaterial = spriteMaterial;
        renderer.color = color;
        renderer.sortingOrder = order;
        return renderer;
    }

    public void ConfigureEnemy(SpriteRenderer renderer)
    {
        renderer.sprite = square;
        renderer.sharedMaterial = spriteMaterial;
        renderer.sortingOrder = 4;
        renderer.transform.localScale = Vector3.one * 0.45f;
    }

    void CreateUI()
    {
        var ui = GreyboxUI.Canvas("GameplayCanvas", -1);
        status = GreyboxUI.Label(ui, "RunStatus", "", new Vector2(0.5f, 1), new Vector2(0, -65), new Vector2(680, 95), 26);
        instruction = GreyboxUI.Label(ui, "Instructions", "Drag the blue player to survive. Auto-fire is on.", new Vector2(0.5f, 1), new Vector2(0, -145), new Vector2(660, 80), 23);
        GreyboxUI.PauseButton(ui, manager.PauseGame);
    }

    void Update()
    {
        if (!Playing)
        {
            ResetDrag();
            return;
        }
        elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
        if (elapsed >= duration) { Finish(true); return; }
        MovePlayer();
        playerRenderer.color = Time.time < immunityUntil && Mathf.FloorToInt(Time.time * 12) % 2 == 0 ? Color.white : new Color(0.2f, 0.7f, 1f);
        if (Time.time >= nextShot)
        {
            Fire();
            nextShot = Time.time + shotInterval;
        }
        UpdateShots();
        if (lastSecond != Mathf.FloorToInt(elapsed)) RefreshHUD();
        if (elapsed > 10 && instruction.gameObject.activeSelf) instruction.gameObject.SetActive(false);
    }

    void MovePlayer()
    {
        if (TryDragPosition(out var draggedPosition))
        {
            SetPlayerPosition(Vector2.MoveTowards(Player.position, draggedPosition, movementSpeed * Time.deltaTime));
            return;
        }
        Vector2 input = Vector2.zero;
        var keyboard = Keyboard.current;
        if (keyboard != null)
            input += new Vector2((keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1 : 0),
                (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1 : 0));
        Vector2 position = (Vector2)Player.position + Vector2.ClampMagnitude(input, 1) * movementSpeed * Time.deltaTime;
        SetPlayerPosition(position);
    }

    void SetPlayerPosition(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, -HalfSize.x + 0.3f, HalfSize.x - 0.3f);
        position.y = Mathf.Clamp(position.y, -HalfSize.y + 0.3f, HalfSize.y - 0.3f);
        Player.position = position;
    }

    bool TryDragPosition(out Vector2 position)
    {
        position = Player.position;
        var touchscreen = Touchscreen.current;
        if (touchscreen != null)
        {
            foreach (var touch in touchscreen.touches)
            {
                if (dragTouchId == -1 && touch.press.wasPressedThisFrame && CanStartDrag(touch.position.ReadValue()))
                    dragTouchId = touch.touchId.ReadValue();
                if (dragTouchId == -1 || touch.touchId.ReadValue() != dragTouchId) continue;
                if (!touch.press.isPressed) { dragTouchId = -1; continue; }
                position = PointerWorldPosition(touch.position.ReadValue());
                return true;
            }
            dragTouchId = -1;
        }

        // Mouse drag is also available for Editor testing. Mobile uses touch only.
        if (Application.isMobilePlatform) return false;
        var mouse = Mouse.current;
        if (mouse == null) return false;
        if (mouse.leftButton.wasPressedThisFrame && CanStartDrag(mouse.position.ReadValue())) draggingMouse = true;
        if (!mouse.leftButton.isPressed) draggingMouse = false;
        if (!draggingMouse) return false;
        position = PointerWorldPosition(mouse.position.ReadValue());
        return true;
    }

    bool CanStartDrag(Vector2 screenPosition)
    {
        if ((PointerWorldPosition(screenPosition) - (Vector2)Player.position).sqrMagnitude > 0.65f * 0.65f) return false;
        if (EventSystem.current != null)
        {
            uiHits.Clear();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = screenPosition }, uiHits);
            if (uiHits.Count > 0) return false;
        }
        return true;
    }

    Vector2 PointerWorldPosition(Vector2 screenPosition) => gameplayCamera.ScreenToWorldPoint(
        new Vector3(screenPosition.x, screenPosition.y, -gameplayCamera.transform.position.z));

    void ResetDrag()
    {
        dragTouchId = -1;
        draggingMouse = false;
    }

    void OnEnable() => LifecycleGuard.PausedChanged += OnPauseChanged;
    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPauseChanged;
        ResetDrag();
    }
    void OnPauseChanged(bool paused) => ResetDrag();

    void Fire()
    {
        Enemy nearest = pool.Nearest(Player.position);
        if (nearest == null) return;
        foreach (var shot in shots)
        {
            if (shot.Life > 0) continue;
            shot.Renderer.transform.position = Player.position;
            shot.Velocity = ((Vector2)nearest.transform.position - (Vector2)Player.position).normalized * 9f;
            shot.Life = 1.5f;
            shot.Renderer.gameObject.SetActive(true);
            audioSource.PlayOneShot(click, 0.12f);
            return;
        }
    }

    void UpdateShots()
    {
        foreach (var shot in shots)
        {
            if (shot.Life <= 0) continue;
            Vector2 from = shot.Renderer.transform.position;
            Vector2 to = from + shot.Velocity * Time.deltaTime;
            var hit = pool.FirstHit(from, to, 0.3f);
            shot.Renderer.transform.position = to;
            shot.Life -= Time.deltaTime;
            if (hit != null) { hit.Hit(); shot.Life = 0; }
            if (shot.Life <= 0) shot.Renderer.gameObject.SetActive(false);
        }
    }

    public void DamagePlayer()
    {
        if (!Playing || Time.time < immunityUntil) return;
        health--;
        immunityUntil = Time.time + 1f;
        audioSource.PlayOneShot(click, 0.6f);
        Haptics.Pulse();
        RefreshHUD();
        if (health <= 0) Finish(false);
    }

    public void EnemyKilled()
    {
        kills++;
        RefreshHUD();
    }

    void RefreshHUD()
    {
        if (status == null) return;
        lastSecond = Mathf.FloorToInt(elapsed);
        int remaining = Mathf.CeilToInt(duration - elapsed);
        status.text = $"HP {health}/{maxHealth}    Kills {kills}\nWave {Wave}/3    Survive {remaining / 60:00}:{remaining % 60:00}";
    }

    void Finish(bool won)
    {
        if (finished) return;
        finished = true;
        LaneResult.Record(won, elapsed, kills);
        if (won) manager.WinGame(); else manager.LoseGame();
    }

    void CreateAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        click = AudioClip.Create("GreyboxClick", 1103, 1, 22050, false);
        var samples = new float[1103];
        for (int i = 0; i < samples.Length; i++) samples[i] = Mathf.Sin(i * 2 * Mathf.PI * 700 / 22050) * (1f - (float)i / samples.Length);
        click.SetData(samples, 0);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (square != null) Destroy(square);
        if (spriteMaterial != null) Destroy(spriteMaterial);
        if (click != null) Destroy(click);
    }
}
