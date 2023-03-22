using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Image foregroundImage, backgroundImage;

    private Transform mainCamera;

    private void Start()
    {
    mainCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        
        //hide healthbar behind camera
        Vector3 direction = (target.position - mainCamera.position).normalized;
        bool isBehind = Vector3.Dot(direction, mainCamera.forward) <= 0.0f;
        foregroundImage.enabled = !isBehind;
        backgroundImage.enabled = !isBehind;
    }

    public void SetHealthBarPercentage(float percentage)
    {
        
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
