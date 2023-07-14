using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPos;

    private void Start(){
        startPos = transform.position;
    }

    private void Update(){
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange){
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col){
        EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = col.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = col.gameObject.GetComponent<PlayerHealth>();

        if (!col.isTrigger && (enemyHealth || indestructible || player)){
            if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile)){
                player?.TakeDamage(1, transform);  // player takes damage
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            } else if (!col.isTrigger && indestructible) { 
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
    
    private void MoveProjectile(){
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void DetectFireDistance(){
        if (Vector3.Distance(transform.position, startPos) > projectileRange){
            Destroy(gameObject);
        }
    }
}
