﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds all models. At the start of the game the Init function should be called.
/// When creating a new persistant model, you should add it to the dictinary in the Init function.
/// </summary>
public class ModelManager : MonoBehaviour {

    private Dictionary<string, object> modelDictionary;

	private static ModelManager manager;

    public static ModelManager instance {
        get {
            if (!manager) {
                manager = FindObjectOfType(typeof(ModelManager)) as ModelManager;

                if (!manager) {
                    Debug.LogError("There needs to be one active ModelManager script on a GameObject in your scene.");
                } else {
                    manager.Init();
                }
            }

            return manager;
        }
    }

    void Init() {
        if (modelDictionary == null)
            modelDictionary = new Dictionary<string, object>();

        // Add models here that need to be persistant
        modelDictionary.Add(TestModel.ID, new TestModel());
    }

    public static object GetModel(string ID) {
        return instance.modelDictionary[ID];
    }
}