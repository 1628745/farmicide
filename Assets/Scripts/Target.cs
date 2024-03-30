using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
            // instantiate the health bar at the location of the object
            Vector3 newPosition = transform.position;
            newPosition.y += 1.5f;
            healthBar = Instantiate(healthBarPrefab, newPosition, Quaternion.identity, this.transform);
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
