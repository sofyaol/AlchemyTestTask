using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : Tracked
{
    [SerializeField] protected Text _infoLabel;
    [SerializeField] protected int _maxTemperature = 50;
    [SerializeField] protected int _levelCount = 2;
    
    protected int[] _temperatures; 
    protected Ingredient[] _ingredients;
    protected float _maxDistance = 0.4f;
    
    protected float _distanceStep;
    
    protected void Awake()
    {
        _temperatures = new int[_levelCount];
        
        int temperatureStep = _maxTemperature / 2;
        int currentTemperature = temperatureStep;
        
        FillTemperatureLevels(currentTemperature, temperatureStep);
        
        _distanceStep = _maxDistance / _levelCount;
        _ingredients = FindObjectOfType<IngredientsHolder>().GetList();
        
    }

    protected virtual void FillTemperatureLevels(int currentTemperature, int temperatureStep)
    {
       
    }

    internal void SetIngredientsTemperature()
    {
        foreach (var ingredient in _ingredients)
        {
            if (!ingredient.gameObject.activeSelf) continue;
            
            float distance = GetDistanceTo(ingredient);

            if (distance < _maxDistance)
            {
                int temperatureLevel = (int) ((_maxDistance - distance) / _distanceStep);
                SetOptimalTemperature(ingredient, temperatureLevel);
            }
        }
    }

    protected virtual void SetOptimalTemperature(Ingredient ingredient, int temperatureLevel)
    {
       
    }

    float GetDistanceTo(Ingredient ingredient)
    {
        return Vector3.Distance(ingredient.transform.position, transform.position);
    }
}
