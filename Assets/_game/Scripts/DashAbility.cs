using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float _dashSpeed;

    private bool _canMove;
    private Vector2 _currentTouchPos;
    Collider2D _col;

    void Start()
    {
        _col = GetComponent<Collider2D>();
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
                    while(transform.position.x != _currentTouchPos.x && transform.position.y != _currentTouchPos.y)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, _currentTouchPos, _dashSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }
}
