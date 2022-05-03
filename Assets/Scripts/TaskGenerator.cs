using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskGenerator : MonoBehaviour
{
    [SerializeField] private float _timeForRecipe = 180f; // 3 minutes
    [SerializeField] Text _taskLabel;
    private List<Recipe> _book;
    internal int CurrentRecipe { get; set; }
    
    private void Start()
    {
        CurrentRecipe = 0;
        _book = FindObjectOfType<RecipesBook>().Book;
        StartCoroutine(CreateTask());
    }

    IEnumerator CreateTask()
    {
        while (true)
        {
            string text = "";
            var recipe = _book[CurrentRecipe];
            var ingredientsCount = recipe.Ingredients.Count;
            for (int i = 0; i < ingredientsCount; i++)
            {
                string temperature = recipe.Temperatures[i] switch
                {
                    0 => "Cold ",
                    50 => " Hot ",
                    _ => ""
                };

                text += temperature + recipe.Ingredients[i].name;

                if (i == ingredientsCount - 1) continue;
                text += " + ";
            }

            _taskLabel.text = text;
            SetNextIndex();
            yield return new WaitForSeconds(_timeForRecipe);
        }
    }

    private void SetNextIndex()
    {
        if (CurrentRecipe == _book.Count - 1)
        {
            CurrentRecipe = 0;
            return;
        }

        CurrentRecipe++;
    }
}
