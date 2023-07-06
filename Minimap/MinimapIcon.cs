using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [SerializeField] GameObject parentObject;   //object that is the parent for the minimap and target prefab
    [SerializeField] GameObject minimapPrefab, minimapTargetPrefab; //minimap icon prefabs
    GameObject minimapPrefabReference, minimapTargetPrefabReference;
    [SerializeField] PowerUp powerUp;

    void Awake()
    {
        powerUp = GetComponent<PowerUp>();

        CreateMinimapObject();
    }

    void OnEnable()
    {
        if (powerUp != null)
        {
            powerUp.onPowerupCollected += DestroyMinimapIcon;
        }
    }

    void OnDisable()
    {
        if (powerUp != null)
        {
            powerUp.onPowerupCollected -= DestroyMinimapIcon;
        }
    }

    void DestroyMinimapIcon()
    {
        if (minimapPrefabReference != null)
        {
            Destroy(minimapPrefabReference);
        }

        if (minimapTargetPrefabReference != null)
        {
            Destroy(minimapTargetPrefabReference);
        }
    }

    public void CreateMinimapObject()
    {
        if (minimapPrefab != null && parentObject != null)
        {
            minimapPrefabReference = Instantiate(minimapPrefab,
                parentObject.transform.position,
                Quaternion.identity);

            minimapPrefabReference.transform.parent = parentObject.transform;
        }
    }

    public void CreateTargetMinimapObject()
    {
        if (minimapTargetPrefab != null && parentObject != null)
        {
            minimapTargetPrefabReference = Instantiate(minimapTargetPrefab,
                parentObject.transform.position,
                Quaternion.identity);

            minimapTargetPrefabReference.transform.parent = parentObject.transform;
        }
    }
}
