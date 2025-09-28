using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.maxHealth / 10;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.health / 10;
    }
}
