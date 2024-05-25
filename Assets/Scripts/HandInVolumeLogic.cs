using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.VisualScripting;

public class HandInVolumeLogic : MonoBehaviour, IMixedRealityTouchHandler
{
    private DefaultObserverEventHandler observerEventHandler {
        get { return this.gameObject.transform.parent.GetComponent<DefaultObserverEventHandler>(); }
    }

    private void handInAction() {
        GetComponent<AudioSource>().mute = false;
        this.observerEventHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked;
    }
    private void handOutAction() {
        GetComponent<AudioSource>().mute = true;
        this.observerEventHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked;
    }
    void Start() {
        handOutAction();
    }
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        handInAction();
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) {
        handOutAction();
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) {
        handInAction();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    }

}
