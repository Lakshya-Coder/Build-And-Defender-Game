using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{
    [SerializeField] private Building building;

    private void Awake() =>
        transform.Find("button").GetComponent<Button>().onClick.AddListener(DestroyBuilding);

    private void DestroyBuilding()
    {
        BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;

        // Give back some percent of real cost
        foreach (ResourceAmount resourceAmount in buildingType.constructionResourceCostArray)
            ResourceManager.Instance.AddResource(resourceAmount.resourceType,
                Mathf.FloorToInt(resourceAmount.amount * 0.6f));

        Destroy(building.gameObject);
    }
}