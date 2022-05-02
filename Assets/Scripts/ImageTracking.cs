using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    private Ingredient[] _ingredients;
    private Crystal[] _crystals;
    
    private Dictionary<string, Ingredient> _ingredientsDictionary = new Dictionary<string, Ingredient>();
    private Dictionary<string, Crystal> _crystalDictionary = new Dictionary<string, Crystal>();
    private ARTrackedImageManager _imageManager;
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        _imageManager = GetComponent<ARTrackedImageManager>();
        
        _ingredients = FindObjectOfType<IngredientsHolder>().GetList();
        _crystals = FindObjectOfType<CrystalsHolder>().GetList();
        
        FillDictionary(_ingredients, _ingredientsDictionary);
        FillDictionary(_crystals, _crystalDictionary);
    }

    private void FillDictionary<T>(IEnumerable<T> collection, Dictionary<string,T> dictionary) where T : MonoBehaviour
    {
        foreach (var item in collection)
        {
            dictionary.Add(item.name, item);
            item.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _imageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        _imageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var item in eventArgs.added)
        {
            UpdateImage(item);
        }
        
        foreach (var item in eventArgs.updated)
        {
            if (item.trackingState == TrackingState.Tracking)
            {
                UpdateImage(item);
            }
            else
            {
                FindInDictionaries(item.referenceImage.name).gameObject.SetActive(false);
            }
        }
        
        foreach (var item in eventArgs.removed)
        {
            FindInDictionaries(item.referenceImage.name).gameObject.SetActive(false);
        }
    }

    private Tracked FindInDictionaries(string imageName)
    {
        if (_crystalDictionary.TryGetValue(imageName, out Crystal crystal)) return crystal;
        return _ingredientsDictionary.TryGetValue(imageName, out Ingredient ingredient) ? ingredient : null;
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string key = trackedImage.referenceImage.name;
        Tracked tracked = FindInDictionaries(key);
        tracked.transform.position = trackedImage.transform.position;
        tracked.transform.rotation = trackedImage.transform.rotation;
        tracked.gameObject.SetActive(true);
    }
}
