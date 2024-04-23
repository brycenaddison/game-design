/**
 * GUI element to receive input from the Settings.
 *
 * Author: Michael
 * Date: 4 / 23 / 24
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingPercent : MonoBehaviour {
  public Slider sliderUI;
  private Text textSliderValue;

  void Start (){
    textSliderValue = GetComponent<Text>();
  }

  public void ValueAI() {
    string sliderMessage = sliderUI.value + " Competitors";
    textSliderValue.text = sliderMessage;
  }

  public void ValueMap() {
    string sliderMessage = sliderUI.value + " x " + sliderUI.value;
    textSliderValue.text = sliderMessage;
  }

  public void ValueDif() {
    string sliderMessage = sliderUI.value + "%";
    textSliderValue.text = sliderMessage;
  }
}
