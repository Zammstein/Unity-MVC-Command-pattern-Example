using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// SaveLoadController
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// This singleton class holds all persistant save models.
/// When creating a new persistant model, you should add it to the dictinary in the Init function.
/// </summary>
public class SaveLoadController : MonoBehaviour {

    private static Dictionary<string, SaveModel> modelDictionary; // !< This dictionary has all saveable models.
    private BinaryFormatter formatter = new BinaryFormatter(); // !< A binary formatter is used to read / write save files.
    private static SaveLoadController manager;

    public static SaveLoadController instance {
        get {
            if (!manager) {
                manager = FindObjectOfType(typeof(SaveLoadController)) as SaveLoadController;

                if (!manager)
                    Debug.LogError("There needs to be one active SaveLoadController script on a GameObject in your scene.");
                else
                    manager.Init();
            }
            return manager;
        }
    }

    /// <summary>
    /// Add all models to the dictionary here so they will be saved / loaded by this controller.
    /// </summary>
    Dictionary<string, SaveModel> CreateSaveModelDictionary() {
        Dictionary<string, SaveModel> dic = new Dictionary<string, SaveModel>();

        // Add models below to the dictionary to save / load them
        // e.g. :
        //dic.Add(KeyBindingModel.ID, new KeyBindingModel());
        //dic.Add(OptionsModel.ID, new OptionsModel());

        return dic;
    }

    void Init() {
        if (modelDictionary == null) {
            modelDictionary = CreateSaveModelDictionary();

            // If a save directory exists try to load save files and overwrite 
            if (Directory.Exists("Saves")) {
                LoadData();
            }
        }
    }

    public object GetModel(string ID) {
        return modelDictionary[ID];
    }

    /// <summary>
    /// This function will try to retrieve all save files corresponding to their save model.
    /// Once a save is deserialized, file versions are being checked and upgrades will be triggered.
    /// </summary>
    void LoadData() {
        List<string> keys = new List<string>(modelDictionary.Keys);
        foreach (string key in keys) {
            if (File.Exists("Saves/" + key + ".binary")) {
                try {
                    // retrieve the saved model from a binary file.
                    FileStream fileStream = File.Open("Saves/" + key + ".binary", FileMode.Open);
                    SaveModel saveModel = (SaveModel)formatter.Deserialize(fileStream);

                    // Compare versions
                    if (saveModel.GetVersion() < modelDictionary[key].GetVersion())
                        modelDictionary[key].UpgradeModel(saveModel);
                    else
                        modelDictionary[key] = saveModel;

                    fileStream.Close();
                } catch (Exception e) {
                    Debug.LogError("Somthing went wrong trying to read save file: " + key);
                    Debug.LogError(e.StackTrace);
                }
            }
        }
    }

    /// <summary>
    /// Saves the game data from the models in the modelDictionary
    /// </summary>
    public void SaveData() {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        //Loop the dictionary and save each model
        foreach (KeyValuePair<string, SaveModel> entry in modelDictionary) {
            string path = "Saves/" + entry.Key + ".binary";
            FileStream saveFile;
            if (!File.Exists(path)) {
                saveFile = File.Create(path);
            } else {
                saveFile = File.Open(path, FileMode.Open);
            }

            formatter.Serialize(saveFile, entry.Value);

            saveFile.Close();
        }

    }
}