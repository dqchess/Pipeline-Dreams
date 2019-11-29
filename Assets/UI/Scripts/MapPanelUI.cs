﻿using UnityEngine;
internal class MapPanelUI : MonoBehaviour {

    bool visible;
    [SerializeField] GameObject Panel;

    public void HideDialogue() {

        visible = false;


        Panel.SetActive(false);
    }
    public void ShowDialogue() {

        visible = true;


        Panel.SetActive(visible);

    }
}