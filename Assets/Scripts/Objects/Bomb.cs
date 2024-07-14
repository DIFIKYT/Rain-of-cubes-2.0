using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    private void OnEnable()
    {
        
    }

    //private IEnumerator IncreasingTransparency()
    //{

    //}

    private void Explode()
    {
        Vector3 explosionPosition = gameObject.transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rigiBody))
                rigiBody.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius);
        }
    }
}