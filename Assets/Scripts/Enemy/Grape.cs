using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{

    [SerializeField] private GameObject grapeProjectilePrefab;

    private Animator myAni;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Fire");

    private void Awake(){
        myAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Attack(){
        myAni.SetTrigger(ATTACK_HASH);

        if (transform.position.x - PlayerController.Instance.transform.position.x < 0){
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnProjectileAnimEvent(){
        Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }
}
