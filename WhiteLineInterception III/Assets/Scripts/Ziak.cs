using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ziak : MonoBehaviour
{
    public Transform player;
    public float repathInterval = 0.25f;
    public float directChaseDistance = 25f;

    NavMeshAgent agent;
    float timer;

    public Vector3 SteeringTarget { get; private set; }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;   // c'est ta physique qui déplace
        agent.updateRotation = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.autoBraking = false;
    }

    void Update()
    {
        // On garde l'agent collé à la vraie position de la voiture
        agent.nextPosition = transform.position;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = repathInterval;

            // Le joueur peut être hors NavMesh (saut, toit...) : on le ramène dessus
            if (NavMesh.SamplePosition(player.position, out var hit, 15f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
        }

        // Si la voie est libre jusqu'au joueur, on fonce droit dessus
        bool clear = !NavMesh.Raycast(transform.position, player.position, out _, NavMesh.AllAreas);
        bool close = (player.position - transform.position).sqrMagnitude < directChaseDistance * directChaseDistance;

        SteeringTarget = (clear && close) ? player.position : agent.steeringTarget;
    }
}