using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TargetDamageEvent : UnityEvent<float> {}

public class Target : MonoBehaviour
{
    public Player owner;

    public float health;
    public TargetDamageEvent onDamage;
    public Object healthBarPrefab;
    public float maxHealth;
    public ObjectHealthBar healthBarScript;
    public Object healthBar;
    
    public void Start()
    {
        // get the health bar from the gameView where it is hidden but named "healthBar"
        if (gameObject.CompareTag("Object"))
        {
            healthBarPrefab = GameObject.Find("HealthBar");
            // instantiate the health bar
            healthBar = Instantiate(healthBarPrefab, this.transform);
            healthBarScript = healthBar.GetComponent<ObjectHealthBar>();
        }
        maxHealth = health;
        
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0) Destroy(gameObject);
        onDamage.Invoke(damage);
        if (healthBarScript) healthBarScript.DisplayUI();
    }
}
