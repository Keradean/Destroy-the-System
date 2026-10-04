using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Entities/Components/Shooting")]
public class ShootingComponent : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public GameObject ProjectilePrefab => projectilePrefab;
    public Transform FirePoint => firePoint != null ? firePoint : transform;
}
