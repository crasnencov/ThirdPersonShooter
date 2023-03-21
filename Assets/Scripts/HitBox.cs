using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public EnemyController enemyController;

    public void OnRaycastHit(Gun Gun, Vector3 direction)
    {
        enemyController.TakeDamage(Gun.gun.damage, direction);
    }
}