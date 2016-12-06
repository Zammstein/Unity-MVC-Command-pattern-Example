using UnityEngine;
/// <summary>
/// TestController
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// This is an example implementation of a controller class
/// In this example this controller will listen to commands from the test view.
/// Typically, the function called when a command is recieved adjusts a model. After that the update event of the model is triggered to let all views (that listen to the model) know an update has been made.
/// </summary>
public class TestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.StartListening(TestEvents.ADD_ONE, OnTestEventTriggered);
	}

    void OnTestEventTriggered(object[] args) {
        TestModel tm = (TestModel)ModelManager.GetModel(TestModel.ID);
        tm.SetAmount(tm.GetAmount() + (int) args[0]);

        EventManager.TriggerEvent(TestModel.UPDATE_EVENT, tm);
    }

    // Update is called once per frame
    void OnDestroy () {
        EventManager.StopListening(TestEvents.ADD_ONE, OnTestEventTriggered);
    }
}
