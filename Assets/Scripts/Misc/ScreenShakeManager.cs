using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    private CinemachineImpulseSource impulseSource;

    protected override void Awake(){
        base.Awake();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // call in PlayerHealth.cs TakeDamage() method
    public void ShakeScreen(){
        impulseSource.GenerateImpulse();
    }

}
