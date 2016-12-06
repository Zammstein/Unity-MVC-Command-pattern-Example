using System;
/// <summary>
/// SaveModel
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// Save Model abstract class must be inherited by models that are saved/loaded by the SaveLoadController.
/// </summary>
[Serializable]
public abstract class SaveModel {
    protected int version;

    public int GetVersion() { return version; } //!< Return the version of the save file. This can be compared with other versions of the save file. 
    public virtual void UpgradeModel(SaveModel oldVersion) { } //!< Upgrade the model using an older version.
}
