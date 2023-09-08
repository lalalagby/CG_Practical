using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static IKichenwareObejct;

public interface IKichenwareObejct
{
    /// <summary>
    /// teaBase:       tea or milkTea
    /// fruit:         fruit add,at most 2
    /// basicAdd:      sugar and ice
    /// ingredients :  red bean and pearl
    /// </summary>
    public enum MilkTeaMaterialType {
        none,
        teaBase,
        basicAdd,
        fruit,
        ingredients,
        unTreat,
    }

    [System.Serializable]
    public struct HeyTeaObejctStruct {
        public HeyTeaObjectSO heyTeaObjectSO;
        public HeyTeaObject heyTeaObject;
        public int maxNum;
        public int currentNum;
    }

    [System.Serializable]
    public struct MilkTeaMaterialQuota {
        public HeyTeaObejctStruct[] heyTeaObejctStructArray;
        public int totalSum;
        public bool canMixed;
        public MilkTeaMaterialType milkTeaMaterialType;
        
        public bool CanAdd(HeyTeaObjectSO heyTeaObjectSO) {
            if (canMixed&& heyTeaObejctStructArray[heyTeaObejctStructArray.Count()-1].currentNum>= heyTeaObejctStructArray[heyTeaObejctStructArray.Count() - 1].maxNum) {
                return false;
            }
            int sum = 0;
            foreach(HeyTeaObejctStruct heyTeaObejctStruct in heyTeaObejctStructArray) {
                if(heyTeaObjectSO== heyTeaObejctStruct.heyTeaObjectSO) {
                    //check the type
                    if (heyTeaObejctStruct.currentNum >= heyTeaObejctStruct.maxNum) {
                        return false;
                    }
                }
                sum += heyTeaObejctStruct.currentNum;
            }
            if (sum >= totalSum) {
                return false;
            } else {
                return true;
            }
        }

        public bool CheckMixed() {
            bool tag = true;
            if (canMixed) {
                for(int i = 0; i < heyTeaObejctStructArray.Count() - 1; i++) {
                    if (heyTeaObejctStructArray[i].currentNum < heyTeaObejctStructArray[i].maxNum) {
                        tag = false;
                    }
                }
            } else {
                tag = false;
            }
            return tag;
        } 
        public void ClearAll() {
            for (int i = 0; i < heyTeaObejctStructArray.Count(); i++) {
                heyTeaObejctStructArray[i].heyTeaObject = null;
                heyTeaObejctStructArray[i].currentNum = 0;
            }
        }

        public bool AddHeyTeaObject(HeyTeaObject heyTeaObject) {
            for(int i=0;i< heyTeaObejctStructArray.Count(); i++) {
                if (heyTeaObject.GetHeyTeaObjectSO() == heyTeaObejctStructArray[i].heyTeaObjectSO) {
                    heyTeaObejctStructArray[i].heyTeaObject = heyTeaObject;
                    heyTeaObejctStructArray[i].currentNum++;
                    return true;
                }
            }
            return false;
        }
    }

    [System.Serializable]
    public struct HeyTeaObjectTransform {
        public Transform parentTransform;
        public Vector3[] postionBiasList;
        public Vector3[] rotationBiasList;
        public Vector3[] scaleFactorList;
        public MilkTeaMaterialType milkTeaMaterialType;
        public int maxShow;
        public int maxStore;

        public void SetTransform(Transform transform) {
            transform.parent = parentTransform;
            transform.localPosition = postionBiasList[0];
            transform.localEulerAngles = rotationBiasList[0];
            transform.localScale = scaleFactorList[0];

            if (parentTransform.childCount > maxShow) {
                transform.gameObject.SetActive(false);
                Debug.Log(transform.name + " is set " + false);
            }
            if (transform.childCount > 1) {
                for (int i=0 ; i < transform.childCount;i ++) {
                    GameObject child = transform.GetChild(i).gameObject;
                    bool tag = child.activeSelf ? false:true ;
                    child.gameObject.SetActive(tag);
                    Debug.Log(child.name+" is set "+tag);
                }
            }
        }

        public MilkTeaMaterialType GetMilkTeaMaterialType() {
            return milkTeaMaterialType;
        }

        public void ResetLayer(int layer) {
            for(int i = 0; i < parentTransform.childCount; i++) {
                Transform child = parentTransform.GetChild(i);
                child.localPosition = postionBiasList[layer];
                child.localEulerAngles = rotationBiasList[layer];
                child.localScale = scaleFactorList[layer];
                child.parent = parentTransform;
            }
        }

        public Transform GetParentTransform() {
            return parentTransform;
        }

       public bool CanSetParent(MilkTeaMaterialType milkTeaMaterialType) {
            if (this.milkTeaMaterialType == milkTeaMaterialType&&parentTransform.childCount<maxStore) {
                return true;
            } else {
                return false;
            }
        }
    }

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType);

    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) { heyTeaObjectSO = null;return false; }

    public bool InteractWithOtherKichenware(IKichenwareObejct kichenwareObejct) {
        if(kichenwareObejct.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO)) {
            return TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else {
            return false;
        }
    }

    public void DestroyChild(HeyTeaObjectSO heyTeaObjectSO) {

    }
}
