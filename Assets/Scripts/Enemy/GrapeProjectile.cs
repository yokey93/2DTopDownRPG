using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject grapeSplatterPrefab;

    private Vector3 grapeShadowAdjust = new Vector3(0, -0.3f, 0);

    void Start()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;  // setup endPos which is technically our PLAYER
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPos, Vector3 endPos){
        float timePassed = 0f;
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + grapeShadowAdjust, Quaternion.identity);  // Create Variable to Instantiate the grapeShadow

        while (timePassed < duration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(startPos, endPos, linearT) + new Vector2(0f, height); // add linear height to the position of the grapeProjectile
            grapeShadow.transform.position = Vector2.Lerp(startPos, endPos, linearT);
            yield return null;
        }

        Instantiate(grapeSplatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(grapeShadow);
    }
}
