using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardImageInfo : MonoBehaviour
{
    public Color color;
    public GameObject current_slot;
    public List<GameObject> slots = new List<GameObject>();
    

    public float volumeTransitionSpeed = 5;
    private float volume = 0;
    private AudioSource audioSource;
    private DefaultObserverEventHandler observerEventHandler;
    public PlayerCountLogic playerCountLogic;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        observerEventHandler = transform.parent.GetComponent<DefaultObserverEventHandler>();
        if (playerCountLogic == null) 
            Debug.LogError("Base object is not set for " + gameObject.name);
    }

    private void Update() {
        audioSource.volume = Mathf.Lerp(audioSource.volume, volume, volumeTransitionSpeed * Time.deltaTime);
    }

    private void Activate() {
        volume = 1;
        playerCountLogic.playerCount++;
        // observerEventHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked;
    }

    private void Deactivate() {
        volume = 0;
        playerCountLogic.playerCount--;
        // observerEventHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked;
    }


    public void Exit(in GameObject slot) {
        if (slots.Count == 0) {
            Deactivate();
            current_slot = null;
        }
        else {
            current_slot = slots[0];
            current_slot.GetComponent<ColideTriggerLogic>().Enter(gameObject);
        }
        
    }
    public void Enter(in GameObject slot) {
        current_slot = slot;
        Activate();
    }


}