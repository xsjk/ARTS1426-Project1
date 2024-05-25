using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Unity.VisualScripting;
using UnityEngine;

public class ColideTriggerLogic : MonoBehaviour
{

    public Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.0745f);
    public float animationDuration = 0.5f;

    [HideInInspector]
    public GradientController controller;

    private float angle;
    private GameObject current_card;
    private List<GameObject> cards = new List<GameObject>();
    void Start() {
        angle = Mathf.Atan2(transform.localPosition.z, transform.localPosition.x);
        // round angle to nearest 45 degree
        angle = Mathf.Round((angle - Mathf.PI / 8) / Mathf.PI * 4) * Mathf.PI / 4 + Mathf.PI / 8;
    }

    private void OnTriggerEnter(Collider other) {
        var card = other.gameObject;
        if (card.tag != "Card") return;

        var cardImageInfo = card.GetComponent<CardImageInfo>();
        if (current_card == null && cardImageInfo.current_slot == null) {
            Enter(card);
            cardImageInfo.Enter(gameObject);
        }
        cardImageInfo.slots.Add(gameObject);
        cards.Add(card);
        
    }

    private void OnTriggerExit(Collider other) {
        var card = other.gameObject;
        if (card.tag != "Card") return;
        var cardImageInfo = card.GetComponent<CardImageInfo>();
        cards.Remove(card);
        cardImageInfo.slots.Remove(gameObject);
        if (current_card == card && cardImageInfo.current_slot == gameObject) {
            Exit(card);
            cardImageInfo.Exit(gameObject);
        }
    }

    private Transform oldTransform;
    private GameObject grapsedObject;
    public void grasp(GameObject obj) {
        GetComponent<Animator>().SetBool("grasped", true);
        oldTransform = obj.transform.parent;
        grapsedObject = obj;
        grapsedObject.transform.parent = transform.GetChild(0);
    }

    public void release() {
        GetComponent<Animator>().SetBool("grasped", false);
        grapsedObject.transform.parent = oldTransform;
        grapsedObject = null;
        oldTransform = null;
    }
    
    public void Enter(in GameObject card) {

        current_card = card;

        Color color = card.GetComponent<CardImageInfo>().color;
        // controller.Add(angle, color);
        color.a = defaultColor.a;

        // var selfcycle = transform.GetChild(0).gameObject;
        // var selfsphere = selfcycle.transform.GetChild(2).gameObject;
        // var material1 = selfsphere.GetComponent<Renderer>().materials[1];
        // material1.color = color;

        var cardsphere = card.transform.GetChild(1).GetChild(2).gameObject;

        grasp(cardsphere);
    }

    public void Exit(in GameObject card) {

        // var selfcycle = transform.GetChild(0).gameObject;
        // var selfsphere = selfcycle.transform.GetChild(2).gameObject;
        // var material1 = selfsphere.GetComponent<Renderer>().materials[1];
        // material1.color = defaultColor;

        // controller.Remove(angle);

        release();

        current_card = null;

    }
}
