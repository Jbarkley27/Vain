using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


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
    public bool IsSetup = false;
    public EnemySpawner Spawner;
    public StatusEffectEnemyManager statusEffectEnemyManager;
    public MeshFlashEffect meshFlashEffect;



    [Header("AI Behaviour Settings")]
    // public float _attackRange = 5f;
    public float _lookAtSpeed = 5f;
    public float minSpeed;
    public float maxSpeed;
    public float _speed;
    public float _SeekSpeedMultiplier = 1.5f;



    [Tooltip("This controls how frequently the enemy will change either player scent node or wander node.\n The lower the less time is also spent sitting still")]
    public float _newScentNodeInterval = 2;
    public float _newWanderNodeInterval = 5;
    // public Coroutine newScentCo;
    private SphereCollider _detectionCollider;
    public float _minWanderNodeDistance = 50f;
    public float _maxWanderNodeDistance = 100f;
    public float _minPlayerScentNodeDistance = 20f;
    public float _maxPlayerScentNodeDistance = 50f;
    [Range(10, 100000)] public float maxVelocity = 1000f;



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
    [SerializeField] private float _coolDownMin;
    [SerializeField] private float _coolDownMax;
    public float CoolDown;
    public int AttackCost = 1;



    [Header("Sight")]
    public float sightRange = 10f;       // How far the enemy can see
    public float visionWidth = 1f;        // Radius of the "vision cone" (sphere)
    // public LayerMask detectionMask;       // What the enemy can see (e.g., Player layer)
    public GameObject gameObjectInView;


    [Header("Runtime Debug")]
    public Vector3 target;
    public Vector3 _currentWanderNode;
    public Vector3 _currentScentNode;
    public bool CanSeePlayer;
    public int rayCount = 12;
    public float rayLength = 10f;
    public LayerMask hitLayers;
    public Gradient normalColor;
    public Gradient hitColor;
    public float hitForce = 10f;
    private LineRenderer[] lineRenderers;
    public Vector3 avoidanceForce;
    public float seekStateDetectionRadiusMultiplier = .8f;



    [Header("UI")]
    [SerializeField] private GameObject healthBarPrefab;
    public EnemyHealthUI healthUI;



    [Header("Health")] // TODO : Move to Stats Class
    public int MaxHealth;
    public int CurrentHealth;



    public enum EnemyState
    {
        Wander,
        Seek,
        Attack,
        Reposition,
        Dead
    }


    void Start()
    {
        CreateEntityDetection();
        StartCoroutine(RandomizeMovement());
        sightRange = Random.Range(sightRange - 10, sightRange + 15f);
    }



    private void Update()
    {
        if (!IsSetup) return;
        CanSeePlayer = CanEnemySeePlayer();
        target = GetCurrentTargetPosition();

        // if (Spawner) planetOrigin = Spawner.planet;

        // make it so that if the enemy has a current scent node, it will always create a line following it
        // that can be seen in the game view by using a line renderer but only make it last for a frame
        CreateMovementDebug();

    }



    protected virtual void FixedUpdate()
    {
        if (!IsSetup) return;
        HandleState();
        UsePhysicsToMove();
        HandleEntityDetection();
    }




    // Debugging ------------------------------------------------------------------------------------------
    public void CreateMovementDebug()
    {
        if (!EnemyManager.Instance.ShowEnemyDebugRays)
        {
            return;
        }


        // if in seek mode show enemy sightLength as a line renderer
        if (_currentState == EnemyState.Seek)
        {
            GameObject lineObject = new GameObject("Enemy Sight Line");
            lineObject.transform.parent = transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * sightRange);
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
            Destroy(lineObject, .1f); // Destroy after a frame
        }



        if (_currentState == EnemyState.Seek)
        {
            // Create a line connecting the enemy to the new scent node
            GameObject lineObject = new GameObject("Player Scent Node Line");
            lineObject.transform.parent = transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, _currentScentNode);
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            Destroy(lineObject, .1f); // Destroy after a frame
        }

        

        if (_currentState == EnemyState.Wander)
        {
            // Create a line connecting the enemy to the new scent node
            GameObject lineObject = new GameObject("Wander Node Line");
            lineObject.transform.parent = transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.4f;
            lineRenderer.endWidth = 0.4f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, _currentWanderNode);
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.magenta;
            lineRenderer.endColor = Color.magenta;
            Destroy(lineObject, .1f); // Destroy after a frame
        }
    }


    public void CreateEntityDetection()
    {
        // Setup LineRenderers
        lineRenderers = new LineRenderer[rayCount];
        for (int i = 0; i < rayCount; i++)
        {
            GameObject lrObj = new GameObject("Ray_" + i);
            lrObj.transform.parent = transform;
            LineRenderer lr = lrObj.AddComponent<LineRenderer>();

            lr.positionCount = 2;
            lr.startWidth = 0.4f;
            lr.endWidth = 0.4f;
            lr.material = new Material(Shader.Find("Sprites/Default")); // simple shader
            lr.colorGradient = normalColor;

            lineRenderers[i] = lr;
        }

        rayLength = Random.Range(rayLength, rayLength * 1.5f);
    }


    public void HandleEntityDetection()
    {
        if (!EnemyManager.Instance.ShowEnemyDebugRays)
        {
            foreach (var lr in lineRenderers)
            {
                lr.enabled = false;
            }
        }
        else
        {
            foreach (var lr in lineRenderers)
            {
                lr.enabled = true;
            }
        }

        float tempRayLength = _currentState == EnemyState.Seek ? rayLength * seekStateDetectionRadiusMultiplier : rayLength;

        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Direction in XZ plane
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            // Ray start & end
            Vector3 start = transform.position;
            Vector3 end = start + dir * tempRayLength;

            if (Physics.Raycast(start, dir, out RaycastHit hit, tempRayLength, hitLayers))
            {
                // Update line to stop at hit point
                end = hit.point;
                lineRenderers[i].colorGradient = hitColor;

                // Apply force (if object has rigidbody)
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 enemyPos = transform.position;
                    Vector3 hitPos = hit.point;

                    float distance = Vector3.Distance(enemyPos, hitPos);

                    // Calculate a force that pushes away from the hit point
                    Vector3 pushDirection = (enemyPos - hitPos).normalized;
                    float pushForce = (tempRayLength - distance) / tempRayLength * hitForce; // Stronger force when closer
                    avoidanceForce = pushDirection * pushForce;
                    // Debug.Log($"Ray {i} hit {hit.collider.name} at distance {hit.distance} applying avoidance force {avoidanceForce} Push Direction {pushDirection} Push Force {pushForce}");
                }
            }
            else
            {
                lineRenderers[i].colorGradient = normalColor;
            }

            // Update line positions
            lineRenderers[i].SetPosition(0, start);
            lineRenderers[i].SetPosition(1, end);
        }
    }









    // Pooling | Instantiate Logic ------------------------------------------------------------------------
    public virtual void Setup(int worldTier, Transform playerTarget, EnemySpawner spawner, bool DebugMode = false)
    {
        if (IsSetup) return;

        if (DebugMode)
        {
            Spawner = spawner;
            // Setup Health
            CurrentHealth = MaxHealth;
            // Setup UI
            CreateHealthUI();
            return;
        }

        Spawner = spawner;


        // set other stuff
        _rb = GetComponent<Rigidbody>();
        // _attackRange = Random.Range(_attackRange, _attackRange + 20);
        _lookAtSpeed = Random.Range(_lookAtSpeed, _lookAtSpeed + 10f);
        _currentState = EnemyState.Wander;
        gameObject.name = gameObject.name + " " + Random.Range(1, 20);
        _newScentNodeInterval = Random.Range(_newScentNodeInterval, _newScentNodeInterval + 3);
        // CastTime = Random.Range(_castTimeMin, _castTimeMax);
        CoolDown = Random.Range(_coolDownMin, _coolDownMax);
        _speed = Random.Range(minSpeed, maxSpeed);


        _detectionCollider = GlobalDataStore.Instance.PlanetCollider;

        // Requirements to enter Wander state initially
        _currentWanderNode = RandomPointInCircle();

        // Setup Health
        CurrentHealth = MaxHealth;

        // Setup UI
        CreateHealthUI();

        IsSetup = true;

        // Start generating new nodes
        StartCoroutine(GenerateNewNodes());
    }

    public virtual void OnSpawned()
    {
        gameObject.SetActive(true);
        if (healthUI) healthUI.gameObject.SetActive(true);
    }

    public virtual void OnDespawned()
    {
        _currentState = EnemyState.Wander;
        if (healthUI) healthUI.gameObject.SetActive(false);
    }











    // Health & Damage Logic ------------------------------------------------------------------------------

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
        // Debug.Log("enemy taking damage | Status Effect: " + statusEffectType.ToString());
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
            // Debug.Log("Adding Status to enemy");
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







    // Wander State Logic ------------------------------------
    protected virtual void Wander()
    {
        RotateTowards(_currentWanderNode);

        // animation
        if (floatTween == null) StartFloatingAnimation();

        // SWITCH TO SEEK
        if (Spawner.isPlayerInZone)
        {
            _currentState = EnemyState.Seek;
            PlaySeekAnimation();
            _currentScentNode = GetRandomPositionAroundPlayer();
            return;
        }
    }










    // Seek State Logic --------------------------------------

    protected virtual void Seek()
    {
        RotateTowards(GlobalDataStore.Instance.Player.transform.position);

        // SWITCH TO WANDER
        if (!Spawner.isPlayerInZone)
        {
            // Wait a few seconds before switching to wander
            StartCoroutine(FlipBoolAfterTime(Random.Range(3f, 5f), Spawner.isPlayerInZone));
            _currentState = EnemyState.Wander;
            return;
        }


        // SWITCH TO ATTACK
        if (CanEnemySeePlayer() && IsPlayerInAttackRangeOfPlayer() && !IsAttacking && CanAttack)
        {
            if (!EnemyManager.Instance.TryToAttack(AttackCost)) return;
            // _currentState = EnemyState.Attack;
            IsAttacking = true;
            StartCoroutine(InitializeAttack());
        }
    }









    // Attack State Logic -------------------------------

    protected virtual void Attack()
    {
        // RotateTowards(GlobalDataStore.Instance.Player.transform.position);
        // if (CanAttack) StartCoroutine(InitializeAttack());
    }



    public IEnumerator InitializeAttack()
    {
        CanAttack = false;

        AttackDataBase randomAttack = AvailableAttacks[Random.Range(0, AvailableAttacks.Count)];
        if (randomAttack == null) yield break;


        // yield return PlayAttackAnimation();

        yield return StartCoroutine(randomAttack.Execute(this));

        // wait a bit after attack
        yield return new WaitForSeconds(.8f);

        // _currentState = EnemyState.Seek;
        EnemyManager.Instance.EnemyFinishedAttack(AttackCost);
        IsAttacking = false;

        yield return new WaitForSeconds(CoolDown);
        CanAttack = true;
    }











    // HELPERS ------------------------------------------------------------------------------------------

    protected bool IsPlayerTooClose(float range)
    {
        return Vector3.Distance(transform.position, target) <= range;
    }



    private bool IsPlayerInAttackRangeOfPlayer()
    {
        return Vector3.Distance(transform.position, target) <= sightRange;
    }



    private void RotateTowards(Vector3 targetDirection)
    {
        float finalSpeed = _currentState == EnemyState.Wander ? _lookAtSpeed * .5f : _lookAtSpeed;
        Quaternion targetRotation = Quaternion.LookRotation((targetDirection - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * finalSpeed);
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
            maxDistance: sightRange
        );

        if (hitSomething)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                gameObjectInView = hit.collider.gameObject;
                CanSeePlayer = true;
                return true;
            }

            CanSeePlayer = false;
        }

        return false;
    }



    private IEnumerator RandomizeMovement()
    {
        while (enabled)
        {
            if (_currentState == EnemyState.Wander)
            {
                // New Speed
                _speed = Random.Range(minSpeed, maxSpeed);
            }

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }


    public IEnumerator FlipBoolAfterTime(float time, bool boolean)
    {
        yield return new WaitForSeconds(time);
        boolean = !boolean;
    }










    // Nodes ------------------------------------------------------------------------------------------
    public Vector3 RandomPointInCircle()
    {
        Vector3 center = _detectionCollider.transform.TransformPoint(_detectionCollider.center);

        float angle = Random.Range(0f, Mathf.PI * 2f);

        // pick distance inside [min, max]
        float distance = Random.Range(_minWanderNodeDistance, _maxWanderNodeDistance);

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        return new Vector3(center.x + x, center.y, center.z + z);
    }



    private IEnumerator GenerateNewNodes()
    {
        while (enabled)
        {
            if (_currentState == EnemyState.Wander)
            {
                float random = Random.Range(_newWanderNodeInterval, _newWanderNodeInterval + 5);
                yield return new WaitForSeconds(random);

                // Debug.Log("Getting new wander node");
                // Current Scent Node
                _currentWanderNode = RandomPointInCircle();

                // Create a sphere at the new scent node position
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.GetComponent<Collider>().enabled = false; // Disable collider
                sphere.transform.position = _currentWanderNode;
                sphere.transform.localScale = Vector3.one * 3f; // Adjust size as needed
                sphere.GetComponent<Renderer>().material.color = Color.blue; // Set color to red for visibility
                if (!EnemyManager.Instance.ShowEnemyDebugRays)
                {
                    // hide the sphere if debug rays are not on
                    Destroy(sphere);
                    continue;
                }

                Destroy(sphere, random);
            }
            else
            {
                float random = Random.Range(_newScentNodeInterval, _newScentNodeInterval + 5);
                yield return new WaitForSeconds(random);

                // Debug.Log("Getting new scent node");
                // Current Scent Node
                _currentScentNode = GetRandomPositionAroundPlayer();

                // Create a sphere at the new scent node position
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.GetComponent<Collider>().enabled = false; // Disable collider
                sphere.transform.position = _currentScentNode;
                sphere.transform.localScale = Vector3.one * 3f; // Adjust size as needed
                sphere.GetComponent<Renderer>().material.color = Color.blue; // Set color to red for visibility
                if (!EnemyManager.Instance.ShowEnemyDebugRays)
                {
                    // hide the sphere if debug rays are not on
                    Destroy(sphere);
                    continue;
                }
                Destroy(sphere, random);
            }
        }
    }



    public Vector3 GetRandomPositionAroundPlayer()
    {
        // Pick a random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Convert angle to a direction vector on the XZ plane
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        // Offset by distance
        return GlobalDataStore.Instance.Player.transform.position + direction * Random.Range(_minPlayerScentNodeDistance, _maxPlayerScentNodeDistance);
    }










    // Movement ------------------------------------------------------------------------------------------

    // use new variable to control physics movement force
    // 
    public float wanderTooCloseDistance = 5f;
    public float seekTooCloseDistance = 3f;
    public void UsePhysicsToMove()
    {
        if (_currentState == EnemyState.Dead || IsAttacking) return;
        // Player Scent Node
        float currentSpeed = (_currentState == EnemyState.Seek) ? _speed * _SeekSpeedMultiplier : _speed;
        Vector3 dir = (_currentState == EnemyState.Wander) ? (_currentWanderNode - transform.position).normalized : (_currentScentNode - transform.position).normalized;


        // only apply the dir force if the enemy is not too close to either node
        Vector3 whichNode = (_currentState == EnemyState.Wander) ? _currentWanderNode : _currentScentNode;
        float tooCloseDistance = (_currentState == EnemyState.Wander) ? wanderTooCloseDistance : seekTooCloseDistance;

        // movement to node force
        // if (Vector3.Distance(transform.position, whichNode) < tooCloseDistance)
        _rb.AddForce(dir * currentSpeed, ForceMode.Force);

        // avoidance force from raycasting
        _rb.AddForce(avoidanceForce, ForceMode.Force);


        // Limit max speed
        if (_rb.linearVelocity.magnitude > maxVelocity)
            _rb.linearVelocity = maxVelocity * _rb.linearVelocity.normalized;
    }




    public Vector3 GetCurrentTargetPosition()
    {
        if (_currentState == EnemyState.Wander)
        {
            return _currentWanderNode;
        }
        else
        {
            return _currentScentNode;
        }
    }










    // Death & Rewards -----------------------------------------------------------------------------------

    public void InitiateDeath()
    {
        Debug.Log($"{gameObject.name} has died.");
        _currentState = EnemyState.Dead;
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













    // Animation Events -----------------------------------------------------------------------------------
    private Tween floatTween;

    public void StartFloatingAnimation()
    {
        float finalSpeed = _currentState == EnemyState.Wander ? Random.Range(1f, 2f) : Random.Range(.3f, .5f);
        floatTween = transform.DOMoveY(transform.position.y + Random.Range(0.5f, 1), finalSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }


    public void PlaySeekAnimation()
    {
        // When the enemy goes from wander to seek, make it do a quick hop up and down
        transform.DOPunchPosition(Vector3.up * 1.2f, Random.Range(.6f, 1f), 10, 1);

        // make their material 
    }

    public IEnumerator PlayAttackAnimation()
    {
        // Make the enemy slowly back up a little then lunge forward quickly, then move back to original position
        Sequence attackSeq = DOTween.Sequence();
        float backUpDistance = Random.Range(.01f, .015f);
        attackSeq.Append(transform.DOPunchPosition(-Vector3.forward * backUpDistance, Random.Range(1.5f, 1.5f), 10, 1));
        attackSeq.Play();
        yield return attackSeq.WaitForCompletion();
    }
}
