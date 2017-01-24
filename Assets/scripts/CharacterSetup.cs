using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats {
    public string name = "Guy";
    public float steering = 1.0f;
    public float sonar = 1.0f;
    public float repair = 1.0f;
    public float cooking = 1.0f;
    public float speed = 1.0f;
    public Sprite portrait;
    public Sprite[] idleAnim;
    public Sprite[] walkAnim;
    public Sprite[] repairAnim;
    public Sprite[] panicAnim;
    public Sprite[] climbAnim;
}

public class CharacterSetup : MonoBehaviour {
    public CharacterStats[] characterStats;

	// Use this for initialization
	void Start () {
        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            Pathfinder pathfinder = unit.GetComponent<Pathfinder>();
            UnitStats unitStats = unit.GetComponent<UnitStats>();
            AnimateObject animation = unit.GetComponent<AnimateObject>();
            int charId = GameStartData.playingCharacterIDs[pathfinder.unitId];
            CharacterStats stats = characterStats[charId];

            unit.name = stats.name;
            unitStats.Steering = stats.steering;
            unitStats.Sonar = stats.sonar;
            unitStats.Repair = stats.repair;
            unitStats.Cooking = stats.cooking;
            pathfinder.moveSpeed *= stats.speed;
            unitStats.portrait = stats.portrait;

            animation.idleAnim = stats.idleAnim;
            animation.walkAnim = stats.walkAnim;
            animation.repairAnim = stats.repairAnim;
            animation.panicAnim = stats.panicAnim;
            animation.climbAnim = stats.climbAnim;
        }
    }
}
