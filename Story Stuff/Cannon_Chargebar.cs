using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon_Chargebar : MonoBehaviour
{
    public GameObject chargeBar;

    [SerializeField] Story_Cannon superweaponCannon;

    Vector3 offSet = new Vector3(0, -0.2f, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = superweaponCannon.transform.position + offSet;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChargeBar();
    }

    void UpdateChargeBar()
    {
        if (superweaponCannon == null)
            return;

        chargeBar.transform.localScale = new Vector3(superweaponCannon.superweaponCharge, 0.5f);
    }
}
