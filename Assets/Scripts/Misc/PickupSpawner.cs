using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab, heartPrefab, staminaPrefab;

    // Call in DestructibleObjects.cs 
    public void DropItems(){
        int randomNum = Random.Range(1, 4);
        
        if(randomNum == 1){
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

        if (randomNum == 2){
            Instantiate(staminaPrefab, transform.position, Quaternion.identity);
        }

        if (randomNum == 3){
            int randomCoinAmt = Random.Range(1, 4);

            for (int i = 0; i < randomCoinAmt; i++){
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
