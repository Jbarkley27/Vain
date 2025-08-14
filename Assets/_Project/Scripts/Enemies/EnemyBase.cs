using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;


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
    public bool IsSetup = false;
    public EnemySpawner Spawner;
    public bool CanRotate = true;
    public Planet planetOrigin;
    public StatusEffectEnemyManager statusEffectEnemyManager;
    public MeshFlashEffect meshFlashEffect;
    public GameObject visual;


    [Header("AI Settings")]
    public float _attackRange = 5f;
    public float _lookAtSpeed = 5f;
    public float _wanderNodeCloseRange = 2f;
    public float _stoppingDistance;
    public float _avoidanceRadius;
    public float _baseAcceleration = 20f;
    public float minSpeed;
    public float maxSpeed;
    public float _newScentNodeInterval = 2;
    public Coroutine newScentCo;

    [System.Serializable]
    public struct Reward
    {
        public Resource.ResourceType resourceType;
        public int amount;
    }

    [Header("Rewards")]
    public List<Reward> Rewards = new List<Reward>();


    [Header("Attacks")]
    public List<AttackDataBase> AvailableAttacks = new List<AttackDataBase>();
    public bool IsAttacking { get; set; } = false;
    public bool CanAttack { get; set; } = true;
    public float _timeBeforeAttackMin;
    public float _timeBeforeAttackMax;

    [SerializeField] private float _castTimeMin;
    [SerializeField] private float _castTimeMax;
    public float CastTime;

    [SerializeField] private float _coolDownMin;
    [SerializeField] private float _coolDownMax;
    public float CoolDown;



    [Header("Sight")]
    public float sightRadius = 10f;       // How far the enemy can see
    public float visionWidth = 1f;        // Radius of the "vision cone" (sphere)
    // public LayerMask detectionMask;       // What the enemy can see (e.g., Player layer)
    public GameObject gameObjectInView;


    [Header("Runtime Debug")]
    public Transform target;
    public GameObject _currentWanderNode;
    public Transform _currentScentNode;
    public bool CanSeePlayer;

    [Header("UI")]
    [SerializeField] private GameObject healthBarPrefab;
    public EnemyHealthUI healthUI;

    [Header("Health")] // TODO : Move to Stats Class
    public int MaxHealth;
    public int CurrentHealth;




    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        // Let physics handle movement
        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }

    private void Update()
    {
        if (!IsSetup) return;
        CanSeePlayer = CanEnemySeePlayer();

        if (Spawner) planetOrigin = Spawner.planet;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsSetup) return;
        HandleState();

        // Continuously update path if target moves
        // if (target != null && _agent != null && _agent.isOnNavMesh && _currentState != EnemyState.Wander && !IsAttacking)
        //     _agent.SetDestination(target.position);

        // Use physics to move the enemy
        UsePhysicsToMove();
    }






    // Pooling | Instantiate Logic ------------------------------------------------------------------------

    public virtual void Setup(int worldTier, Transform playerTarget, EnemySpawner spawner, bool DebugMode = false)
    {
        gameObject.name = "Testing 1";
        if (IsSetup) return;

        gameObject.name = "Testing  === Winner";
        if (DebugMode)
        {
            Spawner = spawner;
            // Setup Health
            CurrentHealth = MaxHealth;
            // Setup UI
            CreateHealthUI();
            return;
        }

        // randomize some of the agent properties so there is some variation in the enemy movement
        _agent.speed = Random.Range(minSpeed, maxSpeed);
        _agent.stoppingDistance = Random.Range(_stoppingDistance, _stoppingDistance + 5);
        _agent.acceleration = Random.Range(_baseAcceleration, _baseAcceleration + 60);
        _agent.radius = Random.Range(_avoidanceRadius, _avoidanceRadius + 1);
        Spawner = spawner;
        CanRotate = true;



        // set other stuff
        _rb = GetComponent<Rigidbody>();
        _wanderNodeCloseRange = Random.Range(_wanderNodeCloseRange, _wanderNodeCloseRange + 2);
        _attackRange += _agent.stoppingDistance;
        _stoppingDistance = _agent.stoppingDistance;
        _attackRange = Random.Range(_attackRange, _attackRange + 20);
        _lookAtSpeed = Random.Range(_lookAtSpeed, _lookAtSpeed + 10f);
        target = playerTarget;
        _currentState = EnemyState.Wander;
        gameObject.name = gameObject.name + Random.Range(1f, 4f) + "";
        _newScentNodeInterval = Random.Range(_newScentNodeInterval, _newScentNodeInterval + 3);
        CastTime = Random.Range(_castTimeMin, _castTimeMax);
        CoolDown = Random.Range(_coolDownMin, _coolDownMax);



        // Requirements to enter Wander state initially
        _currentWanderNode = Spawner.GetRandomWanderNodePosition();
        _agent.SetDestination(_currentWanderNode.transform.position);
        if (newScentCo == null) newScentCo = StartCoroutine(GetRandomScentNode());


        // Setup Health
        CurrentHealth = MaxHealth;


        // Setup UI
        CreateHealthUI();

        IsSetup = true;
    }

    public virtual void OnSpawned()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = true;
        gameObject.SetActive(true);
        if (healthUI) healthUI.gameObject.SetActive(true);
    }

    public virtual void OnDespawned()
    {
        _agent.enabled = false;
        _currentState = EnemyState.Wander;
        if (healthUI) healthUI.gameObject.SetActive(false);
    }


    void CreateHealthUI()
    {
        GameObject uiInstance = Instantiate(
            healthBarPrefab,
            EnemyUIManager.Instance.enemyUICanvas.transform
        );

        healthUI = uiInstance.GetComponent<EnemyHealthUI>();
        healthUI.SetTarget(this); // `this` = enemy

        // connect the status effect system with the healthUI
        statusEffectEnemyManager.statusEffectRoot = healthUI.statusEffectRoot;
    }


    public void TakeDamage(int damage, StatusEffectBase.StatusEffectType statusEffectType = StatusEffectBase.StatusEffectType.NONE, int statusApplicationChance = 0)
    {
        Debug.Log("enemy taking damage | Status Effect: " + statusEffectType.ToString());
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        healthUI.UpdateHealthText();
        meshFlashEffect.FlashAll(new MeshFlashEffect.FlashData(ColorManager.Instance.normalHitFlashMat, .1f));

        if (CurrentHealth <= 0)
        {
            InitiateDeath();
        }

        // if not dead, check if a status effect chance was passed
        if (statusEffectType == StatusEffectBase.StatusEffectType.NONE) return;

        // we know there is a passed status effect chance event
        if (Random.Range(0, 100) < statusApplicationChance)
        {
            Debug.Log("Adding Status to enemy");
            statusEffectEnemyManager.AddStatus(statusEffectType, StatusEffectPlayerManager.Instance.GetStatusPrefab(statusEffectType));
        }

    }



    // AI Navigation State Logic ------------------------------------------------------------------------------
    protected virtual void HandleState()
    {
        switch (_currentState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Seek:
                Seek();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }



    protected virtual void Wander()
    {
        _agent.stoppingDistance = 1f;
        _agent.updateRotation = true;

        // This will cause the enemy to choose a random wander Node and move there
        // until the player is within its range.
        if (Vector3.Distance(transform.position, _agent.destination) <= _wanderNodeCloseRange)
        {
            _currentWanderNode = Spawner.GetRandomWanderNodePosition();
            target = _currentWanderNode.transform;
            _agent.SetDestination(_currentWanderNode.transform.position);
        }


        if (GlobalDataStore.Instance.PlanetDetector.CurrentPlanetObject != null)
        {
            _currentScentNode = EnemyManager.Instance.GetRandomPlayerScentNode(); 
            target = _currentWanderNode.transform;
            _currentState = EnemyState.Seek;
        }
    }


    protected virtual void Seek()
    {
        RotateTowards(GlobalDataStore.Instance.Player.transform.position);
        _agent.updateRotation = false;
        _agent.stoppingDistance = _stoppingDistance;
        target = GlobalDataStore.Instance.Player.transform;

        // SWITCH TO WANDER
        if (target == null)
        {
            _currentState = EnemyState.Wander;
            return;
        }

        // SWITCH TO ATTACK
        if (IsPlayerInAttackRangeOfPlayer())
        {
            _currentState = EnemyState.Attack;
            return;
        }

        if (_agent && _agent.isOnNavMesh) _agent.SetDestination(_currentScentNode.transform.position);
    }


    public void UsePhysicsToMove()
    {
        if (_agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance)
            return;

        // Get direction toward next NavMesh corner
        Vector3 dir = (_agent.steeringTarget - transform.position).normalized;

        // Apply physics movement
        _rb.AddForce(dir * _agent.speed, ForceMode.Acceleration);

        // Limit max speed
        if (_rb.linearVelocity.magnitude > maxSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;

        // Rotate toward movement
        // if (dir.sqrMagnitude > 0.01f)
        // {
        //     Quaternion lookRot = Quaternion.LookRotation(dir);
        //     _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, lookRot, Time.fixedDeltaTime * 5f));
        // }
    }
    





    // Attack State Logic --------------------------------------------------------------------------------

    protected virtual void Attack()
    {
        RotateTowards(GlobalDataStore.Instance.Player.transform.position);
        _agent.updateRotation = false;


        if (!IsPlayerInAttackRangeOfPlayer())
        {
            CanAttack = true;
            IsAttacking = false;
            _currentState = EnemyState.Seek;
        }

        // attack
        if (CanAttack && !IsAttacking)
        {
            // StartCoroutine(InitializeAttack());
            Debug.Log($"{gameObject.name} is starting an attack.");
        }


        if (!IsAttacking) _agent.SetDestination(_currentScentNode.transform.position);
    }



    public IEnumerator InitializeAttack()
    {
        CanAttack = false;
        IsAttacking = true;

        SetCanMove(false);


        AttackDataBase randomAttack = AvailableAttacks[Random.Range(0, AvailableAttacks.Count)];
        if (randomAttack == null) yield break;

        yield return new WaitForSeconds(Random.Range(_timeBeforeAttackMin, _timeBeforeAttackMax)
            + CastTime);


        yield return StartCoroutine(randomAttack.Execute(this));
        SetCanMove(true);

        yield return new WaitForSeconds(CoolDown);
    }







    // HELPERS ------------------------------------------------------------------------------------------

    protected bool IsPlayerInRange(float range)
    {
        return target && Vector3.Distance(transform.position, target.position) <= range;
    }



    private bool IsPlayerInAttackRangeOfPlayer()
    {
        return Vector3.Distance(transform.position, target.position) <= _attackRange;
    }


    private void RotateTowards(Vector3 targetDirection)
    {

        Quaternion targetRotation = Quaternion.LookRotation((GlobalDataStore.Instance.Player.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _lookAtSpeed);
    }


    private bool CanEnemySeePlayer()
    {
        RaycastHit hit;

        // Perform a SphereCast forward
        bool hitSomething = Physics.SphereCast(
            origin: transform.position,
            radius: visionWidth,
            direction: transform.forward,
            hitInfo: out hit,
            maxDistance: sightRadius
        // layerMask: detectionMask
        );

        if (hitSomething)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                gameObjectInView = hit.collider.gameObject;
                return true;
            }
        }

        return false;
    }

    public void SetCanMove(bool canMove)
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = !canMove;
            _agent.updatePosition = canMove;
            _agent.updateRotation = canMove;
        }
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




    private IEnumerator GetRandomScentNode()
    {
        while (true)
        {
            yield return new WaitForSeconds(_newScentNodeInterval);

            // Current Scent Node
            ScentNode oldNode = _currentScentNode == null ? null : _currentScentNode.gameObject.GetComponent<ScentNode>();
            _currentScentNode = EnemyManager.Instance.GetRandomPlayerScentNode(oldNode);

            // New Speed
            _agent.speed = Random.Range(minSpeed, maxSpeed);


        }
    }


    public void InitiateDeath()
    {
        CanRotate = false;
        Debug.Log($"{gameObject.name} has died.");
        _currentState = EnemyState.Dead;
        _agent.enabled = false;
        ExplosionManager.Instance.CreateExplosion(gameObject.transform.position, ExplosionManager.ExplosionType.SMALL);
        gameObject.SetActive(false);
        IsSetup = false;
        if (healthUI) healthUI.gameObject.SetActive(false);
        DropRewards();
        Spawner.DespawnEnemy(gameObject);
    }


    public void DropRewards()
    {
        foreach (var reward in Rewards)
        {
            for (int i = 0; i < reward.amount; i++)
            {
                ResourceManager.Instance.SpawnResource(reward.resourceType, transform.position);
            }
        }
    }



    public enum EnemyState
    {
        Wander,
        Seek,
        Attack,
        Reposition,
        Dead
    }
}
