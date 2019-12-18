using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PurseHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentGoldText;
    [SerializeField] private float _animationTimeInSeconds;
   
    private float _previousGoldAmount;
    private float _tGold;
    private float _currentGoldShown;
    private float _currentGoldAmount;
    
    private void Update()
    {
        if (_tGold < 1f)
        {
            _tGold += Time.deltaTime / _animationTimeInSeconds;
            float lerp = Mathf.Lerp(_previousGoldAmount, _currentGoldAmount, _tGold);
            _currentGoldShown = (int) lerp;
            _currentGoldText.text = _currentGoldShown.ToString(CultureInfo.InvariantCulture);
        }
    }
    
    public void UpgradePurseHudValues(float newGoldAmount)
    {
        _previousGoldAmount = _currentGoldShown;
        _currentGoldAmount = newGoldAmount;
        _tGold = 0f;
    }

    public void SetPurseHudValues(float currentGold)
    {
        _currentGoldShown = currentGold;
        _currentGoldText.text = currentGold.ToString(CultureInfo.InvariantCulture);
    }
}
