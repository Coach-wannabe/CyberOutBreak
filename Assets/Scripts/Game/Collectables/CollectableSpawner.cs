using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _collectablePrefabs;

    public void SpawnCollectable(Vector2 position)
    {
        int index = Random.Range(0, _collectablePrefabs.Count);
        var selectedCollectable = _collectablePrefabs[index];

        Instantiate(selectedCollectable, position, Quaternion.identity);
    }
}
