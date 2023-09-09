using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float _dashSpeed;

    private bool _canMove = false;
    private bool _isDashing = false;
    private Vector2 _currentTouchPos;
    Collider2D _col;
    Health _health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit!");
        if (collision.GetComponent<Enemy>() && !_isDashing)
        {
            int damageAmount = collision.GetComponent<Enemy>()._damageAmount;
            _health.TakeDamage(damageAmount);
            Debug.Log(damageAmount);
        }
    }

    void Start()
    {
        _col = GetComponent<Collider2D>();
        _health = GetComponent<Health>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            Vector2 _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);

            if(_touch.phase == TouchPhase.Began)
            {
                Collider2D _touchedCollider = Physics2D.OverlapPoint(_touchPosition);
                if(_col == _touchedCollider)
                {
                    Debug.Log("touched player");
                    _canMove = true;
                }
            }

            if (_touch.phase == TouchPhase.Moved)
            {
                if(_canMove)
                {
                    _currentTouchPos = new Vector2(_touchPosition.x, _touchPosition.y);
                }
            }

            if (_touch.phase == TouchPhase.Ended)
            {
                if(_canMove)
                {
                    Debug.Log("lifted finger");
                    _isDashing = true;
                    StartCoroutine(Dash());
                    _isDashing = false;
                    _canMove = false;
                }
            }
        }
    }

    IEnumerator Dash()
    {
        if (_canMove)
        {
            while(transform.position.x != _currentTouchPos.x && transform.position.y != _currentTouchPos.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, _currentTouchPos, _dashSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
