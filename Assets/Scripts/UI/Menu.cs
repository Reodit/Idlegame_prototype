using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button equipButton;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private Canvas equipCanvas;

    private void Start()
    {
        equipButton.onClick.AddListener(() => 
            equipCanvas.gameObject.SetActive(!equipCanvas.gameObject.activeSelf));
    }
}
