using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _damageCooldown;
    [SerializeField] private int _knockbackStrength;
    [SerializeField] private int _enemyLevel;
    private float _currentCooldown;
    private Rigidbody2D _playerRb;
    private Health _playerHealth;
    private Rigidbody2D _enemyRB;
    private Animator _playerAnimator;
    private Player _player;

    private void Start()
    {
        _enemyRB = gameObject.GetComponent<Rigidbody2D>();

        //get player components
        _playerHealth = FindAnyObjectByType<Player>().GetComponent<Health>();
        _player = FindAnyObjectByType<Player>().GetComponent<Player>();
        _playerRb = _player._rb;
        _playerAnimator = _player._catAnimator;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //_playerRb.velocity = Vector2.zero; //stops walking animation
        if (collision.gameObject.tag == "Player" && _currentCooldown <= 0 && !_player._currentlyAttacking)
        {
            //do damage
            //_playerHealth.TakeDamage(_damageAmount); //uncomment after done debugging
            _playerAnimator.SetTrigger("isDamaged"); //play animation
            _currentCooldown = _damageCooldown;
            
            //player knockback
            StartCoroutine(Knockback(_playerRb));
        }
    }

    private void Update()
    {
        if(_currentCooldown >= 0)
        {
            _currentCooldown -= Time.deltaTime;
        }

        //checks if player is still alive
        if(_playerRb != null)
        {
            //move toward player
            _enemyRB.position = Vector2.MoveTowards(_enemyRB.position, _playerRb.position, _moveSpeed * Time.deltaTime);
        }
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
