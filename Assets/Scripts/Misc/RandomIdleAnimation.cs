using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator myAni;

    private void Awake(){
        myAni = GetComponent<Animator>();
    }

    private void Start(){

        if (!myAni) { return ; }
        AnimatorStateInfo state = myAni.GetCurrentAnimatorStateInfo(0);
        myAni.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
