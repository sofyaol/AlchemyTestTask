using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

[Serializable]
public class Ingredient : Tracked
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material native;
    
    [SerializeField] private GameObject[] _temperatureImages;
    [SerializeField] private int[] _possibleTemperatures;
    [SerializeField] private int _temperature;
    private Action _temperatureChanged;

    internal void SetMaterial(Material material)
    {
        _renderer.material = material;
    }

    internal void SetNativeMaterial()
    {
        _renderer.material = native;
    }

    internal int Temperature
    { 
        get => _temperature;
        set
        {
            if (value > 50 || value < 0) return;
            _temperature = value;
            _temperatureChanged?.Invoke();
        }
    }

    private void ChangeUI()
    {
        for(int i = 0; i < _possibleTemperatures.Length; i++)
        {
            if (Temperature == _possibleTemperatures[i])
            {
                foreach (var image in _temperatureImages)
                {
                    image.SetActive(false);
                }

                _temperatureImages[i].SetActive(true);
                StartCoroutine(DeactivateImageAt(i));
                break;
            }
        }
    }

    private IEnumerator DeactivateImageAt(int index)
    {
        yield return new WaitForSeconds(1.5f);
        _temperatureImages[index].SetActive(false);
    }
    
    private void OnEnable()
    {
        _temperatureChanged += ChangeUI;
    }

    private void OnDisable()
    {
        _temperatureChanged -= ChangeUI;
    }
}
