using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CupObject;

[System.Serializable]
public class CupObject : HeyTeaObject {
    /// <summary>
    /// teaBase:       tea or milkTea
    /// fruit:         fruit add,at most 2
    /// basicAdd:      sugar and ice
    /// ingredients :  red bean and pearl
    /// </summary>
    public enum MilkTeaMaterialType {
        none,
        teaBase,
        fruit,
        basicAdd,
        ingredients,
    }

    //define the struct so that can be seen as a dictionary
    [System.Serializable]
    public struct MilkTeaMaterialQuota {
        public HeyTeaObjectSO[] heyTeaObjectSOArray;
        public int[] maxNum;
        public int totalSum;
        public bool canMixed;
        public int[] currentNum;
        public MilkTeaMaterialType milkTeaMaterialType;
    }
    //use the list to get the inspector
    public List<MilkTeaMaterialQuota> milkTeaMaterialQuotasList = new List<MilkTeaMaterialQuota>();
    public Dictionary<MilkTeaMaterialType, MilkTeaMaterialQuota> milkTeaMaterialQuotasDic = new Dictionary<MilkTeaMaterialType, MilkTeaMaterialQuota>();

    //store the object that add in the milk tea
    private List<HeyTeaObjectSO> heyTeaObjectSOList;

    private void Awake() {
        //init the list
        heyTeaObjectSOList = new List<HeyTeaObjectSO>();

        //move the object from list to dictionary
        for (int i = 0; i < milkTeaMaterialQuotasList.Count; i++) {
            milkTeaMaterialQuotasDic.Add(milkTeaMaterialQuotasList[i].milkTeaMaterialType, milkTeaMaterialQuotasList[i]);
        }
        //set currnt add object to zero,but maybe not need.
        SetCurrentMilkTeaMatrialQuotasDicAllZero();
    }

    private void SetCurrentMilkTeaMatrialQuotasDicAllZero() {
        foreach(KeyValuePair<MilkTeaMaterialType, MilkTeaMaterialQuota> entry in milkTeaMaterialQuotasDic) {
            for(int i = 0; i < entry.Value.maxNum.Count(); i++) {
                entry.Value.currentNum[i] = 0;
            }
        }
    }

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        bool tag = false;
        if (milkTeaMaterialQuotasDic.ContainsKey(milkTeaMaterialType)) {
            //if can be add in cup
            MilkTeaMaterialQuota milkTeaMaterialQuota = milkTeaMaterialQuotasDic[milkTeaMaterialType];
            int sum = milkTeaMaterialQuota.currentNum.Sum();
            //if sum of each object num is greater than totalsum,it cant be added anymore
            if (sum>=milkTeaMaterialQuota.totalSum){
                return false;
            }
            for(int i = 0; i < milkTeaMaterialQuota.heyTeaObjectSOArray.Count(); i++) {
                if (milkTeaMaterialQuota.heyTeaObjectSOArray[i]== heyTeaObjectSO) {
                    //If the item already has a limit of so much,  cannot continue to add it
                    if (milkTeaMaterialQuota.currentNum[i]>= milkTeaMaterialQuotasDic[milkTeaMaterialType].maxNum[i]) {
                        tag = false;;
                    } else {
                        if (milkTeaMaterialQuota.canMixed) {
                            if(milkTeaMaterialQuota.currentNum[milkTeaMaterialQuota.maxNum.Count() - 1] == 0) {
                                milkTeaMaterialQuota.currentNum[i]++;
                                heyTeaObjectSOList.Add(heyTeaObjectSO);
                                if (milkTeaMaterialQuota.currentNum.Sum() == milkTeaMaterialQuota.maxNum.Count() - 1) {
                                    for (int j = 0; j < milkTeaMaterialQuota.maxNum.Count() - 1; j++) {
                                        milkTeaMaterialQuota.currentNum[j] = 0;
                                        heyTeaObjectSOList.Remove(milkTeaMaterialQuota.heyTeaObjectSOArray[j]);
                                    }
                                    milkTeaMaterialQuota.currentNum[milkTeaMaterialQuota.maxNum.Count() - 1]++;
                                    heyTeaObjectSOList.Add(milkTeaMaterialQuota.heyTeaObjectSOArray[milkTeaMaterialQuota.maxNum.Count() - 1]);
                                }
                                tag = true;
                                return tag;
                            } else {
                                tag = false;
                            }
                        } else {
                            //modify the number of object
                            milkTeaMaterialQuota.currentNum[i]++; 
                            //modify the object list,later will use this list to show icon.
                            heyTeaObjectSOList.Add(heyTeaObjectSO);
                            tag = true;
                            return tag;
                        }  
                    }
                } 
            }
        } else {
            return false;
        }
        return tag;
    }

    
}
