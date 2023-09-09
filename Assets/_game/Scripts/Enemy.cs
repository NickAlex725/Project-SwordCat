using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform _targetPos;
    [SerializeField] private int _moveSpeed;

    private void Start()
    {
        _targetPos = FindAnyObjectByType<DashAbility>().GetComponent<Transform>();
    }
    private void Update()
    {
        transform.LookAt(_targetPos.position);
        transform.position = Vector2.MoveTowards(transform.position, _targetPos.transform.position, _moveSpeed * Time.deltaTime);
    }
}
