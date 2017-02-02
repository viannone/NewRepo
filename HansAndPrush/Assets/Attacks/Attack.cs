using UnityEngine;
using System.Collections;

public enum AttackColor {BLUE, RED, GREEN, PURPLE, WHITE, BLACK, BROWN, YELLOW};
public enum Transportation {PROJECTILE, LASER, INSTANT};
public enum Effect {SPLASH, DOT, SLOW, HEAL, CLEANSE, VAMPIRIC, ARMOR, AMPLIFY};
[System.Serializable]
public class Attack {
	public Transform prefab;
	public AttackColor attackColor;
	public int value;
	public CentralNervousSystem sender;
	public Effects[] effects;
}
[System.Serializable]
public struct Effects{
	public Effect effect;
	public float effectValue;
	public float effectTime;
}
