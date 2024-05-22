using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IKichenwareObejct;

[System.Serializable]
public class CupObject : HeyTeaObject,IKichenwareObejct {
    public event EventHandler OnIngredinetAdded;

    
    [SerializeField] public List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;
    [SerializeField] private List<HeyTeaObjectTransform> heyTeaObjectTransformList;

    //try to add something in the cup.
    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) { 
        for(int i = 0; i < milkTeaMaterialQuotaList.Count(); i++) {
            if(milkTeaMaterialType== milkTeaMaterialQuotaList[i].milkTeaMaterialType) {
                if (milkTeaMaterialQuotaList[i].CanAdd(heyTeaObjectSO)) {
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(heyTeaObjectSO, milkTeaMaterialType);
                    if (heyTeaObject == null) {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                        if (milkTeaMaterialQuotaList[i].CheckMixed()) {
                            heyTeaObject=CombineObject(milkTeaMaterialQuotaList[i]);
                            if (!milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                                Debug.LogError("Mixed Failed");
                            }    
                        }

                        for(int j = 0; j < heyTeaObjectTransformList.Count; j++) {
                            heyTeaObjectTransformList[j].ResetLayer(GetLayer());
                        }
                        
                        OnIngredinetAdded?.Invoke(this, EventArgs.Empty);
                        Debug.Log("add success");
                        return true; 
                    }
                }
            }
        }
        Debug.Log("add failed");
        return false;
    }

    public void DestroyChild(HeyTeaObjectSO heyTeaObjectSO) {
        if (heyTeaObjectSO == null) {
            return;
        }
        for (int i = 0; i < milkTeaMaterialQuotaList.Count; i++) {
            for (int j = 0; j < milkTeaMaterialQuotaList[i].heyTeaObejctStructArray.Count(); j++) {
                if (milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObjectSO == heyTeaObjectSO && milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject != null) {
                    DestroyImmediate(milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject.transform.gameObject);
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject = null;
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].currentNum = 0;
                }
            }
        }
    }

    //Get layer to set the child objects transform
    //four layer: teabase milk tea milktea 
    private int GetLayer() {
        int layer = 0;
        foreach(MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            if (milkTeaMaterialQuota.canMixed) {
                for(int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count(); i++) {
                    if (milkTeaMaterialQuota.heyTeaObejctStructArray[i].currentNum >0) {
                        layer = i+1;
                    }
                }
            }
        }
        return layer;
    }

    //if the cup have milk and tea, it will delete the milk and tea and generate milketea prefab
    private HeyTeaObject CombineObject(MilkTeaMaterialQuota milkTeaMaterialQuota) {
        for (int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count() - 1;i++) {
            DestroyImmediate(milkTeaMaterialQuota.heyTeaObejctStructArray[i].heyTeaObject.gameObject);
        }
        milkTeaMaterialQuota.ClearAll();

        return SpawnHeyTeaObejct(milkTeaMaterialQuota.heyTeaObejctStructArray[milkTeaMaterialQuota.heyTeaObejctStructArray.Count()-1].heyTeaObjectSO,milkTeaMaterialQuota.milkTeaMaterialType);
    }

    //when we put object into cup,we will generate a new object and delete the old object.
    public HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        Transform transform = Instantiate(heyTeaObjectSO.prefab);

        HeyTeaObject heyTeaObject = transform.GetComponent<HeyTeaObject>();

        for(int i=0;i< heyTeaObjectTransformList.Count(); i++) {
            if (heyTeaObjectTransformList[i].CanSetParent(milkTeaMaterialType)) {
                heyTeaObjectTransformList[i].SetTransform(heyTeaObject.transform);
                return heyTeaObject;
            }
        }

        return null;
    }

    //return the object list in the cup.
    public List<MilkTeaMaterialQuota> GetMilkTeaMaterialQuota() {
        return milkTeaMaterialQuotaList;
    }

    // Get HeyTeaObjectSO from this(Cup)
    public List<HeyTeaObjectSO> GetOutputHeyTeaObejctSOList()  {
        List<HeyTeaObjectSO> heyTeaObjectSOList = new List<HeyTeaObjectSO>();

        foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList)  {
            foreach (HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray)  {
                if (heyTeaObejctStruct.currentNum > 0)  {
                    heyTeaObjectSOList.Add(heyTeaObejctStruct.heyTeaObjectSO);
                }
            }
        }
        return heyTeaObjectSOList;
    }
}
