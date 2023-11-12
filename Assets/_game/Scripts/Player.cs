using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Image _dashCD;
    [SerializeField] float _moveSpeed;
    [SerializeField] int _dashForce;
    [SerializeField] float _dashCooldown;
    [SerializeField] InputAction _playerMovement;
    [SerializeField] InputAction _playerDash;
    private float _currentDashCooldown;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    

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
        if(_currentDashCooldown > 0)
        {
            _currentDashCooldown -= Time.deltaTime;
            _dashCD.fillAmount = 1 - (_currentDashCooldown / _dashCooldown);
        }
        //WASD movement
        _moveDirection = _playerMovement.ReadValue<Vector2>();
        _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);

        //Dash attack
        _playerDash.performed += PlayerDash;
    }

    private void PlayerDash(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            if (_currentDashCooldown <= 0)
            {
                _currentDashCooldown = _dashCooldown;
                StartCoroutine(Dash());
            }
        }
    }
    private IEnumerator Dash()
    {
        for (int i = 0; i < 25; i++)
        {
            _rb.AddForce(_moveDirection * _dashForce, ForceMode2D.Force);
            yield return new WaitForSeconds(0.01f);
        }
    }
}