/// <summary>
/// Models will only hold data in a structured way. 
/// Every persistant model must have an ID so that it can be indexed by the model manager.
/// The UPDATE_EVENT is used to let views know when a model has been updated.
/// Note that all ID- and UPDATE_EVENT constants must be unique per model.
/// </summary>
public class TestModel {

    public const string ID = "TESTMODEL";
    public const string UPDATE_EVENT = "TESTMODEL_UPDATED";

    private int amount;

    public TestModel() {}

    public int GetAmount() {
        return this.amount;
    }

    public void SetAmount(int value) {
        this.amount = value;
    }
}
