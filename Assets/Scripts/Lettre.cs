using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettre {
	public string valeur;
	private int x;
	private int y;
	private Mot motHorizontal;
	private Mot motVertical;

	public Lettre (string valeur, int x, int y) {
		this.valeur = valeur;
		this.x = x;
		this.y = y;
		this.motHorizontal = null;
		this.motVertical = null;
	}

	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public Mot MotHorizontal {
		get {
			return this.motHorizontal;
		}
		set {
			this.motHorizontal = value;
		}
	}

	public Mot MotVertical {
		get {
			return this.motVertical;
		}
		set {
			this.motVertical = value;
		}
	}
}