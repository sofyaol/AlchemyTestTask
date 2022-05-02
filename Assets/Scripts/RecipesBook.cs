using System.Collections.Generic;
using UnityEngine;

public class RecipesBook : MonoBehaviour
{
 [SerializeField] private List<Recipe> _book;
 internal List<Recipe> Book => _book;

 internal void Add(Recipe recipe)
 { 
  _book.Add(recipe);
 }

}
