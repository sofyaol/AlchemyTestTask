using UnityEngine;

public class IngredientsHolder : MonoBehaviour
{
    [SerializeField] private Ingredient[] _ingridientPrefabs;

    internal Ingredient[] GetList()
    {
        return _ingridientPrefabs;
    }
}
