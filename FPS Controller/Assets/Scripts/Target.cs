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
