using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private int currentWaypointIndex = 0; // Keeps track of the current waypoint

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }


    public Path path; // Reference to the Path script

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85;
    public float eyeHeight;

    [Header("Weapond Values")]
    public Transform gunBarrel;
    [Range(0.1f,10f)]
    public float fireRate;

    public float waypointReachedThreshold = 0.5f; // How close the NPC needs to be to a waypoint
    public bool isLooping = true; // Enables looping through waypoints

    [SerializeField]
    private string currentState;
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        // Validate path setup
        if (path == null || path.waypoints == null || path.waypoints.Count == 0)
        {
            Debug.LogError("Path or waypoints are not set correctly!");
            return;
        }

        stateMachine.Initialise();
        MoveToNextWaypoint(); // Start moving to the first waypoint

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (agent == null || path == null || path.waypoints.Count == 0)
            return;

        // Check if the NPC has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= waypointReachedThreshold)
        {
            MoveToNextWaypoint();
        }

        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    private void MoveToNextWaypoint()
    {
        if (path.waypoints.Count == 0) return;

        // Set the next waypoint as the destination
        agent.SetDestination(path.waypoints[currentWaypointIndex].position);

        // Update to the next waypoint index
        currentWaypointIndex++;

        if (currentWaypointIndex >= path.waypoints.Count)
        {
            if (isLooping)
            {
                // Reset to the first waypoint
                currentWaypointIndex = 0;
            }
            else
            {
                // Stop moving if not looping
                agent.isStopped = true;
            }
        }
    }

    public bool CanSeePlayer(){

        if(player != null){
        
        if(Vector3.Distance(transform.position, player.transform.position) < sightDistance){
            
            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView){

                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);

                RaycastHit hitInfo = new RaycastHit();

                if(Physics.Raycast(ray, out hitInfo, sightDistance)){

                    if(hitInfo.transform.gameObject == player){

                        return true;
                    }
                }
                Debug.DrawRay(ray.origin, ray.direction * sightDistance);
            }


        }
    
  }
  return false;
}
}
