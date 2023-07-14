using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake(){
        ps = GetComponent<ParticleSystem>();
    }

    private void Update(){
        if (ps && !ps.IsAlive()){
            DestroySelfAnimEvent();
        }
        // if there is particle system and does it contain live particles?
        // if it not longer is alive (with live particles) destroy itself
    }


    public void DestroySelfAnimEvent(){
        Destroy(gameObject);
    }
}
