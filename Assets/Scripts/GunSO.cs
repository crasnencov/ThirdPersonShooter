
using UnityEngine;
[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    [Header("Info")]
    public new string  name;
    
    [Header("Shooting")]
    public float damage;
    public float maxDistance;//aim distance player.cs
    
    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    // [HideInInspector]
    public bool reloading;

}
