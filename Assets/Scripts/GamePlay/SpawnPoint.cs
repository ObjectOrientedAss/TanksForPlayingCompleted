using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int Index;

    /// <summary>
    /// Checks if there are players nearby. Returns true if there is no one near.
    /// </summary>
    /// <returns></returns>
    public bool IsFree()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, LayerMask.NameToLayer("Tank"));
        return colliders.Length == 0;
    }
}
