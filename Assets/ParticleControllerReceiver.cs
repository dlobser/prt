using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class ParticleControllerReceiver : GlobalControllerReceiver, IGlobalTriggerPressSetHandler {


    void OnEnable() {
        base.OnEnable();
    }

    void OnDisable() {
        base.OnDisable();
    }


    public void OnGlobalTriggerPressDown(ViveEventData eventData) {
        if (eventData.deviceType.Equals(this.deviceType)) {
            //DO SOMETHING
        }
    }

    public void OnGlobalTriggerPress(ViveEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnGlobalTriggerPressUp(ViveEventData eventData) {
        throw new System.NotImplementedException();
    }
}
