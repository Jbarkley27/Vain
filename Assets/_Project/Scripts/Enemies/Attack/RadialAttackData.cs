using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Radial Attack", menuName = "Attacks/Radial Attack")]
public class RadialAttackData : AttackDataBase
{
    public int NumberOfProjectiles = 8;
    public AnimationCurve speedCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [Range(0f, 360f)]
    public float spreadAngle = 360f; // Total spread, centered on forward
    public float projectileForce = 50;

    private void Update()
    {

    }

    public override IEnumerator Execute(EnemyBase enemy)
    {
        if (enemy == null) yield break;


        // Special case: 1 projectile goes forward
        if (NumberOfProjectiles == 1)
        {
            FireProjectile(enemy.transform.forward, enemy);
            yield break;
        }

        // Disable enemy attack while firing
        enemy.SetCanMove(false);


        float angleStep;
        float startAngle;

        if (Mathf.Approximately(spreadAngle, 360f))
        {
            // Avoid duplicate at 0/360 by dividing evenly around the full circle
            angleStep = 360f / NumberOfProjectiles;
            startAngle = 0f; // Start at forward direction
        }
        else
        {
            angleStep = spreadAngle / (NumberOfProjectiles - 1);
            startAngle = -spreadAngle / 2f; // Center the cone
        }


        for (int i = 0; i < NumberOfProjectiles; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Quaternion angleRotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 localDirection = angleRotation * Vector3.forward;
            Vector3 worldDirection = enemy.gameObject.transform.rotation * localDirection;

            FireProjectile(worldDirection, enemy);

            yield return new WaitForSeconds(RateOfFire); // Optional delay
        }


        // for (int i = 0; i < NumberOfProjectiles; i++)
        // {
        //     float currentAngle = startAngle + (angleStep * i);
        //     Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
        //     Vector3 localDirection = rotation * Vector3.forward;

        //     // Rotate the local direction by the GameObject's orientation
        //     Vector3 worldDirection = enemy.gameObject.transform.rotation * localDirection;

        //     // Instantiate and launch projectile
        //     ProjectileBase mover = Instantiate(ProjectilePrefab, enemy.gameObject.transform.position, Quaternion.identity).GetComponent<ProjectileBase>();


        //     if (mover != null)
        //     {
        //         mover.Initialize(worldDirection, BaseSpeed, speedCurve, Lifetime);
        //     }

        //     // angle += angleStep;
        //     // yield return new WaitForSeconds(fireDelay);


        //     // GameObject proj = Instantiate(ProjectilePrefab, enemy.gameObject.transform.position, Quaternion.LookRotation(direction));
        //     // ProjectileBase mover = proj.GetComponent<ProjectileBase>();

        //     // if (mover != null)
        //     // {
        //     //     mover.Initialize(direction, angle, speedCurve, Lifetime);
        //     // }

        //     yield return new WaitForSeconds(RateOfFire);
        // }

        enemy.CanAttack = true;
        enemy.IsAttacking = false;
        enemy.SetCanMove(true);

    }

    void FireProjectile(Vector3 direction, EnemyBase enemy)
    {
        ProjectileBase mover = Instantiate(ProjectilePrefab, enemy.gameObject.transform.position, Quaternion.identity).GetComponent<ProjectileBase>();

        if (mover != null)
        {
            mover.Initialize(direction, BaseSpeed, speedCurve, Lifetime, Damage);
        }
    }
}