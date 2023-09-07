using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IKichenwareObejct;

[System.Serializable]
public class CupObject : HeyTeaObject,IKichenwareObejct {
    public event EventHandler OnIngredinetAdded;

    
    [SerializeField] private List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;
    [SerializeField] private List<HeyTeaObjectTransform> HeyTeaObjectTransformList;
    [SerializeField] private Transform palcementPoint;

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        for(int i = 0; i < milkTeaMaterialQuotaList.Count(); i++) {
            if(milkTeaMaterialType== milkTeaMaterialQuotaList[i].milkTeaMaterialType) {
                if (milkTeaMaterialQuotaList[i].CanAdd(heyTeaObjectSO)) {
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(heyTeaObjectSO, milkTeaMaterialType);
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                        if (milkTeaMaterialQuotaList[i].CheckMixed()) {
                            heyTeaObject=CombineObject(milkTeaMaterialQuotaList[i]);
                            if (!milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                                Debug.LogError("Mixed Failed");
                            }    
                        }
                        ReSetAllObjectLayer();
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

    public void ReSetAllObjectLayer() {
        for (int i= 0; i < HeyTeaObjectTransformList.Count(); i++){
            if (HeyTeaObjectTransformList[i].milkTeaMaterialType == MilkTeaMaterialType.teaBase) {
                HeyTeaObjectTransformList[i].ChangeTransform(palcementPoint, 0);
            } else {
                HeyTeaObjectTransformList[i].ChangeTransform(palcementPoint,CheckLayer());
            }
        }
    }

    private int CheckLayer() {
        int layer = 0;
        foreach(MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            if (milkTeaMaterialQuota.canMixed) {
                for(int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count(); i++) {
                    if (milkTeaMaterialQuota.heyTeaObejctStructArray[i].currentNum >= 0) {
                        layer = i+1;
                    }
                }
            }
        }
        return layer;
    }
    private HeyTeaObject CombineObject(MilkTeaMaterialQuota milkTeaMaterialQuota) {
        for (int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count() - 1;i++) {
            Destroy(milkTeaMaterialQuota.heyTeaObejctStructArray[i].heyTeaObject.gameObject);
        }
        milkTeaMaterialQuota.ClearAll();
        return SpawnHeyTeaObejct(milkTeaMaterialQuota.heyTeaObejctStructArray[milkTeaMaterialQuota.heyTeaObejctStructArray.Count()-1].heyTeaObjectSO,milkTeaMaterialQuota.milkTeaMaterialType);
    }
    public HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        Transform transform = Instantiate(heyTeaObjectSO.prefab);

        HeyTeaObject heyTeaObject = transform.GetComponent<HeyTeaObject>();

        for(int i=0;i< HeyTeaObjectTransformList.Count(); i++) {
            if (HeyTeaObjectTransformList[i].CanSetParent(milkTeaMaterialType)) {
                HeyTeaObjectTransformList[i].SetTransform(heyTeaObject.transform, palcementPoint,0);
                HeyTeaObjectTransformList[i].SetUsed(true);
            }
        }

        return heyTeaObject;
    }

    public List<MilkTeaMaterialQuota> GetMilkTeaMaterialQuota() {
        return milkTeaMaterialQuotaList;
    }

    ///// <summary>
    ///// teaBase:       tea or milkTea
    ///// fruit:         fruit add,at most 2
    ///// basicAdd:      sugar and ice
    ///// ingredients :  red bean and pearl
    ///// </summary>

    ////define the struct so that can be seen as a dictionary
    //[System.Serializable]
    //public struct ObjectBias {
    //    public Vector3 postionBias;
    //    public Vector3 rotationBias;
    //    public Vector3 scaleFactor;
    //    public IKichenwareObejct.MilkTeaMaterialType milkTeaMaterialType;
    //}
    //[SerializeField] private List<ObjectBias> objectBias = new List<ObjectBias>();

    ////use the list to get the inspector
    //[SerializeField] private List<IKichenwareObejct.MilkTeaMaterialQuota> milkTeaMaterialQuotasList = new List<IKichenwareObejct.MilkTeaMaterialQuota>();
    //[SerializeField] private Dictionary<IKichenwareObejct.MilkTeaMaterialType, IKichenwareObejct. MilkTeaMaterialQuota> milkTeaMaterialQuotasDic = new Dictionary<IKichenwareObejct.MilkTeaMaterialType, IKichenwareObejct.MilkTeaMaterialQuota>();

    ////store the object that add in the milk tea
    //private List<HeyTeaObjectSO> heyTeaObjectSOList;

    //private void Awake() {
    //    //init the list
    //    heyTeaObjectSOList = new List<HeyTeaObjectSO>();

    //    //move the object from list to dictionary
    //    for (int i = 0; i < milkTeaMaterialQuotasList.Count; i++) {
    //        milkTeaMaterialQuotasDic.Add(milkTeaMaterialQuotasList[i].milkTeaMaterialType, milkTeaMaterialQuotasList[i]);
    //    }
    //    //set currnt add object to zero,but maybe not need.
    //    SetCurrentMilkTeaMatrialQuotasDicAllZero();
    //}

    //private void SetCurrentMilkTeaMatrialQuotasDicAllZero() {
    //    foreach(KeyValuePair<IKichenwareObejct.MilkTeaMaterialType, IKichenwareObejct.MilkTeaMaterialQuota> entry in milkTeaMaterialQuotasDic) {
    //        for(int i = 0; i < entry.Value.maxNum.Count(); i++) {
    //            entry.Value.currentNum[i] = 0;
    //        }
    //    }
    //}

    //public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, IKichenwareObejct.MilkTeaMaterialType milkTeaMaterialType) {
    //    bool tag = false;
    //    if (milkTeaMaterialQuotasDic.ContainsKey(milkTeaMaterialType)) {
    //        //if can be add in cup
    //        IKichenwareObejct.MilkTeaMaterialQuota milkTeaMaterialQuota = milkTeaMaterialQuotasDic[milkTeaMaterialType];
    //        int sum = milkTeaMaterialQuota.currentNum.Sum();
    //        //if sum of each object num is greater than totalsum,it cant be added anymore
    //        if (sum>=milkTeaMaterialQuota.totalSum){
    //            return false;
    //        }
    //        for(int i = 0; i < milkTeaMaterialQuota.heyTeaObjectSOArray.Count(); i++) {
    //            if (milkTeaMaterialQuota.heyTeaObjectSOArray[i]== heyTeaObjectSO) {
    //                //If the item already has a limit of so much,  cannot continue to add it
    //                if (milkTeaMaterialQuota.currentNum[i]>= milkTeaMaterialQuotasDic[milkTeaMaterialType].maxNum[i]) {
    //                    tag = false;;
    //                } else {
    //                    if (milkTeaMaterialQuota.canMixed) {
    //                        if(milkTeaMaterialQuota.currentNum[milkTeaMaterialQuota.maxNum.Count() - 1] == 0) {
    //                            milkTeaMaterialQuota.currentNum[i]++;
    //                            heyTeaObjectSOList.Add(heyTeaObjectSO);

    //                            if (milkTeaMaterialQuota.currentNum.Sum() == milkTeaMaterialQuota.maxNum.Count() - 1) {
    //                                for (int j = 0; j < milkTeaMaterialQuota.maxNum.Count() - 1; j++) {
    //                                    milkTeaMaterialQuota.currentNum[j] = 0;
    //                                    heyTeaObjectSOList.Remove(milkTeaMaterialQuota.heyTeaObjectSOArray[j]);
    //                                }
    //                                milkTeaMaterialQuota.currentNum[milkTeaMaterialQuota.maxNum.Count() - 1]++;
    //                                heyTeaObjectSOList.Add(milkTeaMaterialQuota.heyTeaObjectSOArray[milkTeaMaterialQuota.maxNum.Count() - 1]);
    //                            }

    //                            OnIngredinetAdded?.Invoke(this, EventArgs.Empty);

    //                            tag = true;
    //                            return tag;
    //                        } else {
    //                            tag = false;
    //                        }
    //                    } else {
    //                        //modify the number of object
    //                        milkTeaMaterialQuota.currentNum[i]++; 
    //                        //modify the object list,later will use this list to show icon.
    //                        heyTeaObjectSOList.Add(heyTeaObjectSO);

    //                        OnIngredinetAdded?.Invoke(this, EventArgs.Empty);

    //                        tag = true;
    //                        return tag;
    //                    }  
    //                }
    //            } 
    //        }
    //    } else {
    //        return false;
    //    }
    //    return tag;
    //}

    //public Dictionary<IKichenwareObejct.MilkTeaMaterialType, IKichenwareObejct.MilkTeaMaterialQuota> GetMilkTeaMaterialDic() {
    //    return milkTeaMaterialQuotasDic;
    //}
}
