using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float hp = 50f;
    public float currentHp;
    public Text hpText;

    void Start()
    {
        currentHp = hp;
        hpText.text = "HP: " + currentHp + "/" + hp.ToString();                    
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
