using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    
    public float idleTime = 1f;
    public float idleTimer;
    public float visibilitySpring = 2f;
    private float _desiredVisibility;
    public float visibility;
    public Target target;
    
    void Start()
    {
        target = GetComponentInParent<Target>();
        healthSlider = GetComponentInChildren<Slider>();
    }
    
    void Update()
    {
        if (target)
        {
            // hide/show detail panel
            idleTimer += Time.deltaTime;
            if (idleTimer > idleTime)
            {
                _desiredVisibility = 0f;
            }

            visibility = Mathf.Lerp(visibility, _desiredVisibility, Time.deltaTime * visibilitySpring);
            gameObject.transform.localScale = visibility * new Vector3(1, 1, 1);
            healthSlider.value = target.health / target.maxHealth;
        }
    }

    public void DisplayUI()
    {
        _desiredVisibility = 1f;
        idleTimer = 0f;
    }
    public void InteruptUI()
    {
        _desiredVisibility = 0f;
    }
}
