using UnityEngine;
using Valve.VR;

public struct ClickedEventArgs1
{
    public uint controllerIndex;
    public uint flags;
    public float padX, padY;
}

public delegate void ClickedEventHandler1(object sender, ClickedEventArgs1 e);

public class SteamVR_TrackedController1 : MonoBehaviour
{
    public uint controllerIndex;
    public VRControllerState_t controllerState;
    public bool triggerPressed = false;
    public bool steamPressed = false;
    public bool menuPressed = false;
    public bool padPressed = false;
    public bool padTouched = false;
    public bool gripped = false;

    public Vector2 padPosition = Vector2.zero;

    public event ClickedEventHandler1 MenuButtonClicked;
    public event ClickedEventHandler1 MenuButtonUnclicked;
    public event ClickedEventHandler1 TriggerClicked;
    public event ClickedEventHandler1 TriggerUnclicked;
    public event ClickedEventHandler1 SteamClicked;
    public event ClickedEventHandler1 PadClicked;
    public event ClickedEventHandler1 PadUnclicked;
    public event ClickedEventHandler1 PadTouched;
    public event ClickedEventHandler1 PadUntouched;
    public event ClickedEventHandler1 Gripped;
    public event ClickedEventHandler1 Ungripped;

    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<SteamVR_TrackedObject>() == null)
        {
            gameObject.AddComponent<SteamVR_TrackedObject>();
        }

		if (controllerIndex != 0)
		{
			this.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)controllerIndex;
			if (this.GetComponent<SteamVR_RenderModel>() != null)
			{
				this.GetComponent<SteamVR_RenderModel>().index = (SteamVR_TrackedObject.EIndex)controllerIndex;
			}
		}
		else
		{
			controllerIndex = (uint) this.GetComponent<SteamVR_TrackedObject>().index;
        }
    }

	public void SetDeviceIndex(int index)
	{
			this.controllerIndex = (uint) index;
	}

	public virtual void OnTriggerClicked(ClickedEventArgs1 e)
    {
        if (TriggerClicked != null)
            TriggerClicked(this, e);
    }

    public virtual void OnTriggerUnclicked(ClickedEventArgs1 e)
    {
        if (TriggerUnclicked != null)
            TriggerUnclicked(this, e);
    }

    public virtual void OnMenuClicked(ClickedEventArgs1 e)
    {
        if (MenuButtonClicked != null)
            MenuButtonClicked(this, e);
    }

    public virtual void OnMenuUnclicked(ClickedEventArgs1 e)
    {
        if (MenuButtonUnclicked != null)
            MenuButtonUnclicked(this, e);
    }

    public virtual void OnSteamClicked(ClickedEventArgs1 e)
    {
        if (SteamClicked != null)
            SteamClicked(this, e);
    }

    public virtual void OnPadClicked(ClickedEventArgs1 e)
    {
        if (PadClicked != null)
            PadClicked(this, e);
    }

    public virtual void OnPadUnclicked(ClickedEventArgs1 e)
    {
        if (PadUnclicked != null)
            PadUnclicked(this, e);
    }

    public virtual void OnPadTouched(ClickedEventArgs1 e)
    {
        if (PadTouched != null)
            PadTouched(this, e);
    }

    public virtual void OnPadUntouched(ClickedEventArgs1 e)
    {
        if (PadUntouched != null)
            PadUntouched(this, e);
    }

    public virtual void OnGripped(ClickedEventArgs1 e)
    {
        if (Gripped != null)
            Gripped(this, e);
    }

    public virtual void OnUngripped(ClickedEventArgs1 e)
    {
        if (Ungripped != null)
            Ungripped(this, e);
    }

    // Update is called once per frame
    void Update()
    {
		var system = OpenVR.System;
		if (system != null && system.GetControllerState(controllerIndex, ref controllerState))
		{
			ulong trigger = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Trigger));
            if (trigger > 0L && !triggerPressed)
            {
                triggerPressed = true;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnTriggerClicked(e);

            }
            else if (trigger == 0L && triggerPressed)
            {
                triggerPressed = false;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnTriggerUnclicked(e);
            }

            ulong grip = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_Grip));
            if (grip > 0L && !gripped)
            {
                gripped = true;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnGripped(e);

            }
            else if (grip == 0L && gripped)
            {
                gripped = false;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnUngripped(e);
            }

            ulong pad = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !padPressed)
            {
                padPressed = true;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnPadClicked(e);
            }
            else if (pad == 0L && padPressed)
            {
                padPressed = false;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnPadUnclicked(e);
            }

            ulong menu = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_ApplicationMenu));
            if (menu > 0L && !menuPressed)
            {
                menuPressed = true;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnMenuClicked(e);
            }
            else if (menu == 0L && menuPressed)
            {
                menuPressed = false;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnMenuUnclicked(e);
            }
            pad = controllerState.ulButtonTouched & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !padTouched)
            {
                padTouched = true;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnPadTouched(e);

            }
            else if (pad == 0L && padTouched)
            {
                padTouched = false;
                ClickedEventArgs1 e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                padPosition.x = e.padX;
                padPosition.y = e.padY;
                OnPadUntouched(e);
            }
        }
    }
}
