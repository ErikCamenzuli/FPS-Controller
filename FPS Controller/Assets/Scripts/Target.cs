using UnityEngine;

public class Target : MonoBehaviour
{
    public float hp = 50f;

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
