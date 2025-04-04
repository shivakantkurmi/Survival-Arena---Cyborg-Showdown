using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;            
    public float detectionRadius = 10f; 
    public float attackRadius = 2f;     // Distance at which the enemy attacks the player
    public LayerMask playerLayer;       // Layer for the player

    [Header("Movement Settings")]
    public float patrolSpeed = 2f;      // Speed while patrolling
    public float chaseSpeed = 4f;       // Speed while chasing the player
    public Transform[] patrolPoints;    // Points for the enemy to patrol
    private int currentPatrolIndex = 0; // Tracks the current patrol point

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f; // Time between attacks
    public int attackDamage = 10;       // Damage dealt to the player per attack
    private float nextAttackTime = 0f;  // Tracks when the enemy can attack again

    [Header("References")]
    public NavMeshAgent agent;         // NavMeshAgent for enemy movement
    private Animator animator;          // Animator for enemy animations

    public bool isChasing = false;     // Tracks if the enemy is chasing the player

    void Start()
    {
        // Initialize references
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            // Start patrolling towards the first point
            agent.destination = patrolPoints[currentPatrolIndex].position;
            agent.speed = patrolSpeed;
        }
    }

    void Update()
    {
        // Check if the player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            // If within attack radius, stop moving and attack
            agent.isStopped = true;
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // If within detection radius, start chasing the player
            StartChasing();
        }
        else
        {
            // If out of detection radius, return to patrolling
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

        // Set the player's position as the destination
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
        if (patrolPoints.Length == 0) return; // No patrol points to follow

        // If the enemy has reached the current patrol point, move to the next
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            // Damage the player (assuming the player has a script with a `TakeDamage` method)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Enemy attacked the player for {attackDamage} damage.");
            }

            // Play attack animation
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", !agent.isStopped && agent.velocity.magnitude > 0.1f);
            animator.SetBool("IsChasing", isChasing);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize detection and attack radii in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
