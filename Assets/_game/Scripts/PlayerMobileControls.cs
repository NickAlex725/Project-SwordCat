using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMobileControls : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashForce;
    private PlayerInput _inputs;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
        _inputs = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.actions["Move"].triggered)
        {
            Debug.Log("move action");
        }

        if (_inputs.actions["Dash"].triggered)
        {
            Debug.Log("dash action");
        }
    }

    private void OnMove(InputValue value)
    {
        _rb.velocity = value.Get<Vector2>() * _moveSpeed;
    }

    private void OnDash(InputValue value)
    {
        _rb.AddForce(value.Get<Vector2>() * _dashForce, ForceMode2D.Force);
    }
}
