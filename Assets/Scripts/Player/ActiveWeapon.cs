using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private bool attackButtonDown, isAttacking = false;

    private float timeBetweenAttacks = 2f;

    protected override void Awake(){
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void Start(){
        playerControls.Combat.Attack.started += _ => StartAttacking(); // mouse click down
        playerControls.Combat.Attack.canceled += _ => StopAttacking(); // mouse click up

        AttackCooldown();
    }

    private void Update(){
        Attack();
    }

    public void WeaponNull(){
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown(){
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacks());
    }

    private IEnumerator TimeBetweenAttacks(){
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    public void NewWeapon(MonoBehaviour newWeapon){
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    private void StartAttacking(){
        attackButtonDown = true;
    }

    private void StopAttacking(){
        attackButtonDown = false;
    }

    private void Attack(){
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon){
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
