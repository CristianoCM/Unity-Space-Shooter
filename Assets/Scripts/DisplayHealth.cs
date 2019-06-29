using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    Text healthText;
    Player player;

    private void Start()
    {
        healthText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player)
        {
            float numberOfSignalsF = player.GetHealth() / player.GetDamageBase();
            int numberOfSignalsI = Mathf.CeilToInt(numberOfSignalsF);
            healthText.text = new String('+', numberOfSignalsI);
        }
    }

    public void SetZeroHealth()
    {
        healthText.text = String.Empty;
    }
}
