using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{

    [SerializeField] float enemyMoveSpeed = 2f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Knockback knockback;

    private SpriteRenderer spriteRender;

    private void Awake(){
        spriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate(){
        if (knockback.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDirection * (enemyMoveSpeed + Time.deltaTime));

        if (moveDirection.x < 0){
            spriteRender.flipX = true;
        } else if (moveDirection.x > 0){
            spriteRender.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition){
        moveDirection = targetPosition;
    }

    public void StopMoving(){
        moveDirection = Vector3.zero;
    }

}
