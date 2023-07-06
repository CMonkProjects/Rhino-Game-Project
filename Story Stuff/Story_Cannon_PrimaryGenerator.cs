using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_Cannon_PrimaryGenerator : MonoBehaviour
{
    [SerializeField] DestructableHealth destructableHealth;

    [SerializeField] List<BoxCollider2D> hitboxList;

    [SerializeField] string messageWhenDestroyed;

    void OnEnable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed += Destroyed;
    }

    void Start()
    {
        GameEvents.current.onAllGeneratorsDestroyed += PrimaryGeneratorOnline;
    }

    void OnDisable()
    {
        if (destructableHealth != null)
            destructableHealth.onDestroyed -= Destroyed;
    }

    void OnDestroy()
    {
        GameEvents.current.onAllGeneratorsDestroyed -= PrimaryGeneratorOnline;
    }

    void PrimaryGeneratorOnline()
    {
        GameEvents.current.PrimaryGeneratorOnline();

        foreach(BoxCollider2D hitbox in hitboxList)
        {
            hitbox.enabled = true;
        }

        GameEvents.current.NewMessage("Primary Generator Online");
    }

    void Destroyed()
    {
        GameEvents.current.PrimaryGeneratorDestroyed();

        GameEvents.current.NewMessage(messageWhenDestroyed);

        GameEvents.current.LevelEnd();
    }
}
