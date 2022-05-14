using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    private static bool SHOW = true;
    private static bool HIDE = false;

    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;

    private void Awake()
    {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        buildingRepairBtn = transform.Find("pfBuildingRepairBtn");

        SetDemolishBtnVisibility(HIDE);
        SetRepairBtnVisibility(HIDE);
    }

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        if (healthSystem.IsFullHealth())
            SetRepairBtnVisibility(HIDE);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SetRepairBtnVisibility(SHOW);

        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);

        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Instantiate(GameAssets.Instance.pfBuildingDestroyedParticles, transform.position, Quaternion.identity);

        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);

        CinemachineShake.Instance.ShakeCamera(10f, .2f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);

        Destroy(gameObject);
    }

    private void SetRepairBtnVisibility(bool visibility)
    {
        if (buildingRepairBtn)
            buildingRepairBtn.gameObject.SetActive(visibility);
    }

    private void SetDemolishBtnVisibility(bool visibility)
    {
        if (buildingDemolishBtn)
            buildingDemolishBtn.gameObject.SetActive(visibility);
    }

    private void OnMouseEnter() => SetDemolishBtnVisibility(SHOW);

    private void OnMouseExit() => SetDemolishBtnVisibility(HIDE);
}