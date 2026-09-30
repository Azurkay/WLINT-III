using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Ziak : MonoBehaviour
{
    [Header("Cible")]
    public Transform player;
    public float repathInterval = 0.25f;
    public float directChaseDistance = 25f;
    public float stopDistance = 6f;

    [Header("Roues (colliders)")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Roues (visuel, optionnel)")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Conduite")]
    public float motorTorque = 800f;
    public float brakeTorque = 3000f;
    public float maxWheelAngle = 30f;
    public float maxSpeed = 35f;          // m/s (35 = 126 km/h)
    public float fullLockAngle = 45f;
    public Vector3 centerOfMass = new Vector3(0f, -0.5f, 0f);

    [Header("Anti-blocage")]
    public float stuckSpeed = 1f;
    public float stuckTime = 1.5f;
    public float reverseTime = 1.2f;

    NavMeshAgent agent;
    Rigidbody rb;
    float repathTimer, stuckTimer, reverseTimer;
    bool directLine;
    Vector3 steeringTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.autoBraking = false;
    }

    void Start()
    {
        steeringTarget = transform.position;
        agent.Warp(transform.position);
    }

    void Update()
    {
        UpdatePath();
        SyncWheel(frontLeft, frontLeftMesh);
        SyncWheel(frontRight, frontRightMesh);
        SyncWheel(rearLeft, rearLeftMesh);
        SyncWheel(rearRight, rearRightMesh);
    }

    // ---------- Chemin (NavMesh) ----------
    void UpdatePath()
    {
        if (player == null) return;

        if (!agent.isOnNavMesh)
        {
            steeringTarget = player.position;
            return;
        }

        agent.nextPosition = transform.position;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = repathInterval;

            if (NavMesh.SamplePosition(player.position, out var playerHit, 15f, NavMesh.AllAreas))
            {
                agent.SetDestination(playerHit.position);

                directLine = NavMesh.SamplePosition(transform.position, out var carHit, 5f, NavMesh.AllAreas)
                             && !NavMesh.Raycast(carHit.position, playerHit.position, out _, NavMesh.AllAreas);
            }
        }

        bool close = (player.position - transform.position).sqrMagnitude
                     < directChaseDistance * directChaseDistance;

        steeringTarget = (directLine && close) ? player.position : agent.steeringTarget;
    }

    // ---------- Conduite ----------
    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 local = transform.InverseTransformDirection(steeringTarget - transform.position);
        local.y = 0f;

        float angle = Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
        float speed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        float steer = Mathf.Clamp(angle / fullLockAngle, -1f, 1f);
        float throttle;
        float brake = 0f;

        if (reverseTimer > 0f)
        {
            reverseTimer -= Time.fixedDeltaTime;
            throttle = -0.7f;
            steer = -steer;
        }
        else
        {
            float turn = Mathf.Clamp01(Mathf.Abs(angle) / 90f);
            float targetSpeed = maxSpeed * Mathf.Lerp(1f, 0.3f, turn);

            if (speed > targetSpeed) { throttle = 0f; brake = 0.3f; }
            else throttle = 1f;

            if (distToPlayer < stopDistance) { throttle = 0f; brake = 1f; }

            if (Mathf.Abs(speed) < stuckSpeed && distToPlayer > stopDistance + 2f)
            {
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer > stuckTime)
                {
                    reverseTimer = reverseTime;
                    stuckTimer = 0f;
                }
            }
            else stuckTimer = 0f;
        }

        frontLeft.steerAngle = steer * maxWheelAngle;
        frontRight.steerAngle = steer * maxWheelAngle;

        SetWheel(frontLeft, throttle, brake);
        SetWheel(frontRight, throttle, brake);
        SetWheel(rearLeft, throttle, brake);
        SetWheel(rearRight, throttle, brake);
    }

    void SetWheel(WheelCollider w, float throttle, float brake)
    {
        w.motorTorque = throttle * motorTorque;
        w.brakeTorque = brake * brakeTorque;
    }

    void SyncWheel(WheelCollider col, Transform mesh)
    {
        if (col == null || mesh == null) return;
        col.GetWorldPose(out var pos, out var rot);
        mesh.SetPositionAndRotation(pos, rot);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, steeringTarget);
        Gizmos.DrawSphere(steeringTarget, 0.5f);
    }
}