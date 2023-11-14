using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorHelper : MonoBehaviour
{
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindAnyObjectByType<Player>().GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopAttacking()
    {
        _player._currentlyAttacking = false;
        _player.ResetAttackChain();
    }
    public void StartAttacking()
    {
        _player._currentlyAttacking = true;
    }

    public void ResetCooldown()
    {
        _player._currentDashCooldown = _player._dashCooldown; //reset cooldown
    }

    public void ResetAttackChain()
    {
        _player.ResetAttackChain();
    }
}
