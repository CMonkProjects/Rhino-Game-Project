using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    protected string weaponName;
    protected string ammoString = ": ";
    [SerializeField] protected Sprite weaponSprite;
    protected float ammo;
    public bool displayAmmo = true;
    //GUI
    public GameObject weaponGUIPrefab;
    internal GameObject weaponGUI;
    public Text weaponNameText;
    public Text weaponAmmoText;
    public Image weaponImage;

    internal GameObject gameUI;

    public Slider weaponBar;
    public bool hasWeaponBar = false;
    protected float currentBar;
    protected float currentBarMax;

    PlayerWeaponAmmo playerWeaponAmmo;
    int ammoIndex;

    // Start is called before the first frame update
    internal virtual void Start()
    {
        playerWeaponAmmo = transform.parent.GetComponent<PlayerWeaponAmmo>();
        ammoIndex = GetComponent<PlayerWeaponGeneric>().ammoIndex;

        ammo = playerWeaponAmmo.ammoList[ammoIndex].ammoCurrent;
        weaponName = playerWeaponAmmo.ammoList[ammoIndex].weaponName;

        CreateUI();
    }

    internal virtual void CreateUI()
    {
        gameUI = GameObject.Find("Game_UI");

        if (gameUI != null)
        {
            weaponGUI = Instantiate(weaponGUIPrefab, gameUI.transform);
            weaponNameText = weaponGUI.transform.Find("WeaponNameText").GetComponent<Text>();
            weaponAmmoText = weaponGUI.transform.Find("WeaponAmmoText").GetComponent<Text>();
            weaponImage = weaponGUI.transform.Find("WeaponImage").GetComponent<Image>();

            if (hasWeaponBar)
            {
                weaponBar = weaponGUI.transform.Find("WeaponBar").GetComponent<Slider>();
            }

            if (weaponNameText != null)
            {
                weaponNameText.text = weaponName;
            }

            if (weaponAmmoText != null && displayAmmo)
            {
                weaponAmmoText.text = ammoString + ammo;
            }
            else
            {
                weaponAmmoText.text = "";
            }

            if (weaponImage != null)
            {
                weaponImage.sprite = weaponSprite;
            }
        }
    }

    internal virtual void OnEnable()
    {
        if (weaponGUI != null)
            weaponGUI.SetActive(true);
    }

    internal virtual void OnDisable()
    {
        if (weaponGUI != null)
            weaponGUI.SetActive(false);
    }

    //Called by most player weapons
    internal virtual void UpdateUI(float _ammo)
    {
        ammo = _ammo;

        if (weaponAmmoText != null)
        {
            weaponAmmoText.text = ammoString + ammo;
        }
    }

    //This is called by the laser weapon
    internal virtual void UpdateBar(float _currentBar, float _fullBar)
    {
        currentBar = _currentBar;
        currentBarMax = _fullBar;

        if (weaponBar != null)
        {
            weaponBar.value = currentBar / currentBarMax;
        }
    }
}
