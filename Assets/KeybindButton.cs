using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    public int targetBinding;
    public bool allowMouseBindings;
    public InputActionReference inputAction;
    public Button bindButton;
    public TextMeshProUGUI displayText;

    void Start()
    {
        RefreshText();

        bindButton.onClick.AddListener(() => Rebind());
    }

    void Rebind()
    {
        displayText.text = "Waiting for input";

        playerInput.SwitchCurrentActionMap("Interface");

        var operation = inputAction.action.PerformInteractiveRebinding()
            .WithTargetBinding(targetBinding)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(OnComplete);

        if (!allowMouseBindings)
        {
            operation.WithControlsExcluding("Mouse");
        }

        operation.Start();
    }

    void OnComplete(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();

        RefreshText();

        playerInput.SwitchCurrentActionMap("World");
    }

    void RefreshText()
    {
        //int index = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);

        displayText.text = InputControlPath.ToHumanReadableString(inputAction.action.bindings[targetBinding].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
