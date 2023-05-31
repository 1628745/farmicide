using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGFX : MonoBehaviour
{
    public GameObject mainGFX;
    public GameObject movementArrow;
    public GameObject ui;
    public Slider healthSlider;
    public TextMeshProUGUI interactionHint, bagHint, money;
    public CircleProgressBar interactionBar;
    
    public float idleTime = 1f;
    public float idleTimer;
    public float visibilitySpring = 2f;
    private float _desiredVisibility = 0f, _visibility = 0f;
    private Player _player;

    private Rigidbody2D _rb;

    void Start()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // in-game ui feedback
        movementArrow.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(_player.moveDir.y, _player.moveDir.x)*Mathf.Rad2Deg);
        mainGFX.transform.rotation = Quaternion.Euler(0f, _rb.velocity.x >= 0f ? 0f : 180f, 0f); // sprite flipping
        
        // hide/show detail panel
        idleTimer += Time.deltaTime;
        if (idleTimer > idleTime)
        {
            _desiredVisibility = 0f;
        }

        _visibility = Mathf.Lerp(_visibility, _desiredVisibility, Time.deltaTime*visibilitySpring);
        ui.transform.localScale = _visibility * (new Vector3(1, 1, 1));
        
        // detail panel
        interactionHint.text = _player.focus ? _player.focus.hint : "";
        bagHint.text = "Bag: " + (_player.bag ? _player.bag.name : "(empty)");
        money.text = "$ " + _player.money;
        
        healthSlider.value = _player.target.health/_player.maxHealth;
        interactionBar.SetProgress(_player.interactTimer / _player.interactHold);
    }

    public void DisplayUI()
    {
        _desiredVisibility = 1f;
        idleTimer = 0f;
    }
}
