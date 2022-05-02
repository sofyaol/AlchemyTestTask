using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Recipe 
{
    [SerializeField] private List<Ingredient> _ingredients;
    [SerializeField] private List<int> _temperatures;

    internal List<int> Temperatures
    {
        get => _temperatures;
        set => _temperatures = value;
    }
    
    internal  List<Ingredient> Ingredients
    {
        get => _ingredients;
        set => _ingredients = value;
    }
}


