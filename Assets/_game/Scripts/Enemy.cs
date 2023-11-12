using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform _targetPos;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _damageCooldown;
    [SerializeField] private int _knockbackStrength;
    private float _currentCooldown;
    private Rigidbody2D _playerRb;
    private Health _playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            if (_currentCooldown <= 0)
            {
                //do damage
                _playerHealth = collision.GetComponent<Health>();
                _playerHealth.TakeDamage(_damageAmount);
                _currentCooldown = _damageCooldown;
                //player knockback
                _playerRb = collision.GetComponent<Rigidbody2D>();
                _playerRb.AddForce((collision.transform.position - transform.position).normalized * _knockbackStrength, ForceMode2D.Force);
            }
        }
    }

    private void Start()
    {
        //find player
        _targetPos = FindAnyObjectByType<Player>().GetComponent<Transform>();
    }
    private void Update()
    {
        if(_currentCooldown >= 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
        //move toward player
        transform.position = Vector2.MoveTowards(transform.position, _targetPos.transform.position, _moveSpeed * Time.deltaTime);
    }
}
