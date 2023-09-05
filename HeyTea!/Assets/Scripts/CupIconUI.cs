using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        var sortDic = from pair in cupObject.GetMilkTeaMaterialDic() orderby pair.Key ascending select pair;
        foreach (KeyValuePair<CupObject.MilkTeaMaterialType,CupObject.MilkTeaMaterialQuota> entry in sortDic) {
            for (int i= 0;i < entry.Value.heyTeaObjectSOArray.Count();i ++) {
                if (entry.Value.currentNum[i] != 0) {
                    for(int j=0;j< entry.Value.currentNum[i]; j++) {
                        Transform iconTransform = Instantiate(iconTemplate, transform);
                        iconTransform.gameObject.SetActive(true);
                        iconTransform.GetComponent<CupIconSingleUI>().SetHeyTeaObjectSO(entry.Value.heyTeaObjectSOArray[i]);
                    }
                }
            }
        }
    }

}
