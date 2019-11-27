using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 10f;
    public float impactForce = 10f;
    [Tooltip("The higher this number is, the faster the fire rate increases." +
             " The Lower this number is, the slower it fires.")]
    public float fireRate;
    private float nextTimeToFire;

    public Camera fpsCamera;
    //public ParticleSystem muzzleFlashParticle;
    public GameObject impactEffects;

    public int maxAmmo = 50;
    public float reloadTime = 5f;
    private int currentAmmo;
    public Text currentAmmoText;

    private bool isReloading = false;

    public Animator animator;


    void Start()
    {
        currentAmmo = maxAmmo;
    }

    //should fix for when more than 1 weapon is implemented
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void OnGUI()
    {
        currentAmmoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo.ToString();    
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading)
        {
            return;
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentAmmo == maxAmmo)
            {
                return;
            }

            StartCoroutine(Reload());
            return;
        }

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetButton("Fire1") && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            Debug.Log("FIRE!");
        }
    }
    /// <summary>
    /// Reloading the gun!
    /// Setting animation to either true and waiting a small amount of time
    /// </summary>
    /// <returns></returns>
    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);
        Debug.Log("Reloading.");
        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    //Raycasting shots
    void Shoot()
    {
        int layerMask = 1 >> 8;
        layerMask = ~layerMask;
        //muzzleFlashParticle.Play();

        currentAmmo--;

        RaycastHit hitInfo;

      if (Physics.Raycast(fpsCamera.transform.position, 
                          fpsCamera.transform.forward, out hitInfo, range, layerMask))   
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

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
            GameObject impact = Instantiate(impactEffects, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impact, 2f);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
        }

    }
}
