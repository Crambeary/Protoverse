using UnityEngine;
using System.Collections;

public class CameraBumper : MonoBehaviour {
	[System.Flags]
	public enum BumperDirection {
		None = 0,
		HorizontalPositive = 1,
		HorizontalNegative = 2,
		VerticalPositive = 4,
		VerticalNegative = 8,
		Horizontal = HorizontalPositive | HorizontalNegative,
		Vertical = VerticalPositive | VerticalNegative,
		AllDirections = Horizontal | Vertical,
	}

	public BumperDirection blockDirection = BumperDirection.AllDirections;
}
