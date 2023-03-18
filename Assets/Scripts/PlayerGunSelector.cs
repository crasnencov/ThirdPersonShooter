using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType Gun;
    [SerializeField]
    private Transform GunParent;
    [SerializeField]
    private List<GunSO> Guns;
    [SerializeField]
    private PlayerIK InverseKinematics;

    [Space]
    [Header("Runtime Filled")]
    public GunSO ActiveGun;
    
    private void Start()
    {
        Debug.Log("gun selector" );
        GunSO gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        ActiveGun = gun;
        gun.Spawn(GunParent, this);

        // some magic for IK
        // Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        // InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        // InverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
        // InverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        // InverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }
}
