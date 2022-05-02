using System.Collections;
using UnityEngine;

public class CrystalsHolder : MonoBehaviour
{
    [SerializeField] private Crystal[] _crystals;

    internal Crystal[] GetList()
    {
        return _crystals;
    }

    private void Start()
    {
        StartCoroutine(ActivateCrystals());
    }

    IEnumerator ActivateCrystals()
    {
        while (true)
        {
            foreach (var crystal in _crystals)
            {
                if (crystal.gameObject.activeSelf)
                {
                    crystal.SetIngredientsTemperature();
                }
                
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
