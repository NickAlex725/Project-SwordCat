using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] InputAction _playerMovement;
    [SerializeField] InputAction _playerDash;
    [SerializeField] float _moveSpeed;
    [SerializeField] int _dashForce;
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
            //_rb.AddForce(_moveDirection * _dashForce, ForceMode2D.Force);
            //_rb.position = _moveDirection * _dashForce;
            //_rb.MovePosition(_moveDirection * _dashForce);
            StartCoroutine(Dash());
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