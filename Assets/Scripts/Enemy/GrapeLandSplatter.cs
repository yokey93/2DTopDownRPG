using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;

    private void Awake(){
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start(){
        StartCoroutine(spriteFade.SlowFadeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D col){
        PlayerHealth player = col.gameObject.GetComponent<PlayerHealth>();
        player?.TakeDamage(1, transform);

        Invoke("DisableCollider", 0.2f);
    }

    private void DisableCollider(){
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
