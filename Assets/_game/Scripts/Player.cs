using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float _dashCooldown;

    [Header("Combat Attributes")]
    [SerializeField] Transform _attackPoint;
    [SerializeField] float _attackRange;

    [Header("Animator")]
    [SerializeField] Animator _catAnimator;

    [Header("Input Actions")]
    [SerializeField] InputAction _playerMovement;
    [SerializeField] InputAction _playerDash;

    private float _currentDashCooldown;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private int _currentDamage = 1;
    

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
        if (_currentDashCooldown > 0)
        {
            _currentDashCooldown -= Time.deltaTime;
            _dashCD.fillAmount = 1 - (_currentDashCooldown / _dashCooldown);
        }

        //WASD movement
        _moveDirection = _playerMovement.ReadValue<Vector2>();
        _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);

        //play walking animation
        if(_rb.velocity != Vector2.zero)
        {
            _catAnimator.SetFloat("speed", 1);
        }
        else
        {
            _catAnimator.SetFloat("speed", 0);
        }

        //Dash attack
        _playerDash.performed += PlayerDash;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemy>())
            {
                var enemyHealth = enemy.GetComponent<Health>();
                enemyHealth.TakeDamage(_currentDamage);
            }
        }
    }

    private void PlayerDash(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            if (_currentDashCooldown <= 0)
            {
                //start cd
                _currentDashCooldown = _dashCooldown;
                //play animation
                _catAnimator.SetTrigger("Dash");
                //dash coroutine
                StartCoroutine(Dash());
            }
        }
    }
    private IEnumerator Dash()
    {
        for (int i = 0; i < 25; i++)
        {
            //attack while dashing
            Attack();
            //dashing
            _rb.AddForce(_moveDirection * _dashForce, ForceMode2D.Force);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnDrawGizmos()
    {
        //visual for attack circle
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}