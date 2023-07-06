using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public List<Transform> slots;
    
    public List<GameObject> slotAttackers;

    void Start()
    {
        slotAttackers = new List<GameObject>();

        for (int i = 0; i < slots.Count; ++i)
        {
            slotAttackers.Add(null);
        }
    }

    public Transform GetSlotPosition(int index)
    {
        return slots[index];
    }

    //Reserve a slot for one of the attacking enemies
    public int ReserveSlot(GameObject attacker)
    {
        var distance = 1.12f;

        var bestPosition = transform.position;
        var offset = (attacker.transform.position - bestPosition).normalized * distance;
        bestPosition += offset;
        int bestSlot = -1;
        float bestDistance = 99f;

        //We cycle through all the slots and find the closest one not currently in use
        for (int i = 0; i < slotAttackers.Count; ++i)
        {
            //If the slot is not currently in use...
            if (slotAttackers[i] != null)
                continue;

            //...then find the closest slot to the attacker
            var _distance = (GetSlotPosition(i).transform.position - bestPosition).sqrMagnitude;

            if (_distance < bestDistance)
            {
                bestSlot = i;  //pick the chosen slot
                bestDistance = _distance;
            }
        }

        //If we have a chosen slot then reserve it for the attacker
        if (bestSlot != -1)
            slotAttackers[bestSlot] = attacker;

        return bestSlot;
    }

    //Clear up a slot when its slotAttacker enemy is destroyed or picked another slot position
    public void ReleaseSlot(int index)
    {
        slotAttackers[index] = null;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < slotAttackers.Count; ++i)
        {
            if (slotAttackers == null || slotAttackers.Count <= i || slotAttackers[i] == null)
                Gizmos.color = Color.yellow; //if slot is empty it's colored yellow
            else
                Gizmos.color = Color.red;   //slot is red if it's taken
            Gizmos.DrawWireCube(GetSlotPosition(i).transform.position, new Vector3(0.16f, 0.16f, 0.16f));
        }
    }
}
