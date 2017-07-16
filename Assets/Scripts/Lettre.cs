using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettre {
	public string valeur;
	private int x;
	private int y;
	private Mot motHorizontal;
	private Mot motVertical;
	private GameObject go;

	public Lettre (string valeur, int x, int y) {
		this.valeur = valeur;
		this.x = x;
		this.y = y;
		this.motHorizontal = null;
		this.motVertical = null;
		this.go = null;
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

	public GameObject Go {
		get {
			return this.go;
		}
		set {
			this.go = value;
		}
	}

	public Lettre SuivanteHorizontale {
		get {
			int index = motHorizontal.ListeLettres.IndexOf (this);
			if (index < (motHorizontal.Taille - 1)) {
				return motHorizontal.ListeLettres [index + 1];
			} else {
				return motHorizontal.Suivant.ListeLettres [0];
			}
		}
	}

	public Lettre PrecedenteHorizontale {
		get {
			int index = motHorizontal.ListeLettres.IndexOf (this);
			if (index > 0) {
				return motHorizontal.ListeLettres [index - 1];
			} else {
				return motHorizontal.Precedent.ListeLettres [motHorizontal.Precedent.Taille - 1];
			}
		}
	}

	public Lettre SuivanteVerticale {
		get {
			int index = motVertical.ListeLettres.IndexOf (this);
			if (index < (motVertical.Taille - 1)) {
				return motVertical.ListeLettres [index + 1];
			} else {
				return motVertical.Suivant.ListeLettres [0];
			}
		}
	}

	public Lettre PrecedenteVerticale {
		get {
			int index = motVertical.ListeLettres.IndexOf (this);
			if (index > 0) {
				return motVertical.ListeLettres [index - 1];
			} else {
				return motVertical.Precedent.ListeLettres [motVertical.Precedent.Taille - 1];
			}
		}
	}
}