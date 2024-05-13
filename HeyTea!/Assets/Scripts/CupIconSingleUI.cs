using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    public void SetHeyTeaObjectSO(HeyTeaObjectSO heyTeaObjectSO) {
        image.sprite = heyTeaObjectSO.sprite;
    }
}
