﻿using UnityEngine;
using UnityEngine.Events;

public class ShootingComponent : MonoBehaviour
{
    // Object Pool for projectiles
    GameObjectPool projectilePool;
    UnityAction projectileHit;

    public Transform shootingSpot;

    // The number of projectiles we can have in the air at once.
    public int activeProjectilesAtOnce;

    // The number of projectiles currently in the air. Public so the projectiles can subtract from it.
    [HideInInspector]
    public int activeProjectiles;

    // The number of projectiles we can shoot per second.
    public float shotsPerSecond;

    float nextFire;
    float fireRate;

    // Start is called before the first frame update
    void Awake()
    {
        projectilePool = GetComponent<GameObjectPool>();
        projectileHit += onProjectileHit;
        // Setup Fire rate
        fireRate = 1 / shotsPerSecond;
    }

    void Start()
    {
        EventBroker.StartListening("Projectile Hit", projectileHit);
    }

    // Update is called once per frame
    void Update()
    {
        nextFire += Time.deltaTime;
    }

    void OnEnable()
    {
        EventBroker.StopListening("Projectile Hit", projectileHit);
    }

    public void Shoot()
    {

        if (activeProjectilesAtOnce > 0 && activeProjectiles >= activeProjectilesAtOnce)
        {
            return;
        }

        if (nextFire > fireRate)
        {
            GameObject projectile = projectilePool.GetObject();
            projectile.transform.position = shootingSpot.position;
            projectile.transform.up = transform.up;

            nextFire = 0;
            activeProjectiles++;
        }

        //AudioSource.PlayClipAtPoint(manager.shootClip, Camera.main.transform.position);
    }

    void onProjectileHit()
    {
        activeProjectiles--;
    }
}
