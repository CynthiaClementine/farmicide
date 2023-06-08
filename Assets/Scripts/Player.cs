using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    
    public Color color;
    public float money;
    public GameObject bag;
    public Interactable focus;

    public float interactHold = 0f, interactTimer = 0f;
    public InputAction interactAction;
    public InputAction turnAction;

    public float moveSpeed = 5f;
    public Vector2 moveDir;
    private float friction = 0.85;
    private float accelSpeed = 0.2;
    private Rigidbody2D _rb;
    private PlayerGFX _playerGFX;
    private GameManager _manager;
    public Target target;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGFX = GetComponent<PlayerGFX>();
        target = GetComponent<Target>();
        target.owner = this;
        target.health = maxHealth;
        _manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        interactTimer = interactAction.ReadValue<float>() > 0f ? interactTimer + Time.deltaTime : 0f;
        if (interactTimer > interactHold)
        {
            interactTimer = 0f;
            OnInteract();
        }
        
        //movement
        Vector2 accelDir = turnAction.ReadValue<Vector2>().normalized;
        //apply friction when needed
        if (accelDir.x * moveDir.x <= 0) {
            moveDir.x *= friction;
        }
        if (accelDir.y * moveDir.y <= 0) {
            moveDir.y *= friction;
        }
        moveDir.x += accelDir.x;
        moveDir.y += accelDir.y;
        //make sure velocity never exceeds max val
        if (moveDir.sqrMagnitude > 1) {
            moveDir = moveDir.normalized;
        }
        _rb.velocity = moveDir * moveSpeed;
        
        if(focus || interactTimer > 0f) _playerGFX.DisplayUI();
    }

    void OnInteract()
    {
        if (focus)
        {
            focus.onInteract.Invoke(this);
        } else if (bag)
        {
            var obj = Instantiate(bag, transform.position, Quaternion.identity);
            var tobj = obj.GetComponent<Target>();
            if (tobj) tobj.owner = this;
            
            bag = null;
        }
    }

    public void Focus(Interactable interactable)
    {
        focus = interactable;
    }

    public void TakeDamage(float damage)
    {
        _playerGFX.DisplayUI();
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }
    
    private void OnEnable()
    {
        interactAction.Enable();
        turnAction.Enable();
    }

    private void OnDestroy()
    {
        _manager.LoseGame(this);
    }
}
