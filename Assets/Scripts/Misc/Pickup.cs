using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        Stamina,
        HealthHeart,
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelerationRate = 0.2f;
    [SerializeField] private float moveSpeed = 3f;

    [Header ("Animation PopUp Settings")]
    [SerializeField] AnimationCurve aniCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;


    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        // CALL COROUTINE FOR COIN POP ANIMATION CURVE
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update(){
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance){
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        } else {
            moveSpeed = 0f;
            moveDir = Vector3.zero; 
        }
    }

    private void FixedUpdate(){
        rb.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.GetComponent<PlayerController>()){
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private void DetectPickupType(){
        switch (pickUpType){
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                Debug.Log("Gold Coin was picked up");
                break;
            case PickUpType.HealthHeart:
                PlayerHealth.Instance.HealPlayer();
                Debug.Log("Heart was picked up");
                break;
            case PickUpType.Stamina:
                // do stamina stuff
                Stamina.Instance.RefreshStamina();
                Debug.Log("Stamina was picked up");
                break;
            default:
                break;
        }
    }

    private IEnumerator AnimCurveSpawnRoutine(){
        float timePassed = 0;
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);
        Vector2 endPoint = new Vector2(randomX, randomY);

        while (timePassed < popDuration){
            timePassed += Time.deltaTime;
            float linearT = timePassed/ popDuration;
            float heightT = aniCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }
}
