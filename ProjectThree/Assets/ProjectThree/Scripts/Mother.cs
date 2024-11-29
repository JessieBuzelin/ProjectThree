using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Mother : MonoBehaviour
{
    public Transform player;
    public float sightRange = 10f;
    public float fieldOfView = 90f;
    public float wanderRadius = 15f;
    public float wanderTime = 3f;
    public float timeToStopChasing = 5f; // Stops chasing after given time
    private NavMeshAgent agent;
    private float timeSinceLastWander = 0f;
    private float timeSinceLostSight = 0f; // how long player has been out of sight
    private bool isChasing = false; // is chasing player or not
    private PlayerIsHiding playerIsHiding; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

      
        playerIsHiding = player.GetComponent<PlayerIsHiding>();

       
        if (playerIsHiding == null)
        {
            Debug.LogWarning("PlayerIsHiding script not found on Player object.");
        }
    }

    void Update()
    {
       
        WanderRandomly();

        
       

        // If the player is not hiding, check if they're in sight and chase them
        if (PlayerInSight())
        {
            FollowPlayer();
            timeSinceLostSight = 0f;
            isChasing = true;
        }
        else
        {
            
            if (isChasing)
            {
                timeSinceLostSight += Time.deltaTime;

                if (timeSinceLostSight >= timeToStopChasing)
                {
                    StopChasing();
                }
            }
        }
    }

    bool PlayerInSight()
    {
        // Check if the player is within the sight range and not hiding
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfView / 2)
            {
                // Raycast to check if player is in sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, sightRange))
                {
                    if (hit.transform == player)
                    {
                        return true; 
                    }
                }
            }
        }
        return false; 
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.position);
    }

    void StopChasing()
    {
        
        isChasing = false;
        agent.ResetPath();
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Call the reset game function when collision occurs
            ResetGame();
        }
    }
    void ResetGame()
    {
        string HuntingSystem = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(HuntingSystem); // Reload the current scene
    }

    void WanderRandomly()
    {
        
        timeSinceLastWander += Time.deltaTime;

        if (timeSinceLastWander >= wanderTime)
        {
            
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            timeSinceLastWander = 0f;
        }
    }
}
