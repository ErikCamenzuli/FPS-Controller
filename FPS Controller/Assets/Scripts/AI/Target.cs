<<<<<<< HEAD:FPS Controller/Assets/Scripts/Target.cs
﻿using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float hp = 50f;
    public float currentHp;
    public Text hpText;

    void Start()
    {
        currentHp = hp;               
    }

    void OnGUI()
    {
        hpText.text = "HP: " + hp + "/" + currentHp.ToString();
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0f)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

}
=======
﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Target : MonoBehaviour
{
    public float hp = 50f;
    public float currentHp;
    public Text hpText;

    public GameObject damageEffects;

    public AudioClip soundClip;
    public AudioSource soundSource;

    public GameObject gameObjectRespawn;

    void Start()
    {
        soundSource.clip = soundClip;
        currentHp = hp;
    }

    void Awake()
    {
        RespawnObject();        
    }

    void OnGUI()
    {
        hpText.text = "HP: " + hp + "/" + currentHp.ToString();
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        soundSource.Play();

        GameObject effect = (GameObject)Instantiate(damageEffects, transform.position,transform.rotation);
        Destroy(effect, 2f);

        
        if (hp <= 0f)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    void RespawnObject()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(gameObjectRespawn.gameObject);
            hp = 50f;            
        }
    }

}
>>>>>>> BranchErik:FPS Controller/Assets/Scripts/AI/Target.cs
