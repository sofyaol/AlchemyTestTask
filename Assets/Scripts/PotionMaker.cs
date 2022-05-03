using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private Text _infoLabel;
    [SerializeField] private Material _spoiled;
    private Ingredient[] _ingredients;
    private int _ingredientsCount;
    [SerializeField] private float maxCrossingDistance;
    private List<Recipe> _book;
    private TaskGenerator _taskGenerator;
    private int _lastIndex = 0;
    public Action PotionIsMade;
    
    void Start()
    {
        _book = FindObjectOfType<RecipesBook>().Book;
        _ingredients = FindObjectOfType<IngredientsHolder>().GetList();
        _ingredientsCount = _ingredients.Length;
        _taskGenerator = FindObjectOfType<TaskGenerator>();
        _taskGenerator.NewTask += () => _lastIndex = 0;
        StartCoroutine(CheckForCrossingIngredient());
    }

    private void OnDisable()
    {
        _taskGenerator.NewTask -= () => _lastIndex = 0;
    }

    IEnumerator CheckForCrossingIngredient()
    {
        while (true)
        {
            int i = 0;
            
            while (i < _ingredientsCount)
            {
                if (_ingredients[i].gameObject.activeSelf)
                {
                    for (int j = i + 1; j < _ingredientsCount; j++)
                    {
                        TryCreatePotionBy(_ingredients[i], _ingredients[j]);
                    }
                }

                i++;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void TryCreatePotionBy(Ingredient target, Ingredient other)
    {
        if (IsCrossingEnable(target, other))
        {
            if (IsCoupleRight(target, other))
            {
                _lastIndex++;
                
                if (_lastIndex == _book[_taskGenerator.CurrentRecipe].Ingredients.Count)
                {
                    _infoLabel.text = "Recipe is ready!";
                    PotionIsMade?.Invoke();
                    return;
                }

                _infoLabel.text = "Ingredients are Right!";
                
            }
            else
            {
                _infoLabel.text = "Ingredients are Spoiled!";
                IngredientsAreSpoiled(target, other);
            }
        }
    }

    private void IngredientsAreSpoiled(Ingredient target, Ingredient other)
    {
        target.SetMaterial(_spoiled);
        other.SetMaterial(_spoiled);
    }

    private bool IsCoupleRight(Ingredient target, Ingredient other)
    {

        var currentRecipe = _taskGenerator.CurrentRecipe;
        var recipe = _book[currentRecipe];
        
        if (target == recipe.Ingredients[_lastIndex])
        {
            return other == recipe.Ingredients[_lastIndex + 1] && 
                   CheckTemperaturesOf(target, other, recipe, _lastIndex, _lastIndex + 1);
        }

        if (other == recipe.Ingredients[_lastIndex])
        {
            return target == recipe.Ingredients[_lastIndex + 1] &&
                   CheckTemperaturesOf(target, other, recipe, _lastIndex + 1, _lastIndex);
        }

        return false;

        // 1. Находим рецепт с ингредиентом где есть target
        // 2. Проверяем, в этом рецепте второй ингредиент это other:
        // Если да - прорвеяем температуры, если подходят возвращаем true, иначе - false;
        // Если нет - continue.
        /*
        for(int r = 0; r < _book.Count; r++)
        {
            var recipe = _book[r];
            
            for(int t = 0; t < recipe.Ingredients.Count; t++)
            {
                var ingredient = recipe.Ingredients[t];
                    
                if (target == ingredient)
                {
                    for(int o = 0; o < recipe.Ingredients.Count; o++)
                    {
                        var ingredient2 = recipe.Ingredients[o];
                        
                        if (other == ingredient2)
                        {
                            return CheckTemperaturesOf(target, other, recipe, t, o);
                        }
                    }
                }
            }
        }
        
        return false;
        */
    }

    private bool CheckTemperaturesOf(Ingredient target, Ingredient other, Recipe recipe, int targetIndex, int otherIndex)
    {
        if (target.Temperature != recipe.Temperatures[targetIndex]) return false;
        return other.Temperature == recipe.Temperatures[otherIndex];
    }

    private bool IsCrossingEnable(Ingredient target, Ingredient other)
    {
        if (!other.gameObject.activeSelf) return false;
        return CrossingDistance(target.transform, other.transform);
    }

    private bool CrossingDistance(Transform target, Transform other)
    {
        return Vector3.Distance(target.position, other.position) < maxCrossingDistance;
    }
}
