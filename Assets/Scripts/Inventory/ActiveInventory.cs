using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    protected override void Awake(){
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start(){
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int) ctx.ReadValue<float>());
        EquipStartingWeapon();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    public void EquipStartingWeapon(){
        ToggleActiveHighlight(0); // start with sword each spawn
    }

    private void ToggleActiveSlot(int numValue){
        // will return 0, 1, 2, 3, 4, or 5 based on keyboard input
        ToggleActiveHighlight(numValue);
    }

    private void ToggleActiveHighlight(int indexNum){
        activeSlotIndexNum = indexNum;
        foreach (Transform inventorySlot in this.transform){
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon(){
        if (PlayerHealth.Instance.IsDead) { return; }

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null){
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();


        if (weaponInfo == null){
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        //GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).
        //GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }

}
