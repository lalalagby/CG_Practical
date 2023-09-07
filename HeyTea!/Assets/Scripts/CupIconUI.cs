using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CupIconUI : MonoBehaviour
{
    [SerializeField] private CupObject cupObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        cupObject.OnIngredinetAdded += CupObject_OnIngredientAdded;
    }

    private void CupObject_OnIngredientAdded(object sender,System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTemplate) {
                continue;
            }
            Destroy(child.gameObject);
        }

        var sortList = cupObject.GetMilkTeaMaterialQuota().OrderBy(t => t.milkTeaMaterialType);

        foreach(IKichenwareObejct.MilkTeaMaterialQuota milkTeaMaterialQuota in sortList) {
            foreach(IKichenwareObejct.HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                for(int i = 0; i < heyTeaObejctStruct.currentNum; i++) {
                    Transform iconTransform = Instantiate(iconTemplate, transform);
                    iconTransform.gameObject.SetActive(true);
                    iconTransform.GetComponent<CupIconSingleUI>().SetHeyTeaObjectSO(heyTeaObejctStruct.heyTeaObjectSO);
                }
            }
        }
    }

}
