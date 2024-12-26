using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemy : MonoBehaviour
{
    // Reference to the ammo drop GameObject
    public GameObject ammoDrop;

    // Reference to the AudioSource component
    public AudioSource source;

    // Array of zombie sound clips
    public AudioClip[] zombieSounds;

    // Flag indicating if the zombie is a "big" zombie
    public bool isBig;

    // Effect to play upon zombie death
    public GameObject deathEffect;

    // Reference to the player GameObject
    public static GameObject player;

    // Current health of the zombie
    public float health;

    // Damage dealt by the zombie's attack
    public float attatckDamage;

    // Effect to play upon blood splatter
    public GameObject bloodEffect;

    // Reference to the NavMeshAgent component
    private NavMeshAgent agent;

    // Flag indicating if the zombie is dying
    public bool dying = false;

    // Total damage received by the zombie
    private float totalDamage;

    // Layers to consider for ground and player
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    // Point for the zombie to walk towards
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Time between attacks
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // Range at which the zombie can detect the player
    public float sightRange;
    // Range at which the zombie can attack the player
    public float attackRange;

    // Flags indicating if the player is within sight and attack range
    private bool playerInSightRange;
    private bool playerInAttackRange;

    // Replacement object upon zombie death
    public GameObject replacement;

    // Force of explosion upon zombie death
    public float explosionForce = 1000f;
    // Multiplier for explosion force
    public float explosionMutliplier = 0.025f;

    // Reference to the Animator component
    private Animator animator;

    // Movement speed of the zombie
    private float move;

    // Flag indicating if zombie audio is playing
    private bool isPlayingAudio = false;

    // Minimum time between audio plays
    public float minTime = 0.5f;
    // Maximum time between audio plays
    public float maxTime = 5f;

    // Number of consecutive hits on the zombie
    public static int consecutiveHits;


    public void Awake()
    {
        source = GetComponent<AudioSource>();  
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Wall"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Ground"), true);
    }

    public void takeDamage(float dmg, int hits)
    {

        if (!dying)
        {
            if (isCritical(hits))
            {
                Player.points += 10 + ((hits/3) * 10);
                dmg = Mathf.Ceil(dmg *1.5f);
            } else
            {
                Player.points += 10;
            }
            

            if (bloodEffect != null)
            {
                Vector3 offet = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 2f), Random.Range(-0.5f, 0.5f));
                GameObject blood = Instantiate(bloodEffect, transform.position + offet, Quaternion.identity);
                Destroy(blood, 25f);
            }
        }

        // Deal damage
        health -= dmg;
        totalDamage += dmg;

        // Display the correct death message
        if (health <= 0 && !dying && isCritical(hits))
        {
            DisplayPopup.show(gameObject, "EXECUTION", Color.magenta);
            Player.points += 2.5f * (Mathf.Ceil(totalDamage / 100f) * 100f);
            Die();
        } else if (health <= 0 && !dying)
        {
            DisplayPopup.show(gameObject, "FATAL", Color.red);
            Player.points += 1.25f * (Mathf.Ceil(totalDamage / 100f) * 100f);
            Die();
        }
        else if (!dying && isCritical(hits))
        {
            DisplayPopup.show(gameObject, "CRITICAL", Color.yellow);
        } else if (!dying)
            
        {
            DisplayPopup.show(gameObject, totalDamage.ToString());
        }

    }

   
    private bool isCritical(int hits)
    {
        // Check if shot is critical
        if (hits != 0 && hits % 3 == 0)
        {
            return true;
        }

        return false;
    }

    public void Die()
    {
        dying = true;
        dropAmmoChance();
        if (isBig)
        {
            explode();
        }
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position + Vector3.up, Quaternion.identity);
            Destroy(effect,3f);
        }
        gib();
    }

    private void dropAmmoChance()
    {
        // Ammo Drop chance
        float chance = Random.Range(0f, 1f);
        if (chance < 0.075f && WaveGeneration.waveNumber > 5)
        {
            Instantiate(ammoDrop, transform.position + Vector3.up, Quaternion.Euler(-90f, 0f, 0f));

        }
    }

    private void Update()
    {
        move = agent.velocity.magnitude;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            wander();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            chase();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            attack();
        }

        if (!isPlayingAudio && source != null)
        {
            StartCoroutine(zombieSound());
        }
    }

    private void explode()
    {
        // Death explosion
        float explosionRadius = 3f;
        var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {

            if (obj.CompareTag("Player"))
            {
                Player.takeDamage(50f);
            }

            if (obj.CompareTag("Enemy"))
            {
                obj.GetComponent<BasicEnemy>().takeDamage(25f, 1);
            }

            if (obj.CompareTag("Player") || obj.CompareTag("Enemy"))
            {
                var rb = obj.GetComponent<Rigidbody>();
                rb.AddExplosionForce(1000, transform.position, explosionRadius);
            }
        }
    }

    IEnumerator zombieSound()
    {

        isPlayingAudio = true;

        float delay = Random.Range(minTime, maxTime);


        yield return new WaitForSeconds(delay);

        // Play a random zombie sound
        int randomIndex = Random.Range(0, zombieSounds.Length);
        source.clip = zombieSounds[randomIndex];
        source.Play();

        yield return new WaitForSeconds(zombieSounds[randomIndex].length);

        isPlayingAudio = false;
    }

    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }
    
    private void wander()
    {
        if (!walkPointSet)
        {
            searchWalkPoint();
        } else
        {
            agent.SetDestination(walkPoint);
            animator.SetFloat("speed", move);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

        if (!alreadyAttacked && Player.health>=0)
        {

            //Attack
            Player.takeDamage(attatckDamage);

            //Play Attack animation
            animator.SetTrigger("attack");
            animator.SetFloat("speed", 0f);
            alreadyAttacked = true;
            Invoke("resetAttack", timeBetweenAttacks);
        }
    }

    private void resetAttack()
    {
        alreadyAttacked = false;
    }

    private void chase()
    { 
        agent.SetDestination(player.transform.position);
        animator.SetFloat("speed", move);
    }

    void gib()
    {
        // Replace old model with broken model and explode body parts
        var bodyPart = Instantiate(replacement, transform.position, Quaternion.Euler(0, transform.rotation.y - 90, -90));

        var partRbs = bodyPart.GetComponentsInChildren<Rigidbody>();
        bodyPart.GetComponent<AudioSource>().Play();

        int bodyPartLayer = LayerMask.NameToLayer("BodyPart");

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), bodyPartLayer, true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), bodyPartLayer, true);

        // Apply explosion force to each body part
        foreach (var partRb in partRbs)
        {

            partRb.gameObject.layer = bodyPartLayer;
            Vector3 randomDirection = Random.onUnitSphere * explosionMutliplier;

            // Display blood
            float bloodSpawnProbability = 0.05f;
            if (Random.value < bloodSpawnProbability)
            {
                Vector3 offet = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 2f), Random.Range(-0.5f, 0.5f));
                GameObject blood = Instantiate(bloodEffect, partRb.position + offet, Quaternion.identity);
                Destroy(blood, 0.5f);
            };
            partRb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);

        }

        Destroy(bodyPart, 3f);
        Destroy(gameObject);
    }

}
