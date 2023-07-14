using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamDirectionChange = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    
    private enum State {
        Roaming,
        Attacking
    }

    private Vector2 roamPos;
    private float timeRoaming = 0f;
    private bool canAttack = true;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake(){
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming; // State.Roaming = 0
    }

    private void Start(){
        roamPos = GetRoamingPosition();
    }

    private void Update(){
        MovementStateControl();
    }

    private void MovementStateControl(){
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    private void Roaming(){
        timeRoaming += Time.deltaTime;
        enemyPathfinding.MoveTo(roamPos);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange){
            state = State.Attacking;  // State.Attacking = 1
        }

        if (timeRoaming > roamDirectionChange){
            roamPos = GetRoamingPosition();
        }
    }

    private void Attacking(){
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange){
            state = State.Roaming;  // State.Roaming = 0
        }

        if (canAttack && attackRange != 0){
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking){
                enemyPathfinding.StopMoving();
            } else {
                enemyPathfinding.MoveTo(roamPos);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine(){
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition(){
        timeRoaming = 0f;
        return new Vector2 (Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // normalized makes it not go diaganol
    }
    
    /*private IEnumerator RoamingRoutine(){
        // COROUTINE THAT PRINTS RANDOM VECTOR2(X,Y) COORDS EVERY 2 SECONDS
        // CALL IN START()
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(timeRoaming); 
        }
    }*/
}
