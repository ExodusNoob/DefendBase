using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyClass : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float baseDefense = 5f;

    [Header("Scaled Stats")]
    public float currentHealth;
    public float currentDamage;
    public float currentDefense;

    [Header("Multipliers")]
    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    protected float defenseMultiplier = 1f;

    public float moveSpeed; 

    protected NavMeshAgent agent;
    protected Collider2D targetCollider;
    protected Rigidbody2D rb2D;
    protected Transform target;
    [SerializeField]
    EnemyHealthUI healthUI;
    [SerializeField]
    GameObject healthUIObject;
    bool healthUIActivated = false;

    bool isTowerDefense;
    float nextPathUpdate;
    float pathUpdateInterval = 0.5f; // 2 veces por segundo

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb2D = GetComponent<Rigidbody2D>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    protected virtual void Start()
    {
        isTowerDefense = GameManager.Instance.CurrentMode == GameMode.TowerDefense;

        int level = GameManager.Instance.SelectedEnemyLevel;

        ScaleStats(level);

        healthUIObject.SetActive(false);

        healthUI.Initialize(currentHealth, GameManager.Instance.SelectedEnemyLevel);

        if (GameManager.Instance.CurrentMode == GameMode.BaseDefense)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                target = p.transform;
        }
        else
        {
            GameObject wall = GameObject.FindGameObjectWithTag("Defense");

            if (wall != null)
            {
                target = wall.transform;
                targetCollider = wall.GetComponent<Collider2D>();
            }
        }

        ConfigureByMode();
    }
    void ScaleStats(int level)
    {
        currentHealth = baseHealth * healthMultiplier * (1 + level * 0.1f);
        currentDamage = baseDamage * damageMultiplier * (1 + level * 0.05f);
        currentDefense = baseDefense * defenseMultiplier * (1 + level * 0.03f);
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    protected void ConfigureByMode()
    {
        bool isTowerDefense = GameManager.Instance.CurrentMode == GameMode.TowerDefense;

        if (agent != null)
            agent.enabled = !isTowerDefense;

        if (rb2D != null)
        {
            if (isTowerDefense)
            {
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                rb2D.linearVelocity = Vector2.zero;
                rb2D.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
#if UNITY_EDITOR
        Debug.Log("recibio " + damage);
#endif
        if (currentDefense > 0)
        {
            currentDefense -= damage;

            if (currentDefense < 0)
            {
                float remainingDamage = Mathf.Abs(currentDefense);
                currentDefense = 0;

                currentHealth -= remainingDamage;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void UpdateHealthUI()
    {
        if (!healthUIActivated)
        {
            healthUIObject.SetActive(true);
            healthUIActivated = true;
        }

        healthUI.UpdateHealth(currentHealth);
    }
    protected virtual void Die()
    {
        Debug.Log(name + "murio");
        Destroy(gameObject);
    }
    protected virtual void Move()
    {
        if (isTowerDefense)
            MoveTowerDefense();
        else
            MoveBaseDefense();
    }
    protected void MoveTowerDefense()
    {
        if (rb2D != null) 
            rb2D.linearVelocity = Vector2.left * moveSpeed;
    }

    protected void MoveBaseDefense()
    {
        if (agent != null && target != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = moveSpeed;

            if (Time.time >= nextPathUpdate)
            {
                agent.SetDestination(target.position);
                nextPathUpdate = Time.time + pathUpdateInterval;
            }
        }
    }
    public abstract void Attack(); //cada enemigo lo define de diferente manera
}