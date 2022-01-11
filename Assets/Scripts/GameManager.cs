using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float cash;


    private TextMeshProUGUI cashText;
    private TextMeshProUGUI hungerPrice, sleepPrice, staminaPrice, thirstPrice;
    private TextMeshProUGUI foodPrice, pillowPrice, coffeePrice, waterPrice;
    private float hungerPriceValue = 10f, sleepPriceValue = 10f, staminaPriceValue = 10f, thirstPriceValue = 10f;
    private float foodPriceValue = 10f, pillowPriceValue = 10f, coffeePriceValue = 1f, waterPriceValue = 1f;

    private void Start() {
        cash = 50f;
        cashText = GameObject.Find("CASH").GetComponent<TextMeshProUGUI>();

        hungerPrice = GameObject.Find("HungerPrice").GetComponent<TextMeshProUGUI>();
        sleepPrice = GameObject.Find("SleepPrice").GetComponent<TextMeshProUGUI>();
        staminaPrice = GameObject.Find("StaminaPrice").GetComponent<TextMeshProUGUI>();
        thirstPrice = GameObject.Find("ThirstPrice").GetComponent<TextMeshProUGUI>();
        foodPrice = GameObject.Find("FoodPrice").GetComponent<TextMeshProUGUI>();
        pillowPrice = GameObject.Find("PillowPrice").GetComponent<TextMeshProUGUI>();
        coffeePrice = GameObject.Find("CoffeePrice").GetComponent<TextMeshProUGUI>();
        waterPrice = GameObject.Find("WaterPrice").GetComponent<TextMeshProUGUI>();
        
        cashText.SetText("CASH:$" + cash);
        hungerPrice.SetText("$" + hungerPriceValue); sleepPrice.SetText("$" + sleepPriceValue);
        staminaPrice.SetText("$" + staminaPriceValue); thirstPrice.SetText("$" + thirstPriceValue);
        foodPrice.SetText("$" + foodPriceValue); pillowPrice.SetText("$" + pillowPriceValue);
        coffeePrice.SetText("$" + coffeePriceValue); waterPrice.SetText("$" + waterPriceValue);



        
    }
}
