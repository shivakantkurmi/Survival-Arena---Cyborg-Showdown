using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SwordmanAttack : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;  // Reduced for melee range
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    [Header("Movement Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    private float nextAttackTime = 0f;
    public float meleeAttackDuration = 0.5f; // Duration of the melee attack animation

    public Transform swordTransform; // Reference to the sword
    public GameObject sparkEffectPrefab; // Spark effect when sword hits player

    [Header("Score Value")]
    public int score_val = 50;
    private int score;
    public TextMeshProUGUI scoreText;

    [Header("References")]
    public NavMeshAgent agent;
    private Animator animator;

    private bool isChasing = false;
    private bool isDead = false;
    private bool isMeleeAttacking = false;

    void Start()
    {
        score = ScoreManager.score;
        UpdateScoreText();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[currentPatrolIndex].position;
            agent.speed = patrolSpeed;
        }
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            agent.isStopped = true;
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            StartChasing();
        }
        else
        {
            StopChasing();
            Patrol();
        }

        UpdateAnimator();
    }

    void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
        }

        agent.isStopped = false;
        agent.destination = player.position;
    }

    void StopChasing()
    {
        if (isChasing)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    void AttackPlayer()
    {
        AimSwordAtPlayer();

        if (Time.time >= nextAttackTime && !isMeleeAttacking)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (animator != null)
            {
                animator.SetBool("IsMelee", true);  // Trigger melee attack animation
                isMeleeAttacking = true;
                Invoke(nameof(ExecuteMeleeAttack), meleeAttackDuration); // Execute attack after animation duration
            }
        }
    }

    void ExecuteMeleeAttack()
    {
        // Handle sword collision with player and apply damage
        Collider[] hitColliders = Physics.OverlapSphere(swordTransform.position, 0.5f, playerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                    // Spawn spark effect on hit
                    Instantiate(sparkEffectPrefab, hitCollider.transform.position, Quaternion.identity);
                }
            }
        }

        // Reset melee attack state and animation
        isMeleeAttacking = false;
        animator.SetBool("IsMelee", false);
    }

    void AimSwordAtPlayer()
    {
        if (player == null) return;

        // Rotate the sword to point toward the player
        Vector3 directionToPlayer = (player.position - swordTransform.position).normalized;
        Quaternion swordLookRotation = Quaternion.LookRotation(directionToPlayer);
        swordTransform.rotation = Quaternion.Slerp(swordTransform.rotation, swordLookRotation, Time.deltaTime * 10f);

        // Smoothly rotate the enemy toward the player
        Vector3 enemyDirectionToPlayer = (player.position - transform.position).normalized;
        Quaternion enemyLookRotation = Quaternion.LookRotation(enemyDirectionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, enemyLookRotation, Time.deltaTime * 3f);
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            if (isDead)
            {
                animator.SetBool("IsDead", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);
                animator.SetBool("IsMelee", false);
                return;
            }

            if (!isChasing && !isDead && agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("IsWalking", true);
            }
            else if (!isChasing && agent.velocity.magnitude < 0.1f)
            {
                animator.SetBool("IsWalking", false);
            }

            animator.SetBool("IsChasing", isChasing);
        }
    }

    public void Die()
    {
        if (isDead) return;
        ScoreManager.AddScore(score_val);
        score = ScoreManager.score;
        UpdateScoreText();
        isDead = true;
        agent.isStopped = true;
        animator.SetBool("IsDead", true);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsMelee", false);
        animator.SetBool("IsChasing", false);

        Destroy(gameObject, 3f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {ScoreManager.score}";
    }
}



