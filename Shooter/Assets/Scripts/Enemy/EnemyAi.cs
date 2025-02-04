using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
   
   public NavMeshAgent agent;

   public Transform player;

   public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    public Transform firePoint;

   //Patroling 
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
    
    private void Awake(){

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update(){
// Check for sight and attack range
    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    if (!playerInSightRange && !playerInAttackRange) 
        Patroling();
    else if (playerInSightRange && !playerInAttackRange) 
        ChasePlayer();
    else if (playerInAttackRange && playerInSightRange) 
        AttackPlayer();

        if (health <= 0)
        {
            // Notify EnemyManager when this enemy is destroyed
            EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.EnemyDestroyed();
            }

             // Remove the tag so it isn't counted
            gameObject.tag = "Untagged";

            // Destroy the ENTIRE parent object
           Destroy(gameObject.transform.root.gameObject);
        }

    }


    private void Patroling(){

        if (!walkPointSet) SearchWalkpoint();

    if (walkPointSet) // Fix condition here
    {
        agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }

    }

    private void SearchWalkpoint(){

        //Calculate random point in range
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;

    }

    private void ChasePlayer(){

        agent.SetDestination(player.position);

    }

    private void AttackPlayer()
{
    // Stop movement while attacking
    agent.SetDestination(transform.position);

    // Look at the player
    transform.LookAt(player);

    if (!alreadyAttacked)
    {
        ShootAtPlayer(); // Call shooting function

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
}

private void ShootAtPlayer()
{
    GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
    Rigidbody rb = bullet.GetComponent<Rigidbody>();

    // Ensure the bullet faces the player
    Vector3 direction = (player.position - firePoint.position).normalized;
    
    // Make sure the bullet's rotation is correct
    bullet.transform.forward = direction;

    // Apply force only in the forward direction
    rb.AddForce(direction * 32f, ForceMode.Impulse);
}

    private void ResetAttack(){

        alreadyAttacked = false;

    }


}