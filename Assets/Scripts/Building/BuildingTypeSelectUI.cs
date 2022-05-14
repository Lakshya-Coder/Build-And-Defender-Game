using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;
    private Transform arrowBtn;
    private Transform btnTemplate;
    private float offsetAmount = 130f;

    private void Awake() =>
        SetUpBuildingTypeButtons();

    private void SetUpBuildingTypeButtons()
    {
        int index = 0;

        btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(
            nameof(BuildingTypeListSO)
        );

        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        SetUpArrowBtn(index);

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;

            Transform btnTransform = InstantiateBuildingBtn(index++, buildingType);
            btnTransformDictionary[buildingType] = btnTransform;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private Transform InstantiateBuildingBtn(int index, BuildingTypeSO buildingType)
    {
        Transform btnTransform = Instantiate(btnTemplate, transform);
        btnTransform.gameObject.SetActive(true);

        btnTransform.GetComponent<RectTransform>().anchoredPosition
            = new Vector2(offsetAmount * index, 0);

        btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

        btnTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(buildingType);
        });

        MouseEnterExitEvent mouseEnterExitEvent = btnTransform.GetComponent<MouseEnterExitEvent>();
        mouseEnterExitEvent.OnMouseEnter += (sender, e) =>
        {
            TooltipUI.Instance.Show(buildingType.nameString + "\n" +
                                    buildingType.GetConstructionResourceCostString());
        };
        mouseEnterExitEvent.OnMouseExit += (sender, e) => { TooltipUI.Instance.Hide(); };

        return btnTransform;
    }

    private void SetUpArrowBtn(int index)
    {
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -40);

        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        MouseEnterExitEvent mouseEnterExitEvent = arrowBtn.GetComponent<MouseEnterExitEvent>();
        mouseEnterExitEvent.OnMouseEnter += (sender, e) => { TooltipUI.Instance.Show("Arrow"); };
        mouseEnterExitEvent.OnMouseExit += (sender, e) => { TooltipUI.Instance.Hide(); };
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender,
        BuildingManager.OnActiveBuildingTypeChangedEventArgs e) => UpdateActiveBuildingTypeButton();

    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
            arrowBtn.Find("selected").gameObject.SetActive(true);
        else
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
    }
}