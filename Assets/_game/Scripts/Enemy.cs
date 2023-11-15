using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damageCooldown;
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private int _knockbackStrength;
    [SerializeField] private int _enemyLevel;
    private float _currentCooldown;
    private Rigidbody2D _playerRb;
    private Health _playerHealth;
    private Rigidbody2D _enemyRB;
    private Collider2D _enemyCollider;
    private Animator _playerAnimator;
    private Player _player;

    public Animator _animator;
    public Material[] _materials;
    private Color _initColor;

    [Header("Audio")]
    [SerializeField] AudioSource _sourceHiss;
    [SerializeField] AudioSource _sourceBite;

    private void Start()
    {
        _initColor = _materials[0].color;
        _enemyRB = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _enemyCollider = gameObject.GetComponent<Collider2D>();

        //get player components
        _playerHealth = FindAnyObjectByType<Player>().GetComponent<Health>();
        _player = FindAnyObjectByType<Player>().GetComponent<Player>();
        _playerRb = _player._rb;
        _playerAnimator = _player._catAnimator;

        //start hissing occasionally
        StartCoroutine(OccasionalHiss());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //_playerRb.velocity = Vector2.zero; //stops walking animation
        if (collision.gameObject.tag == "Player" && _currentCooldown <= 0 && !_player._currentlyAttacking)
        {
            //do damage
            _playerHealth.TakeDamage(_damageAmount); //uncomment after done debugging
            _playerAnimator.SetTrigger("isDamaged"); //play animation
            _currentCooldown = _damageCooldown;
            _player._sourceDamaged.Play();
            
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

    private IEnumerator OccasionalHiss()
    {
        yield return new WaitForSeconds(Random.Range(5, 16));
        _sourceHiss.Play();
    }

    public int GetEnemyLevel()
    {
        return _enemyLevel;
    }

    #region Health Indicator using color flickers
    public void ColorOff()
    {
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = _initColor;
        }
    }

    public void ColorOn()
    {
        for(int i = 0; i < _materials.Length; i++)
        {
            float green = 0.4f;

            _materials[i].color = new Color(1f, green, 0.4f);
        }
    }

    public void CheckDie() //need a death animation to execute this
    {
            Destroy(gameObject);
    }

    public void Disable() //Checks player health and disables enemy script before CheckDie deletes the gameobject
    {
        this.enabled = false;
        _enemyCollider.enabled = false;
    }
    #endregion
}
