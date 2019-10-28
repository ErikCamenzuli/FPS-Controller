using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 10f;
    public float impactForce = 10f;
    public float fireRate = 15f;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlashParticle;
    public GameObject impactEffects;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private float nextTimeToFire = 0f;

    //Raycasting shots
    void Shoot()
    {

        muzzleFlashParticle.Play();
        RaycastHit hitInfo;

        if (Physics.Raycast(fpsCamera.transform.position, 
                            fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);

            Target target = hitInfo.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(hitInfo.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffects, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impact, 2f);
        }

    }
}
