using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelController : MonoBehaviour {
    public Image portrait;
    public Text nameText;
    public Slider sanitySlider;

    public Text engineText;
    public Toggle engineToggle;
    public CanvasGroup engineToggleGroup;

    public EngineStationController engineController;
    public Ship ship;

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

        ship.onStationEntered.AddListener(OnStationEntered);
        ship.onStationExited.AddListener(OnStationExited);
    }

    void Update () {
        if (selectedUnit == null) {
            return;
        }

        nameText.text = selectedUnit.name;

        if (selectedUnit.IsPanicked()) {
            sanitySlider.value = 0.0f;
        } else {
            sanitySlider.value = 1.0f - selectedUnit.GetStressLevel();
        }

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

    void engineToggleChanged(bool newValue) {
        engineController.SetEngineOn(engineToggle.isOn);
    }

    void OnStationEntered(Unit unit, TileType type) {
        if (type == TileType.Engine && unit == selectedUnit) {
            engineText.text = "ENGINE";
            engineToggle.isOn = engineController.IsEngineOn();
            DisplayToggle();
        }
    }

    void OnStationExited(Unit unit, TileType type) {
        if (type == TileType.Engine && unit == selectedUnit) {
            HideToggle();
        }
    }

    void setSelection(Unit unit) {
        if (unit == null) {
            group.alpha = 0.0f;
        } else {
            group.alpha = 1.0f;
        }
        selectedUnit = unit;

        TileType type = unit.GetComponent<Pathfinder>().level.TileAtWorldPosition(unit.transform.position).tileType;
        if (type == TileType.Engine) {
            engineText.text = "ENGINE";
            engineToggle.isOn = engineController.IsEngineOn();
            DisplayToggle();
        } else {
            HideToggle();
        }
    }

    void DisplayToggle() {
        engineToggleGroup.alpha = 1.0f;
        engineToggle.onValueChanged.AddListener(engineToggleChanged);
    }

    void HideToggle() {
        engineToggleGroup.alpha = 0.0f;
        engineToggle.onValueChanged.RemoveListener(engineToggleChanged);
    }
}
