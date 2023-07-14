using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private float knockbackAmount = 15f;
    [SerializeField] private GameObject deathVFXPrefab;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake(){
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start(){
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        GetComponent<PickupSpawner>().DropItems();
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockbackAmount);
        
        StartCoroutine(CheckDetectDeathRoutine());
        StartCoroutine(flash.FlashRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine(){
        yield return new WaitForSeconds(flash.GetRestoreFlashTime());
        DetectDeath();
    }

    private void DetectDeath(){
        if (currentHealth <= 0){
            EnemyManager.Instance.UpdateTotalEnemies();
            GetComponent<PickupSpawner>().DropItems();
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);


            Destroy(gameObject);
            Debug.Log($"{gameObject.name} IS DEAD!");
        }
    }
}
