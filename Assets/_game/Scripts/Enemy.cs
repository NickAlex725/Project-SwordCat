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
    [SerializeField] private int _enemyLevel;
    private float _currentCooldown;
    private Rigidbody2D _playerRb;
    private Health _playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerRb = collision.GetComponent<Rigidbody2D>();
        _playerRb.velocity = Vector2.zero;
        if (collision.GetComponent<Player>())
        {
            if (_currentCooldown <= 0)
            {
                //do damage
                _playerHealth = collision.GetComponent<Health>();
                _playerHealth.TakeDamage(_damageAmount);
                _currentCooldown = _damageCooldown;
                //player knockback
                StartCoroutine(Knockback(_playerRb));
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
    private IEnumerator Knockback(Rigidbody2D _rb)
    {
        for (int i = 0; i < 10; i++)
        {
            _playerRb.AddForce((_rb.transform.position - transform.position).normalized * _knockbackStrength, ForceMode2D.Force);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public int GetEnemyLevel()
    {
        return _enemyLevel;
    }
}
