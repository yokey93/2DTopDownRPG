using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    //public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform weaponCollider;

    private bool facingLeft = false;
    private PlayerControls playerControls;
    private Knockback knockback;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private float startingMoveSpeed;
    private bool isDashing = false;

    //[SerializeField] private float roadSpeed = 20f;
    //private bool isRoads = false;

    protected override void Awake() {
        //Instance = this; 
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start(){
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate(){
        FlipPlayer();
        Move();
    }

    public Transform GetWeaponCollider(){
        return weaponCollider;
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move(){
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void FlipPlayer(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            spriteRenderer.flipX = true;
            facingLeft = true;
        } else {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash(){
        if (!isDashing && Stamina.Instance.CurrentStamina > 0){
            Stamina.Instance.UseStamina();
            isDashing = true;
            trailRenderer.emitting = true;
            moveSpeed *= dashSpeed;
            StartCoroutine(EndDash());
        }
    }

    private IEnumerator EndDash(){
        float dashEndTime = .2f;
        float dashCD = .5f;
        yield return new WaitForSeconds(dashEndTime);
        moveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    /*private void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Roads" && isRoads){
            moveSpeed = roadSpeed;
        }
    }*/
}
