using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story_Cannon_Chargebar : MonoBehaviour
{
    [SerializeField] Color low;
    [SerializeField] Color high;
    [SerializeField] Image fillImage;

    [SerializeField] Story_Cannon storyCannon;

    private void Update()
    {
        if (storyCannon != null)
        {
            UpdateCharge(storyCannon.superweaponCharge, storyCannon.superweaponChargeTime);
        }
    }

    public void UpdateCharge(float charge, float maxCharge)
    {
        fillImage.fillAmount = charge / maxCharge;

        fillImage.color = Color.Lerp(low, high, charge / maxCharge);
    }
}
