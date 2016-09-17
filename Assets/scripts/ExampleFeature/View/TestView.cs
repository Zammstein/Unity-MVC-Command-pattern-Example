using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Views are scripts that must be attached to a gameobject in Unity.
/// You can trigger events from here that will adjust models.
/// You can also listen to update events from models when the view needs to be updated.
/// </summary>
public class TestView : MonoBehaviour {

    public Text textView;
    
	void Start () {
        EventManager.StartListening(TestModel.UPDATE_EVENT, OnTestModelUpdated);
	}

    private void OnTestModelUpdated(object arg0) {
        textView.text = ((TestModel) arg0).GetAmount().ToString();
    }

    void OnDestroy() {
        EventManager.StopListening(TestModel.UPDATE_EVENT, OnTestModelUpdated);
    }

    public void AddOne() {
        EventManager.TriggerEvent(TestEvents.ADD_ONE, 1);
    }
}
