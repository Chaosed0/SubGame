using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelController : MonoBehaviour {
    public Image portrait;
    public Text nameText;

    private CanvasGroup group;
    private Unit selectedUnit;

	void Start () {
        group = this.GetComponent<CanvasGroup>();
        group.alpha = 0.0f;

        Pathfinder[] units = FindObjectsOfType<Pathfinder>();
        for (int i = 0; i < units.Length; i++) {
            Unit unit = units[i].GetComponent<Unit>();
            units[i].onSelected.AddListener(() => setSelection(unit));
        }
    }

    void Update () {
        if (selectedUnit == null) {
            return;
        }

        nameText.text = selectedUnit.name;

        if (selectedUnit.IsPanicked()) {
            portrait.sprite = selectedUnit.unitStats.panicPortrait;
        } else if (selectedUnit.GetStressLevel() <= 0.3f) {
            portrait.sprite = selectedUnit.unitStats.goodPortrait;
        } else if (selectedUnit.GetStressLevel() <= 0.6f) {
            portrait.sprite = selectedUnit.unitStats.mediumPortrait;
        } else if (selectedUnit.GetStressLevel() <= 1.0f) {
            portrait.sprite = selectedUnit.unitStats.badPortrait;
        }
	}

    void setSelection(Unit unit) {
        if (unit == null) {
            group.alpha = 0.0f;
        } else {
            group.alpha = 1.0f;
        }
        selectedUnit = unit;
    }
}
