using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;
    private ResourceNearOverlay resourceNearOverlay;

    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearOverlay>();

        Hide();
    }

    private void Start() =>
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender,
        BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            resourceNearOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);

            if (e.activeBuildingType.hasResourceGeneratorData)
                resourceNearOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            else
                resourceNearOverlay.Hide();
        }
    }

    private void Update() =>
        transform.position = UtilsClass.GetMouseWorldPosition();

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide() =>
        spriteGameObject.SetActive(false);
}