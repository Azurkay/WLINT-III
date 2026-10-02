using UnityEngine;
using UnityEngine.AI;

public class Ziak : MonoBehaviour
{
    [Header("Purchase")]
    [SerializeField] private Transform _player;
    [SerializeField] private float _repathInterval = 0.25f;
    [SerializeField] private float _directChaseDistance = 25f;
    [SerializeField] private float _stopDistance = 6f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider _fL;
    [SerializeField] private WheelCollider _fR;
    [SerializeField] private WheelCollider _rL;
    [SerializeField] private WheelCollider _rR;

    [Header("Wheel Meshs")]
    [SerializeField] private Transform _fLMesh;
    [SerializeField] private Transform _fRMesh;
    [SerializeField] private Transform _rLMesh;
    [SerializeField] private Transform _rRMesh;

    [Header("Driving")]
    [SerializeField] private float _motorTorque = 800f;
    [SerializeField] private float _brakeTorque = 3000f;
    [SerializeField] private float _maxWheelAngle = 30f;
    [SerializeField] private float _maxSpeed = 200f;
    [SerializeField] private float _fullLockAngle = 45f;
    [SerializeField] private Vector3 _centerOfMass = new Vector3(0f, -0.5f, 0f);

    [Header("Anti-Lock")]
    [SerializeField] private float _stuckSpeed = 1f;
    [SerializeField] private float _stuckTime = 1.5f;
    [SerializeField] private float _reverseTime = 1.2f;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    private float _repathTimer;
    private float _stuckTimer;
    private float _reverseTimer;
    private bool _directLine;
    private Vector3 _steeringTarget;

    void Awake()
    {
        _rb.centerOfMass = _centerOfMass;

        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _agent.autoBraking = false;
    }

    void Start()
    {
        _steeringTarget = transform.position;
        _agent.Warp(transform.position);
    }

    void Update()
    {
        UpdatePath();
        SyncWheel(_fL, _fLMesh);
        SyncWheel(_fR, _fRMesh);
        SyncWheel(_rL, _rLMesh);
        SyncWheel(_rR, _rRMesh);
    }

    // ---------- Chemin (NavMesh) ----------
    void UpdatePath()
    {
        if (_player == null) return;

        if (!_agent.isOnNavMesh)
        {
            _steeringTarget = _player.position;
            return;
        }

        _agent.nextPosition = transform.position;

        _repathTimer -= Time.deltaTime;
        if (_repathTimer <= 0f)
        {
            _repathTimer = _repathInterval;

            if (NavMesh.SamplePosition(_player.position, out var _playerHit, 15f, NavMesh.AllAreas))
            {
                _agent.SetDestination(_playerHit.position);

                _directLine = NavMesh.SamplePosition(transform.position, out var carHit, 5f, NavMesh.AllAreas)
                             && !NavMesh.Raycast(carHit.position, _playerHit.position, out _, NavMesh.AllAreas);
            }
        }

        bool close = (_player.position - transform.position).sqrMagnitude
                     < _directChaseDistance * _directChaseDistance;

        _steeringTarget = (_directLine && close) ? _player.position : _agent.steeringTarget;
    }

    // ---------- Conduite ----------
    void FixedUpdate()
    {
        if (_player == null) return;

        Vector3 local = transform.InverseTransformDirection(_steeringTarget - transform.position);
        local.y = 0f;

        float angle = Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
        float speed = Vector3.Dot(_rb.linearVelocity, transform.forward);
        float distTo_player = Vector3.Distance(transform.position, _player.position);

        float steer = Mathf.Clamp(angle / _fullLockAngle, -1f, 1f);
        float throttle;
        float brake = 0f;

        if (_reverseTimer > 0f)
        {
            _reverseTimer -= Time.fixedDeltaTime;
            throttle = -0.7f;
            steer = -steer;
        }
        else
        {
            float turn = Mathf.Clamp01(Mathf.Abs(angle) / 90f);
            float targetSpeed = _maxSpeed * Mathf.Lerp(1f, 0.3f, turn);

            if (speed > targetSpeed) { throttle = 0f; brake = 0.3f; }
            else throttle = 1f;

            if (distTo_player < _stopDistance) { throttle = 0f; brake = 1f; }

            if (Mathf.Abs(speed) < _stuckSpeed && distTo_player > _stopDistance + 2f)
            {
                _stuckTimer += Time.fixedDeltaTime;
                if (_stuckTimer > _stuckTime)
                {
                    _reverseTimer = _reverseTime;
                    _stuckTimer = 0f;
                }
            }
            else _stuckTimer = 0f;
        }

        _fL.steerAngle = steer * _maxWheelAngle;
        _fR.steerAngle = steer * _maxWheelAngle;

        SetWheel(_fL, throttle, brake);
        SetWheel(_fR, throttle, brake);
        SetWheel(_rL, throttle, brake);
        SetWheel(_rL, throttle, brake);
    }

    void SetWheel(WheelCollider w, float throttle, float brake)
    {
        w.motorTorque = throttle * _motorTorque;
        w.brakeTorque = brake * _brakeTorque;
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
        Gizmos.DrawLine(transform.position, _steeringTarget);
        Gizmos.DrawSphere(_steeringTarget, 0.5f);
    }
}