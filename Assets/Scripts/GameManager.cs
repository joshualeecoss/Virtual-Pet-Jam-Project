using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float cash;
    private const float PRICE_INCREASE = 1.0f;

    private StatMeterController hungerMeter;
    private StatMeterController sleepMeter;
    private StatMeterController staminaMeter;
    private StatMeterController thirstMeter;

    private ZoneMeterController zoneMeter;

    private TextMeshProUGUI cashText;
    private TextMeshProUGUI hungerPrice, sleepPrice, staminaPrice, thirstPrice;
    private TextMeshProUGUI foodPrice, pillowPrice, coffeePrice, waterPrice;
    private float hungerPriceValue = 10f, sleepPriceValue = 10f, staminaPriceValue = 10f, thirstPriceValue = 10f;
    private float foodPriceValue = 1f, pillowPriceValue = 1f, coffeePriceValue = 1f, waterPriceValue = 1f;

    private void Start() {
        cashText = GameObject.Find("CASH").GetComponent<TextMeshProUGUI>();
        hungerPrice = GameObject.Find("HungerPrice").GetComponent<TextMeshProUGUI>();
        sleepPrice = GameObject.Find("SleepPrice").GetComponent<TextMeshProUGUI>();
        staminaPrice = GameObject.Find("StaminaPrice").GetComponent<TextMeshProUGUI>();
        thirstPrice = GameObject.Find("ThirstPrice").GetComponent<TextMeshProUGUI>();
        foodPrice = GameObject.Find("FoodPrice").GetComponent<TextMeshProUGUI>();
        pillowPrice = GameObject.Find("PillowPrice").GetComponent<TextMeshProUGUI>();
        coffeePrice = GameObject.Find("CoffeePrice").GetComponent<TextMeshProUGUI>();
        waterPrice = GameObject.Find("WaterPrice").GetComponent<TextMeshProUGUI>();

        hungerMeter = GameObject.Find("Meters/HungerMeter").GetComponent<StatMeterController>();
        sleepMeter = GameObject.Find("Meters/SleepMeter").GetComponent<StatMeterController>();
        staminaMeter = GameObject.Find("Meters/StaminaMeter").GetComponent<StatMeterController>();
        thirstMeter = GameObject.Find("Meters/ThirstMeter").GetComponent<StatMeterController>();
        zoneMeter = GameObject.Find("Meters/TheZoneMeter").GetComponent<ZoneMeterController>();

        
        cash = 500f;
        SetText();
    }

    private void Update() {
        SetText();
        ZoneCheck();
    }

    private void ZoneCheck() {
        if (hungerMeter.GetCurrentValue() >= hungerMeter.GetThreshold() &&
            sleepMeter.GetCurrentValue() >= sleepMeter.GetThreshold() &&
            staminaMeter.GetCurrentValue() >= staminaMeter.GetThreshold() &&
            thirstMeter.GetCurrentValue() >= thirstMeter.GetThreshold()) {
                zoneMeter.Increase();
            } else {
                zoneMeter.Decrease();
            }
    }

    private void SetText() {
        cashText.SetText("CASH:$" + cash);
        hungerPrice.SetText("$" + hungerPriceValue);    sleepPrice.SetText("$" + sleepPriceValue);
        staminaPrice.SetText("$" + staminaPriceValue);  thirstPrice.SetText("$" + thirstPriceValue);
        foodPrice.SetText("$" + foodPriceValue);        pillowPrice.SetText("$" + pillowPriceValue);
        coffeePrice.SetText("$" + coffeePriceValue);    waterPrice.SetText("$" + waterPriceValue);
    }

    public void FoodButtonClicked() {
        if (cash >= foodPriceValue) {
            cash -= foodPriceValue;
            foodPriceValue += PRICE_INCREASE;
            if (hungerMeter.GetCurrentValue() <= hungerMeter.GetMaxValue()) {
                hungerMeter.Fill();
            }
        }
    }

    public void PillowButtonClicked() {
        if (cash >= pillowPriceValue) {
            cash -= pillowPriceValue;
            pillowPriceValue += PRICE_INCREASE;
            if (sleepMeter.GetCurrentValue() <= sleepMeter.GetMaxValue()) {
                sleepMeter.Fill();
            }
        }
    }

    public void CoffeeButtonClicked() {
        if (cash >= coffeePriceValue) {
            cash -= coffeePriceValue;
            coffeePriceValue += PRICE_INCREASE;
            if (staminaMeter.GetCurrentValue() <= staminaMeter.GetMaxValue()) {
                staminaMeter.Fill();
            }
        }
    }

    public void WaterButtonClicked() {
        if (cash >= waterPriceValue) {
            cash -= waterPriceValue;
            waterPriceValue += PRICE_INCREASE;
            if (thirstMeter.GetCurrentValue() <= thirstMeter.GetMaxValue()) {
                thirstMeter.Fill();
            }
        }
    }

    public void HungerButtonClicked() {
        return;
    }

    public void SleepButtonClicked() {
        return;
    }

    public void StaminaButtonClicked() {
        return;
    }

    public void ThirstButtonClicked() {
        return;
    }

}
