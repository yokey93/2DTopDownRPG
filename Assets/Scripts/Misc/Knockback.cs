using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public bool GettingKnockedBack { get; private set; }
    [SerializeField] float knockBackTime = 0.2f;
    private Rigidbody2D rb;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust){
        GettingKnockedBack = true;
        Vector2 force = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine(){
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero; // (0,0);
        GettingKnockedBack = false;
    }
}
