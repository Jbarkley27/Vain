using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 *Create Scent Nodes 
 *Player enters planetary region
 *Enemies are activated
 *Should go into wander state until player is in planetary zone
 *If player leaves planetary zone go back into wander state
 *If player enters planetary zone, move into seek state
 *If enemy can see player ( nothing is obstructing view ) AND is in attack range move to attack state
 *If enemy can't see player but is in attack range stay in seek state and move to predefined Or random roaming points
 *If enemy can't see player and is in attack range stay in seek state and ??? it needs to move potentially

 * Player has points around them that enemies will use so that they all don't go directly to player, but the points
 * will be close.

*/

/// <summary>
/// Base class for all enemies — handles health, movement, and pooling lifecycle.
/// Specific enemy types will inherit from this.
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IPoolable
{
    [Header("General")]
    public EnemyID EnemyID;
    public EnemyState _currentState = EnemyState.Wander;
    public Rigidbody _rb;
    protected NavMeshAgent _agent;

    [Header("AI Settings")]
    public float _detectionRange = 15f;
    public float _attackRange = 5f;
    public float _lookAtSpeed = 5f;
    public float _wanderNodeCloseRange = 2f;
    public float _stoppingDistance;
    public float _avoidanceRadius;
    public float _baseAcceleration = 20f;

    [Header("Runtime Debug")]
    public Transform target;
    public GameObject _currentWanderNode;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {
        HandleState();
    }



    // Pooling | Instantiate Logic ------------------------------------------------------------------------

    /// <summary>
    /// Called by the spawner after the enemy is spawned.
    /// </summary>
    public virtual void Setup(int worldTier, Transform playerTarget)
    {
        // randomize some of the agent properties so there is some variation in the enemy movement
        _agent.speed = Random.Range(35f, 50f);
        _agent.stoppingDistance = Random.Range(_stoppingDistance, _stoppingDistance + 10);
        _agent.acceleration = Random.Range(_baseAcceleration, _baseAcceleration + 20);
        _agent.radius = Random.Range(_avoidanceRadius, _avoidanceRadius + 1);

        // set other stuff
        _wanderNodeCloseRange = Random.Range(_wanderNodeCloseRange, _wanderNodeCloseRange + 2);
        _lookAtSpeed = Random.Range(_lookAtSpeed, _lookAtSpeed + 2f);
        _rb = GetComponent<Rigidbody>();
        target = playerTarget;
        _currentState = EnemyState.Wander;
        gameObject.name = gameObject.name + Random.Range(1f, 4f) + "";

        // Requirements to enter Wander state initially
        _currentWanderNode = EnemyManager.Instance.GetRandomWanderNodePosition();
        _agent.SetDestination(_currentWanderNode.transform.position);
    }

    public virtual void OnSpawned()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void OnDespawned()
    {
        _agent.enabled = false;
        _currentState = EnemyState.Wander;
    }

    protected virtual void HandleState()
    {
        switch (_currentState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Seek:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }




    // AI Navigation Logic


    // protected virtual void LookForPlayer()
    // {
    //     if (IsPlayerInRange(_detectionRange))
    //     {
    //         _currentState = EnemyState.Seek;
    //     }
    // }

    protected virtual void Wander()
    {
        _agent.stoppingDistance = 1f;

        // This will cause the enemy to choose a random wander Node and move there
        // until the player is within its range.
        if (Vector3.Distance(transform.position, _agent.destination) <= _wanderNodeCloseRange)
        {
            // Debug.Log(gameObject.name + " is getting a new Wander Node -- " + gameObject.name);
            _currentWanderNode = EnemyManager.Instance.GetRandomWanderNodePosition();
            _agent.SetDestination(_currentWanderNode.transform.position);
        }

        // Debug.Log(gameObject.name + " distance from " + _currentWanderNode.name + " is " + Vector3.Distance(transform.position, agent.destination) + " and my wanderNodeCloseRange = " + _wanderNodeCloseRange);

        // if (_currentWanderNode) RotateTowards(_currentWanderNode.transform.position);


        // if (IsPlayerInRange(detectionRange))
        // {
        //     currentState = EnemyState.Seek;
        // }
    }


    public void RotateTowards(Vector3 targetDirection)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            _rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            _lookAtSpeed * Time.deltaTime // Interpolation factor
        );

        // Apply the smooth rotation to the Rigidbody
        _rb.MoveRotation(smoothedRotation);
    }

    protected virtual void ChasePlayer()
    {
        if (target == null)
        {
            _currentState = EnemyState.Wander;
            return;
        }

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > _detectionRange)
        {
            _currentState = EnemyState.Wander;
            return;
        }

        if (dist <= _attackRange)
        {
            _currentState = EnemyState.Attack;
            _agent.ResetPath();
            return;
        }

        _agent.SetDestination(target.position);
    }

    protected virtual void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking.");
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > _attackRange)
        {
            _currentState = EnemyState.Seek;
        }
    }

    protected bool IsPlayerInRange(float range)
    {
        return target && Vector3.Distance(transform.position, target.position) <= range;
    }

    protected Vector3 GetRandomNavPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            randomPos.y = 100f;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 200f, NavMesh.AllAreas))
                return hit.position;
        }

        return center;
    }

    public enum EnemyState
    {
        Wander,
        Seek,
        Attack,
        Dead
    }
}
