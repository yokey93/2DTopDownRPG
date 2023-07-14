using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    //[SerializeField] private float swordAttackCD = 5f;
    [SerializeField] private WeaponInfo weaponInfo;
    
    private Transform weaponCollider;
    private Animator myAnimator;
    private GameObject slashAnim;

    private void Awake(){
        myAnimator = GetComponent<Animator>();
    }

    private void Start(){
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashAnimSpawnPoint").transform;
    }

    private void Update(){
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }

    public void Attack(){
        myAnimator.SetTrigger("Attack"); //fire sword animation
        weaponCollider.gameObject.SetActive(true); //turn weapon collider ON
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity); 
        slashAnim.transform.parent = this.transform.parent;
    }

    private void MouseFollowWithOffset(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; // radians to degrees conversion 

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, -180f, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        } else{
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            weaponCollider.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void DoneAttackingAnimEvent(){
        weaponCollider.gameObject.SetActive(false);
    }
    
    // FLIP X AXIS USING QUATERNION EULER && ** ADD TO ANIMATION EVENT FIRST FRAME **
    public void SwingUpFlipAnimEvent(){
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180f, 0f, 0f);

        if (PlayerController.Instance.FacingLeft){
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent(){
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (PlayerController.Instance.FacingLeft){
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
