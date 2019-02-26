using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
    
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.


    void Awake()
    {
        // Set up the references.
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
        if (true)//player) //si existe muevete?
        {
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination(Vector3.zero);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    }
}