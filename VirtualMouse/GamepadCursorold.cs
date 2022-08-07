using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;

//Actionmap
/// <summary>
/// Custom gamepad cursor for the Input System. Pipes gamepad/controller input into a Virtual Mouse,
/// which is used to inform the Event System of what the user is hovering over and clicking on.
/// 
/// 1) Make sure the cursor Rect Transform pivot is at the center 0.5, 0.5.
/// 2) Make sure the Cursor is the last element on the canvas so it will draw over the other items.
/// 3) In the cursor, disable the Raycast Target on the Image so it cannot be accidentally selected.
/// </summary>
public class GamepadCursorold : MonoBehaviour
{
    public Vector3 testoutput1;
    public Vector3 testoutput2;
    public Vector3 testoutput3;

    public bool _gamepadButtonDown;
    public bool _ignoreInputChange;
    
    
    public PlayerInput playerInput;
    
    public RectTransform cursorTransform;
    
    public Canvas canvas;
    
    public RectTransform canvasRectTransform;
    
    private float cursorSpeed = 1000f;
    
    private float padding = 50f;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Camera mainCamera;
// .UI
    private string previousControlScheme = "";
    private const string gamepadScheme = "Gamepad";
//    private const string mouseScheme = "Keyboard&Mouse";
    private const string mouseScheme = "Keyboard";

    /// <summary>
    /// Called when the script is enabled.
    /// Get reference to camera and current system mouse.
    /// Create a virtual mouse and add it to the Input System, and pair the new device with the PlayerInput component.
    /// Subscribe to the update events.
    /// </summary>
    private void OnEnable() {

        Debug.Log($"<color=red> OnEnable </color>");
        
        mainCamera = Camera.main;
        currentMouse = Mouse.current;
        _gamepadButtonDown = false;

        if (virtualMouse == null)
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        
        
        
        
        // Pair the device to the user to use the PlayerInput component with the Event System & the Virtual Mouse.
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        
        // Set the initial cursor position.
        if (cursorTransform != null) {
            Debug.Log($"<color=red>Cursor transform ok</color>");
            Vector2 position = cursorTransform.anchoredPosition;    
            InputState.Change(virtualMouse.position, position);
        }    

        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
        
        
        
        Debug.Log($"<color=red> Setup </color>");
    }

    /// <summary>
    /// Called when the script is disabled.
    /// Remove the virtual mouse from the Input System.
    /// Unsubscribe from the update events.
    /// </summary>
    private void OnDisable() {
        if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
        Debug.Log($"<color=red> Disabled </color>");
    }

    /// <summary>
    /// Called on every frame after the Input System is updated.
    /// Gets the delta value of the joystick and adds padding and clamps it to the edges of the screen.
    /// Changes the Virtual Mouse state of the Input System because of the gamepad input.
    /// </summary>
    private void UpdateMotion() {
        if (virtualMouse == null || Gamepad.current == null) {
            Debug.Log($"<color=red>Either virtual or gamepad = null </color>");
            return;
        }
        
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.unscaledDeltaTime;
        
        Debug.Log($"<color=red>deltavalue </color>");

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        // Clamp the cursor to the screen size.
        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);
        
        

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();

        if ((_gamepadButtonDown == true) &&(aButtonIsPressed==false))
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }
        
        _gamepadButtonDown = aButtonIsPressed;
        if (previousMouseState != aButtonIsPressed) {
            
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;

            if (aButtonIsPressed)
            {
                _ignoreInputChange = true;
            }
            
        }

        
        
        
        AnchorCursor(newPosition);
    }

    /// <summary>
    /// Converts screen space corrdinates to a local point in a RectTransform.
    /// Used to convert the cursor coordinates to match it's correct position in the canvas.
    /// </summary>
    /// <param name="position">Cursor position in screen space corrdinates.</param>
    private void AnchorCursor(Vector2 position) {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
        //cursorTransform.anchoredPosition = position;
    }

    /// <summary>
    /// Called when the player switches the active control in the Input System. Is called from the PlayerInput component.
    /// When the player switches from gamepad -> mouse, the gamepad cursor is disabled and the mouse is enabled. The mouse is moved to the position where the gamepad cursor was.
    /// When the player switches from mouse -> gamepad, the mouse is disabled, the gamepad cursor is enabled, and the gamepad cursor is moved to where the system mouse previously was.
    /// </summary>
    /// <param name="input">Current PlayerInput component.</param>
    private void OnControlsChanged(PlayerInput input) {
        //playerInput = PlayerInput.GetPlayerByIndex(0);
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme) {

            if (_ignoreInputChange)
            {
                previousControlScheme = gamepadScheme;
                return;
            }
            Debug.Log("Setting to mouse scheme from other scheme");
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme) {
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            Cursor.visible = false;
            
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
            //WarpCursor
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
        }
    }
}
