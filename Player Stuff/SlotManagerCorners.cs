using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerCorners : MonoBehaviour
{
    public List<Transform> cornerSlots;

    public Transform GetCornerSlotPosition(int index)
    {
        return cornerSlots[index];
    }

    //Find closest corner slot
    public int FindClosestSlot(GameObject attacker)
    {
        int closestSlot = -1;
        float closestDistance = 9999f;

        //Cycle through all the slots to find the closest one
        for (int i = 0; i < cornerSlots.Count; ++i)
        {
            //find the closest slot to the attacker
            var distance = (GetCornerSlotPosition(i).transform.position - attacker.transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestSlot = i;
                closestDistance = distance;
            }
        }

        return closestSlot;
    }
}
