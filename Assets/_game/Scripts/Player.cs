using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    
    [SerializeField] bool _attackChain2 = false;
    [SerializeField] bool _attackChain3 = false;
    private Vector2 _mouseDirection;


    [Header("Animator")]
    public Animator _catAnimator;

    [Header("Input Actions")]
    public InputAction _playerMovement;
    public InputAction _playerDash;

    public float _currentDashCooldown;
    public Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private bool _facingLeft = true;
    public bool _currentlyAttacking = false;
    public bool _invincible;

    [Header("Audio")]
    [SerializeField] AudioSource _sourceSwing;
    [SerializeField] AudioSource _sourceSwing2;
    [SerializeField] AudioSource _sourceSwing3;
    public AudioSource _sourceDamaged;

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
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy" && _currentlyAttacking)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            Health enemyHealth = col.gameObject.GetComponent<Health>();

            switch(enemy.GetEnemyLevel())
            {
                case 1:
                    enemy._animator.Play("Die");
                    _currentDashCooldown = 0; //replenish cooldown
                    _attackChain2 = true;
                    _invincible = true;
                break;

                case 2:
                    if(_attackChain2)
                    {
                        enemy._animator.Play("Die");
                        _currentDashCooldown = 0; //replenish cooldown
                        _attackChain3 = true;
                        _invincible = true;
                    }
                break;

                case 3:
                    if(_attackChain2 & _attackChain3)
                    {
                        enemy._animator.Play("Die");
                        _currentDashCooldown = 0; //replenish cooldown
                        ResetAttackChain();
                        _invincible = true;
                    }
                break;
            }
           _currentlyAttacking = false;
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
            #region Function to calculate mouse position and direction | For dash purposes
            Vector3 mouseRotation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float fRotation = MathF.Atan2(mouseRotation.y, mouseRotation.x);
            float fX = Mathf.Cos(fRotation);
            float fY = Mathf.Sin(fRotation);
            _mouseDirection = new Vector2(fX, fY);
            _mouseDirection.Normalize();
            #endregion

            //start cd
            _currentDashCooldown = _dashCooldown;
            //play animation
            _catAnimator.SetTrigger("Dash");
            //play audio
            if(_attackChain2 && !_attackChain3)
            {
                _sourceSwing2.Play();
            }
            else if(_attackChain2 && _attackChain3)
            {
                _sourceSwing3.Play();
            }else
            {
                _sourceSwing.Play();
            }
            //dash coroutine
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        //make sure player is facing the right way
        if(_mouseDirection.x > 0 && _facingLeft)
        {
            Flip();
        }
        else if(_mouseDirection.x < 0 && !_facingLeft)
        {
            Flip();
        }
        
        for (int i = 0; i < 25; i++)
        {
            //dashing
            _rb.AddForce(_mouseDirection * _dashForce, ForceMode2D.Force);
            yield return new WaitForSeconds(0.01f);
        }
    }
}