using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorHelper : MonoBehaviour
{
    private Player _player;
    public Collider2D _attackCollider;
    private Health _playerHealth;
    public Material[] _materials;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindAnyObjectByType<Player>().GetComponent<Player>();
        _playerHealth = FindAnyObjectByType<Player>().GetComponent<Health>();
    }

    public void StopAttacking()
    {
        _attackCollider.enabled = false;
        _player._currentlyAttacking = false;
        _player.ResetAttackChain();
    }
    public void StartAttacking()
    {
        _attackCollider.enabled = true;
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

    #region Health Indicator using color flickers
    public void ColorOff()
    {
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = Color.white;
        }
    }
    float green;
    public void ColorOn()
    {
        for(int i = 0; i < _materials.Length; i++)
        {
            if(_playerHealth._currentHealth >= 70)
            {
                green  = 0.8f;
            }
            else if(_playerHealth._currentHealth >= 40 && _playerHealth._currentHealth < 70)
            {
                green  = 0.6f;
            }
            else if(_playerHealth._currentHealth < 40)
            {
                green  = 0.4f;
            }
            else
            {
                return;
            }

            _materials[i].color = new Color(1f, green, 0.4f);
        }
    }

    public void CheckDie()
    {
        if(_playerHealth._currentHealth <= 0)
        {
            _playerHealth.Die();
        }
    }

    public void CheckHealthAndDisable() //Checks player health and disables player script before CheckDie deletes the gameobject
    {
        if(_playerHealth._currentHealth <= 0)
        {
            _player._playerMovement.Disable();
            _player._playerDash.Disable();
        }
    }
    #endregion
}
