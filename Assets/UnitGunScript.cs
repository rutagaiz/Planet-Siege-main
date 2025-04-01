using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUnitGunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [Tooltip("Gun objektas (šis pats, jei skriptas pridėtas ant Gun)")]
    [SerializeField] private GameObject gun;

    [Tooltip("Atstumas, per kurį vienetas mato taikinius")]
    [SerializeField] private float range = 5f;

    [Tooltip("Šūvių per sekundę kiekis")]
    [SerializeField] private float fireRate = 1f;

    [Tooltip("Layer'ai, kuriuose ieškoma taikinių")]
    [SerializeField] private LayerMask targetLayer;

    [Tooltip("Tag'ai, kuriuos laikyti taikiniais")]
    [SerializeField] private string[] targetTags;

    [Tooltip("Prefab'as, kuris bus paleidžiamas kaip kulka")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Pozicija, iš kurios bus paleista kulka")]
    [SerializeField] private Transform shootPoint;

    [Tooltip("Šaudo šis unit kaip Ally ar Enemy")]
    [SerializeField] private TurretFaction faction;

    private Transform currentTarget;
    public bool HasTarget = false;
    private float nextTimeToFire;
    private Vector2 shootDirection;

    private void Update()
    {
        currentTarget = FindNearestTarget();

        if (currentTarget != null)
        {
            HasTarget = true;
            shootDirection = (currentTarget.position - transform.position).normalized;

            if (gun != null)
                gun.transform.right = shootDirection;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, range, targetLayer);
            if (hit.collider != null && MatchesTargetTags(hit.collider.tag) && hit.transform == currentTarget)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }

            Debug.DrawRay(transform.position, shootDirection * range, Color.cyan);
        }
        else HasTarget = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        UnitBulletScript bulletScript = bullet.GetComponent<UnitBulletScript>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
            bulletScript.SetFaction(faction);
        }
    }

    private Transform FindNearestTarget()
    {
        List<GameObject> targets = new();

        foreach (string tag in targetTags)
        {
            targets.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }

        float shortestDist = range;
        Transform closest = null;

        foreach (GameObject obj in targets)
        {
            float dist = Vector2.Distance(transform.position, obj.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closest = obj.transform;
            }
        }

        return closest;
    }

    private bool MatchesTargetTags(string tag)
    {
        foreach (string t in targetTags)
        {
            if (t == tag) return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}