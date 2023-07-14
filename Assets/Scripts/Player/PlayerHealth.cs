using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }

    [SerializeField] private float knockbackAmount = 10f;
    [SerializeField] private int maxHealth = 15;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    //const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string LEVEL_ONE = "Scene_1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake(){
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start(){
        IsDead = false;
        currentHealth = maxHealth;
        UpdateHealth();
    }

    private void OnCollisionStay2D(Collision2D col){

        EnemyAI enemy = col.gameObject.GetComponent<EnemyAI>();

        if (enemy){
            // take damage
            TakeDamage(1, col.transform);
        }
    }

    private void CheckPlayerDeath(){
        if (currentHealth <= 0 && !IsDead){
            Debug.Log("PLAYER IS DEAD!");
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject); // so we cant stil attack on death
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathRoutine());
        }
    }

    private void UpdateHealth(){
        if (healthSlider == null){
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void HealPlayer(){
        if(currentHealth < maxHealth){
            currentHealth += 1;
            Debug.Log($"HEALED** Our players current health is: {currentHealth}");
            UpdateHealth();
        }
    }

    // CALLED IN PROJECTILE.CS ONTRIGGER
    public void TakeDamage(int damageAmount, Transform hitTransform){
        if (!canTakeDamage) { return; }
        
        Debug.Log($"DAMAGED** Our players current health is: {currentHealth}");
        ScreenShakeManager.Instance.ShakeScreen(); // start screen shake
        knockback.GetKnockedBack(hitTransform, knockbackAmount);  // start knockback
        StartCoroutine(flash.FlashRoutine());  // start flash
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());

        UpdateHealth();
        CheckPlayerDeath();
    }

    private IEnumerator DamageRecoveryRoutine(){
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private IEnumerator DeathRoutine(){
        yield return new WaitForSeconds(2f);
        EnemyManager.Instance.ResetEnemiesKilled();
        Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(LEVEL_ONE);
    }
}
