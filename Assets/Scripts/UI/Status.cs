using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private TextMeshProUGUI diamond;
    [SerializeField] private TextMeshProUGUI ticket;
    
    private void Start()
    {
        GameManager.Instance.OnUserDataChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnUserDataChanged -= UpdateUI;
    }

    private void UpdateUI(UserData userData)
    {
        money.text = userData.playerMoney.ToString();
        diamond.text = userData.playerDiamond.ToString();
        ticket.text = userData.playerTicket.ToString();
    }
}
