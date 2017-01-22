using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPanelController : MonoBehaviour {
    public UnitPanel panelPrefab;
    public Unit[] units;

    private List<UnitPanel> panels = new List<UnitPanel>();

	void Start () {
        foreach (Transform child in this.transform) {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < units.Length; i++) {
            UnitPanel newPanel = Instantiate<UnitPanel>(panelPrefab, this.transform);
            newPanel.name = units[i].name;
            newPanel.nameText.text = units[i].name;
            panels.Add(newPanel);
        }
    }

    void Update () {
        for (int i = 0; i < units.Length; i++) {
            panels[i].sanitySlider.value = units[i].GetStressLevel();
        }
	}
}
