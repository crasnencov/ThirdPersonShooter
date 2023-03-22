using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Text ammoLeftTxt;
    [SerializeField] private Text magSizeTxt;
    

    public void UpdateInfo(int ammoLeft, int magSize)
    {
        ammoLeftTxt.text = ammoLeft.ToString();
        magSizeTxt.text = magSize.ToString();
        
    }
}
