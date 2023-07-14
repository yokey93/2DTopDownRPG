using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [Header ("SHOOTER CUSTOM OPTIONS")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed = 5f;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;

    [Tooltip("Distance the bullet shoots from the enemy")]
    [SerializeField] private float startingDistance;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float cooldownTime;

    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enabled for oscillate to function properly.")]
    [SerializeField] private bool oscillate; 

    private bool isShooting = false;

    // UNITY SPECIFIC FUNCTION
    private void OnValidate(){
        if (oscillate) { stagger = true ; }
        if (!oscillate) { stagger = false ; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (cooldownTime < 0.1f) { cooldownTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0.1f) { bulletMoveSpeed = 1f; }
    }

    public void Attack(){
        if (!isShooting){
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }

        // fire bullet burst
        for (int i = 0; i < burstCount; i++)
        {
            if(!oscillate){
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
           
            if (oscillate && i % 2 != 1){   // if dividing i by 2 has a remainder of 1
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            } else if (oscillate){
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1; // reverses oscillation in opposite direction
            }

            for (int j = 0; j < projectilesPerBurst; j++){

                Vector2 pos = FindBulletSpawnPos(currentAngle);  // new Vector2 to take in bullet angle floats
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);  // Instantiate the bullet with no rotation towards the player
                newBullet.transform.right = newBullet.transform.position - transform.position;  // Provides correct direction of bullet

                if (newBullet.TryGetComponent(out Projectile projectile)){
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }
                currentAngle += angleStep; // add to our angle after each bullet spawn
                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }   

        yield return new WaitForSeconds(cooldownTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle){

        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if (angleSpread != 0){
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle){

        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin( currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
