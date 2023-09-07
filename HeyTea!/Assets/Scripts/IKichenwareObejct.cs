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
        public bool isShowVisual;
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
        public Vector3[] postionBiasList;
        public Vector3[] rotationBiasList;
        public Vector3[] scaleFactorList;
        public MilkTeaMaterialType milkTeaMaterialType;
        public bool hasUsed;

        public void SetTransform(Transform transform,Transform parentTransform,int layer) {
            transform.parent = parentTransform;
            transform.localPosition = postionBiasList[layer];
            transform.localEulerAngles = rotationBiasList[layer];
            transform.localScale = scaleFactorList[layer];
        }

        public void ChangeTransform(Transform parentTransform, int layer) {
            foreach(Transform child in parentTransform) {
                child.localPosition = postionBiasList[layer];
                child.localEulerAngles = rotationBiasList[layer];
                child.localScale = scaleFactorList[layer];
            }
        }
       public bool CanSetParent(MilkTeaMaterialType milkTeaMaterialType) {
            if (this.milkTeaMaterialType == milkTeaMaterialType && hasUsed == false) {
                return true;
            } else {
                return false;
            }
        }

        public void SetUsed(bool used) {
            this.hasUsed = used;
        }
    }

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType);

    public void Interact(IKichenwareObejct kichenwareObejct) {

    }

    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) { heyTeaObjectSO = null;return false; }
}
