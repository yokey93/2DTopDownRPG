using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaserPrefab;
    [SerializeField] private Transform magicLaserSpawnPoint;
    
    private Animator myAnimator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake(){
        myAnimator = GetComponent<Animator>();
    }

    private void Update(){
        //MouseFollowWithOffset();
    }
    
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }

    public void Attack(){
        myAnimator.SetTrigger(FIRE_HASH);
    }

    // CALLED IN ANIMATION EVENT IN ATTACK ANIMATION
    public void SpawnStaffProjectileAnimEvent(){
        GameObject newLaser = Instantiate(magicLaserPrefab, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    private void MouseFollowWithOffset(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; // radians to degrees conversion 

        if (mousePos.x < playerScreenPoint.x){
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, -180f, angle);
        } else{
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
