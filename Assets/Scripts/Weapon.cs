using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    // Parameters to set for each weapon
    // Spawn point for bullets
    public Transform bulletSpawn;

    // Particle effect for muzzle flash
    [SerializeField] private ParticleSystem muzzleFlash;

    // Visual tracer for bullets
    [SerializeField] private LineRenderer tracer;

    // Maximum range of bullets
    [SerializeField] private float bulletRange;

    // Rate of fire
    [SerializeField] private float fireRate;

    // Time taken to reload
    [SerializeField] private float reloadTime;

    // Flag indicating if the weapon is automatic
    [SerializeField] private bool isAutomatic;

    // Flag indicating if shooting animation should be played
    [SerializeField] private bool playAnimation;

    // Tag for enemies
    [SerializeField] private string EnemyTag;

    // Horizontal spread of bullets
    [SerializeField] private float horizontalSpread;

    // Vertical spread of bullets
    [SerializeField] private float verticleSpread;

    // Delay between bursts
    [SerializeField] private float burstDelay;

    // Number of bullets per burst
    [SerializeField] private int bulletPerBurst;

    // Damage per bullet
    [SerializeField] private float damage;

    // Static variables to track ammunition
    public static int magazineSize;
    public static int reserveAmmo;
    public static int ammoLeft;

    // Number of bullets shot in a single instance
    private int bulletsShot;

    // Flag indicating if the weapon is currently shooting
    private bool isShooting;

    // Flag indicating if the weapon is ready to shoot
    private bool readytoShoot;

    // Flag indicating if the weapon is currently reloading
    private bool reloading;

    // Reference to the player GameObject
    public static GameObject player;

    // Reference to the Animator component
    private Animator animator;

    // Reference to the AudioSource component for shooting sound
    private AudioSource audioSource;

    // Reference to the AudioSource component for reloading sound
    public AudioSource pump;

    // Static references to UI elements
    public static TextMeshProUGUI ammoDisplay;
    public static RawImage crosshair;

    // Reference to the input manager
    private InputManager controls;

    // Original range of bullets
    private float originalBulletRange;

    // Flag indicating if the weapon can shoot
    private bool canShoot;

    // Flag indicating if the weapon is active
    public static bool active = true;

    // Flag indicating if a bullet has hit something
    private bool hasHit;

    // Number of consecutive hits
    private int consecutiveHits;

    private void Awake()
    {
        originalBulletRange = bulletRange;
        canShoot = true;
        readytoShoot = true;

        controls = new InputManager();

        audioSource = GetComponent<AudioSource>();

        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }

        controls.Player.Shoot.started += ctx => StartShot();
        controls.Player.Shoot.canceled += ctx => EndShot();
        controls.Player.Reload.performed += ctx => Reload();
    }

    private void Start()
    {
        consecutiveHits = 0;
        magazineSize = AmmoManager.currentMagazine;
        reserveAmmo = AmmoManager.currentReserve;
        ammoLeft = AmmoManager.currentCapacity;
        animator = player.GetComponent<Animator>();
    }

    public void Update()
    {
        if (active)
        {
            ammoDisplay.text = ammoLeft + " / " + reserveAmmo;

            canShoot = !reloading && ammoLeft > 0;

            // Check to see if player is able to shoot
            if (isShooting && readytoShoot && !reloading && ammoLeft > 0)
            {
                canShoot = true;
                bulletsShot = bulletPerBurst;
                PerformShot();
            } 

            // Display if Player is able to shoot, reload for play if ammo runs out
            if (ammoLeft == 0 && reserveAmmo == 0)
            {
                crosshair.color = Color.red;
                crosshair.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            } else if (ammoLeft == 0)
            {
                crosshair.color = Color.red;
                crosshair.GetComponentInChildren<TextMeshProUGUI>().SetText("Reloading");
                Reload();
            }

            // Checks to see if ray is hitting a enemy
            RaycastHit hit;
            if (canShoot)
            {
                if (Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit, bulletRange))
                {
                    // If it see then turn crosshair green
                    if (hit.collider.gameObject.tag == EnemyTag)
                    {
                        crosshair.color = Color.green;
                    }
                }
                else
                {
                    crosshair.color = Color.white;
                }
                crosshair.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            }
        }
    }

    private void StartShot()
    {

        isShooting = true;
        if (!canShoot)
        {
            crosshair.color = Color.red;
        }
    }

    private void EndShot()
    {
        isShooting = false;
    }

    private void PerformShot()
    {
        readytoShoot = false;


        // Account bullet spread if weapon has any
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-verticleSpread, verticleSpread);
        Vector3 direction = bulletSpawn.forward + new Vector3(x, y, 0);

        Ray ray = new Ray(bulletSpawn.position, direction);
        RaycastHit hit;

        // Deal damage if an enemy is hit
        if (Physics.Raycast(bulletSpawn.position, direction, out hit, bulletRange))
        {
            if (hit.collider.gameObject.tag == EnemyTag)
            {
                hasHit = true;
            }
            bulletRange = hit.distance;
        }
        else
        {
            hasHit = false;
            bulletRange = originalBulletRange;
            consecutiveHits = 0;
        }
        muzzleFlash.Play();
        audioSource.Play();

        if(hasHit)
        {
            if (hit.collider.gameObject.tag == EnemyTag)
            {
                consecutiveHits++;
                hit.collider.GetComponent<BasicEnemy>().takeDamage(damage, consecutiveHits);
            }    
        }

        // Make bullet tracer appar after shooting
        if (tracer)
        {
            // renders bullet tracer in the direction firing
            StartCoroutine("RenderTracer", ray.direction * bulletRange);
        }

        Debug.DrawRay(ray.origin, ray.direction * bulletRange, Color.yellow, 1);

        //Trigger Shooting Animation
        if (playAnimation)
        {
            animator.SetTrigger("shoot");
        }

        if (pump != null)
        {
            pump.Play();
        }

        ammoLeft--;
        bulletsShot--;

        // Shoots the weapon in burst
        if (bulletsShot > 0 && ammoLeft > 0)
        {
            Invoke("ResumeBurst", burstDelay);
        }
        else
        {
            Invoke("ResetShot", fireRate);
            // Shoots the weapon in automatic
            if (!isAutomatic)
            {
                EndShot();
            }
        }
    }

    //Renders the tracer
    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        // enable the tracer for one frame
        tracer.enabled = true;

        //render the tracer from the start of the gun barrel to the target hit
        tracer.SetPosition(0, bulletSpawn.position);
        tracer.SetPosition(1, bulletSpawn.position + hitPoint);

        // disable the tracer
        yield return new WaitForSeconds(0.005f);
        tracer.enabled = false;
    }
    private void ResetShot()
    {
        readytoShoot = true;
    }

    private void ResumeBurst()
    {
        readytoShoot = true;
        PerformShot();
    }

    private void Reload()
    {
        if (reserveAmmo > 0 && reloading == false && ammoLeft < magazineSize)
        {
            reloading = true;
            Invoke("ReloadFinish", reloadTime);
            animator.SetTrigger("reload");
            crosshair.GetComponentInChildren<TextMeshProUGUI>().SetText("Reloading");
        }
    }

    private void ReloadFinish()
    {
        reloading = false;
        // Calculate how much ammo to reload
        int ammoNeeded = magazineSize - ammoLeft;
        int ammoAvailable = Mathf.Min(ammoNeeded, reserveAmmo);

        // Subtract the ammo needed from reserve and add it to the magazine
        reserveAmmo -= ammoAvailable;
        ammoLeft += ammoAvailable;
    }



    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}