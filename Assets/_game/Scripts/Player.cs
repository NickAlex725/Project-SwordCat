using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image _dashCD;

    [Header("Player Attributes")]
    [SerializeField] float _moveSpeed;
    [SerializeField] int _dashForce;
    [SerializeField] public float _dashCooldown;

    [Header("Combat Attributes")]
    [SerializeField] Transform _attackPoint;
    [SerializeField] float _attackRange;
    [SerializeField] bool _attackChain2 = false;
    [SerializeField] bool _attackChain3 = false;
    [SerializeField] Collider2D _attackHitbox;


    [Header("Animator")]
    public Animator _catAnimator;

    [Header("Input Actions")]
    [SerializeField] InputAction _playerMovement;
    [SerializeField] InputAction _playerDash;

    public float _currentDashCooldown;
    public Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private bool _facingLeft = true;
    public bool _currentlyAttacking = false;
    private void OnEnable()
    {
        //needed for input to work
        _playerMovement.Enable();
        _playerDash.Enable();
    }

    private void OnDisable()
    {
        //needed for input to work
       _playerMovement.Disable();
       _playerDash.Disable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //make sure player is facing the right way
        if(_moveDirection.x > 0 && _facingLeft)
        {
            Flip();
        }
        else if(_moveDirection.x < 0 && !_facingLeft)
        {
            Flip();
        }

        //counting down cd if needed
        if (_currentDashCooldown >= 0)
        {
            _currentDashCooldown -= Time.deltaTime;
            _dashCD.fillAmount = _dashCooldown - (_currentDashCooldown / _dashCooldown);
        }

        //WASD movement
        _moveDirection = _playerMovement.ReadValue<Vector2>();
        _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);

        //play walking animation
        _catAnimator.SetFloat("speed", _rb.velocity.magnitude);

        //Dash attack
        _playerDash.performed += PlayerDash;
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingLeft = !_facingLeft;
    }

    //collision check particularly when dash attacking
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy" && _currentlyAttacking)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();

            switch(enemy.GetEnemyLevel())
            {
                case 1:
                    Destroy(enemy.gameObject);
                    _attackChain2 = true;
                break;

                case 2:
                    if(_attackChain2)
                    {
                        Destroy(enemy.gameObject);
                        _attackChain3 = true;
                    }
                break;

                case 3:
                    if(_attackChain2 & _attackChain3)
                    {
                        Destroy(enemy.gameObject);
                        ResetAttackChain();
                    }
                break;

                default:
                    return;
            }
            _currentDashCooldown = 0; //replenish cooldown
        }
    }

    public void ResetAttackChain()
    {
        _attackChain2 = false;
        _attackChain3 = false;
    }

    private void PlayerDash(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton() && _currentDashCooldown <= 0)
        {
            //start cd
            _currentDashCooldown = _dashCooldown;
            //play animation
            _catAnimator.Play("SwordCat_Dash", 0, 0);
            //dash coroutine
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        for (int i = 0; i < 25; i++)
        {
            //dashing
            _rb.AddForce(_moveDirection * _dashForce, ForceMode2D.Force);
            yield return new WaitForSeconds(0.01f);
        }
    }
}