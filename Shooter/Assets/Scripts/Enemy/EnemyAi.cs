using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 100f; // Make sure health is set
    public Transform firePoint;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) 
            Patroling();
        else if (playerInSightRange && !playerInAttackRange) 
            ChasePlayer();
        else if (playerInAttackRange && playerInSightRange) 
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkpoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 2f)
                walkPointSet = false;
        }
    }

    private void SearchWalkpoint()
    {
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ShootAtPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 direction = (player.position - firePoint.position).normalized;
        bullet.transform.forward = direction;
        rb.AddForce(direction * 32f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

   public void TakeDamage(float amount)
{
    health -= amount;
    Debug.Log(gameObject.name + " took damage! Current health: " + health);

    if (health <= 0)
    {
        Die();
    }
}

private void Die()
{
    Debug.Log(gameObject.name + " is dying!"); // Check if this prints

    // Notify the EnemyManager that this enemy is destroyed
    EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
    if (enemyManager != null)
    {
        enemyManager.EnemyDestroyed();
    }

    // Prevents being counted again
    gameObject.tag = "Untagged";

    // Destroy the entire enemy
    Destroy(gameObject);
}
}
