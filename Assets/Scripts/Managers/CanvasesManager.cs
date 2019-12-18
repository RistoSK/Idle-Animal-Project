using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasesManager : MonoBehaviour
{
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _shopCamera;
    [SerializeField] private Canvas _shopCanvas;

    private void Start()
    {
        _shopCanvas.enabled = false;
            
        _shopCamera.gameObject.SetActive(false);
        _gameCamera.gameObject.SetActive(true);
    }
    
    public void ToggleShopCamera()
    { 
        if (_shopCamera.gameObject.activeInHierarchy)
        {
            _shopCanvas.enabled = false;
            
            _shopCamera.gameObject.SetActive(false);
            _gameCamera.gameObject.SetActive(true);
        }
        else
        {
            _shopCanvas.enabled = true;
            
            _shopCamera.gameObject.SetActive(true);
            _gameCamera.gameObject.SetActive(false);
        }
    }
}
