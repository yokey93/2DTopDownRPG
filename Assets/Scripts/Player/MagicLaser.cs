using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f; // speed of lasers growth
    
    
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isGrowing = true;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    
    private void Start(){
        LaserFaceMouse();
        Debug.Log($"{isGrowing} is the bool result for isGrowing variable");
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.GetComponent<Indestructible>()){
            isGrowing = false;
        }
    }

    // CALL IN STAFF.CS SPAWNSTAFFPROJECTILEANIMEVENT()
    public void UpdateLaserRange(float laserRange){
        this.laserRange = laserRange;
        StartCoroutine(IncrLaserLengthRoutine());
    }

    // COROUTINE DELAY FOR LASER growth
    private IEnumerator IncrLaserLengthRoutine(){
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / laserGrowTime;

            //change sprite size
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), 1f);
            //change collider size and offset simultaneously
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), capsuleCollider2D.size.y);
            // divide lerp by 2 for the offset
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearTime)) /2, capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse(){
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }
}
