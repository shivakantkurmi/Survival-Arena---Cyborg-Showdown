// before adding iscybermonster logic  ****************************************************************



// using TMPro;
// using UnityEngine;
// using UnityEngine.AI;

// public class CybeMonsterAttack : MonoBehaviour
// {
//     [Header("Detection Settings")]
//     public Transform player;
//     public float detectionRadius = 10f;
//     public float attackRadius = 7f;
//     public LayerMask playerLayer;
//     public LayerMask obstacleLayer;

//     [Header("Movement Settings")]
//     public float patrolSpeed = 2f;
//     public float chaseSpeed = 4f;
//     public Transform[] patrolPoints;
//     private int currentPatrolIndex = 0;

//     [Header("Attack Settings")]
//     public float attackCooldown = 1.5f;
//     public int attackDamage = 10;
//     private float nextAttackTime = 0f;
//     public float raycastRange = 10f;

//     public Transform gunTransform;

//     [Header("Score Value")]
//     public int score_val = 50;
//     private int score;
//     public TextMeshProUGUI scoreText;

//     [Header("Medikit")]
//     public GameObject mediKit;
//     public int heightOffset = 3;
//     public bool isDeadAnimation = true;

//     [Header("References")]
//     public NavMeshAgent agent;
//     private Animator animator;

//     private bool isChasing = false;
//     private bool isDead = false;
//     private bool isShooting = false;

//     void Start()
//     {
//         score = ScoreManager.score;
//         UpdateScoreText();

//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponent<Animator>();

//         if (patrolPoints.Length > 0)
//         {
//             agent.destination = patrolPoints[currentPatrolIndex].position;
//             agent.speed = patrolSpeed;
//         }

//     }

//     void Update()
//     {
//         if (isDead) return;

//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         if (distanceToPlayer <= attackRadius)
//         {
//             agent.isStopped = true;
//             AttackPlayer();
//         }
//         else if (distanceToPlayer <= detectionRadius)
//         {
//             StartChasing();
//         }
//         else
//         {
//             StopChasing();
//             Patrol();
//         }

//         UpdateAnimator();
//     }

//     void StartChasing()
//     {
//         if (!isChasing)
//         {
//             isChasing = true;
//             agent.speed = chaseSpeed;
//         }

//         agent.isStopped = false;
//         agent.destination = player.position;
//     }

//     void StopChasing()
//     {
//         if (isChasing)
//         {
//             isChasing = false;
//             agent.speed = patrolSpeed;
//         }
//     }

//     void Patrol()
//     {
//         if (patrolPoints.Length == 0) return;

//         if (!agent.pathPending && agent.remainingDistance < 0.5f)
//         {
//             currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
//             agent.destination = patrolPoints[currentPatrolIndex].position;
//         }
//     }

//     void AttackPlayer()
//     {
//         AimGunAtPlayer();

//         if (Time.time >= nextAttackTime)
//         {
//             nextAttackTime = Time.time + attackCooldown;

//             if (CanSeePlayer())
//             {
//                 if (animator != null && !isShooting)
//                 {
//                     animator.SetBool("IsShooting", true);
//                     isShooting = true;
//                 }

//                 RaycastHit hit;
//                 Vector3 gunPosition = gunTransform.position;

//                 // Cast a ray from the gun toward the player's position
//                 if (Physics.Raycast(gunPosition, (player.position - gunPosition).normalized, out hit, raycastRange))
//                 {

//                     if (hit.collider.CompareTag("Player"))
//                     {
//                         Debug.Log("Player hit by enemy raycast!");

//                         PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
//                         if (playerHealth != null)
//                         {
//                             playerHealth.TakeDamage(attackDamage);
//                         }
//                     }
//                 }

//                 Debug.DrawRay(gunPosition, (player.position - gunPosition).normalized * raycastRange, Color.red, 0.1f);
//             }
//         }
//     }

//     void AimGunAtPlayer()
//     {
//         if (player == null) return;

//         // Rotate the gun to point toward the player
//         Vector3 directionToPlayer = (player.position - gunTransform.position).normalized;
//         Quaternion gunLookRotation = Quaternion.LookRotation(directionToPlayer);
//         gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunLookRotation, Time.deltaTime * 10f);

//         // Smoothly rotate the enemy toward the player
//         Vector3 enemyDirectionToPlayer = (player.position - transform.position).normalized;
//         Quaternion enemyLookRotation = Quaternion.LookRotation(enemyDirectionToPlayer);
//         transform.rotation = Quaternion.Slerp(transform.rotation, enemyLookRotation, Time.deltaTime * 3f);
//     }

//     bool CanSeePlayer()
//     {
//         RaycastHit hit;
//         Vector3 direction = (player.position - gunTransform.position).normalized;
//         if (Physics.Raycast(gunTransform.position, direction, out hit, raycastRange))
//         {
//             if (hit.collider.CompareTag("Player"))
//             {
//                 return true;
//             }
//         }
//         return false;
//     }

//     void UpdateAnimator()
//     {
//         if (animator != null)
//         {
//             if (isDead)
//             {
//                 animator.SetBool("IsDead", true);
//                 animator.SetBool("IsWalking", false);
//                 animator.SetBool("IsChasing", false);
//                 animator.SetBool("IsShooting", false);
//                 return;
//             }

//             if (!isChasing && !isDead && agent.velocity.magnitude > 0.1f)
//             {
//                 animator.SetBool("IsWalking", true);
//             }
//             else if (!isChasing && agent.velocity.magnitude < 0.1f)
//             {
//                 animator.SetBool("IsWalking", false);
//             }

//             animator.SetBool("IsChasing", isChasing);

//             if (isShooting && Time.time >= nextAttackTime)
//             {
//                 animator.SetBool("IsShooting", false);
//                 isShooting = false;
//             }

//             if (isChasing)
//             {
//                 animator.SetBool("IsWalking", false);
//             }
//         }
//     }

//     public void Die()
//     {
//         if (isDead) return;
//         ScoreManager.AddScore(score_val);
//         score = ScoreManager.score;
//         UpdateScoreText();
//         isDead = true;
//         agent.isStopped = true;
//         animator.SetBool("IsDead", true);
//         animator.SetBool("IsWalking", false);
//         animator.SetBool("IsShooting", false);
//         animator.SetBool("IsChasing", false);
//         UnityEngine.Vector3 newPosition = transform.position + new UnityEngine.Vector3(0, heightOffset, 0);
//         if (isDeadAnimation)
//         {
//             Destroy(gameObject, 3f);
//         }
//         else
//         {
//             Destroy(gameObject);
//             if (mediKit)
//             {
//                 Instantiate(mediKit, newPosition, transform.rotation);
//             }
//         }

//     }
//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, detectionRadius);

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, attackRadius);
//     }

//     void UpdateScoreText()
//     {
//         scoreText.text = $"Score: {ScoreManager.score}";
//     }
// }





using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CybeMonsterAttack : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRadius = 7f;
    public float meleeRange = 3f;          // Melee range
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
    public int meleeDamage = 20;          // Melee damage
    private float nextAttackTime = 0f;
    public float raycastRange = 10f;

    public Transform gunTransform;

    [Header("Score Value")]
    public int score_val = 50;
    private int score;
    public TextMeshProUGUI scoreText;

    [Header("Medikit")]
    public GameObject mediKit;
    public int heightOffset = 3;
    public bool isDeadAnimation = true;

    [Header("References")]
    public NavMeshAgent agent;
    private Animator animator;

    [Header("Cyber Monster Logic")]
    public bool isCyberMonster = false;   // Flag for Cyber Monster

    private bool isChasing = false;
    private bool isDead = false;
    private bool isShooting = false;
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

        if (isCyberMonster && distanceToPlayer <= meleeRange)
        {
            PerformMeleeAttack(); // Melee attack logic
        }
        else if (distanceToPlayer <= attackRadius)
        {
            agent.isStopped = true;
            AttackPlayer(); // Ranged attack logic
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
        if (isMeleeAttacking) return; // Skip ranged attack if melee is active

        AimGunAtPlayer();

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (CanSeePlayer())
            {
                if (animator != null && !isShooting)
                {
                    animator.SetBool("IsShooting", true);
                    animator.SetBool("IsMelee", false); // Ensure melee is off
                    isShooting = true;
                    isMeleeAttacking = false;
                }

                RaycastHit hit;
                Vector3 gunPosition = gunTransform.position;

                // Cast a ray from the gun toward the player's position
                if (Physics.Raycast(gunPosition, (player.position - gunPosition).normalized, out hit, raycastRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player hit by enemy raycast!");

                        PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(attackDamage);
                        }
                    }
                }

                Debug.DrawRay(gunPosition, (player.position - gunPosition).normalized * raycastRange, Color.red, 0.1f);
            }
        }
    }

    void PerformMeleeAttack()
    {
        if (Time.time < nextAttackTime) return; // Wait for cooldown
        nextAttackTime = Time.time + attackCooldown;

        agent.isStopped = true;

        if (animator != null)
        {
            animator.SetBool("IsShooting", false); // Ensure shooting is off
            animator.SetBool("IsMelee", true);     // Enable melee animation
            isShooting = false;
            isMeleeAttacking = true;
        }

        // Deal damage to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= meleeRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage); // Apply melee damage
                Debug.Log("Player hit by melee attack!");
            }
        }
    }

    void AimGunAtPlayer()
    {
        if (player == null) return;

        // Rotate the gun to point toward the player
        Vector3 directionToPlayer = (player.position - gunTransform.position).normalized;
        Quaternion gunLookRotation = Quaternion.LookRotation(directionToPlayer);
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunLookRotation, Time.deltaTime * 10f);

        // Smoothly rotate the enemy toward the player
        Vector3 enemyDirectionToPlayer = (player.position - transform.position).normalized;
        Quaternion enemyLookRotation = Quaternion.LookRotation(enemyDirectionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, enemyLookRotation, Time.deltaTime * 3f);
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - gunTransform.position).normalized;
        if (Physics.Raycast(gunTransform.position, direction, out hit, raycastRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
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
                animator.SetBool("IsShooting", false);
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

            if (isShooting && Time.time >= nextAttackTime)
            {
                animator.SetBool("IsShooting", false);
                isShooting = false;
            }

            if (isMeleeAttacking && Time.time >= nextAttackTime)
            {
                animator.SetBool("IsMelee", false);
                isMeleeAttacking = false;
            }
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
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsChasing", false);

        UnityEngine.Vector3 newPosition = transform.position + new UnityEngine.Vector3(0, heightOffset, 0);

        if (isDeadAnimation)
        {
            Destroy(gameObject, 3f);
        }
        else
        {
            Destroy(gameObject);
            if (mediKit)
            {
                Instantiate(mediKit, newPosition, transform.rotation);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeRange); // Visualize melee range
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {ScoreManager.score}";
    }
}
