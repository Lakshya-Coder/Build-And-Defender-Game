using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static void Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform buildingConstructionTransform =
            Instantiate(GameAssets.Instance.pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);
    }

    private float constructionTimer;
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;

        Instantiate(
            GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity
        );
    }

    private void SetBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

        spriteRenderer.sprite = buildingType.sprite;

        buildingTypeHolder.buildingType = buildingType;
    }

    private void Update() => HandleConstructionTimer();

    private void HandleConstructionTimer()
    {
        constructionTimer -= Time.deltaTime;

        UpdateConstructionMaterial();

        // Is time to place a real building
        if (constructionTimer <= 0f)
        {
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(
                GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity
            );

            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);

            Destroy(gameObject);
        }
    }

    private void UpdateConstructionMaterial() =>
        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());

    public float GetConstructionTimerNormalized() => 1 - constructionTimer / constructionTimerMax;
}