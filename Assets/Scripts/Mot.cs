using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mot {
	private List<Lettre> listeLettres;
	private int taille;
	private Mot precedent;
	private Mot suivant;

	public Mot () {
		this.listeLettres = new List<Lettre> ();
		this.taille = 0;
		this.precedent = null;
		this.suivant = null;
	}

	public void AjouterLettre(Lettre l) {
		listeLettres.Add (l);
		taille = listeLettres.Count;
	}

	public List<Lettre> ListeLettres {
		get {
			return this.listeLettres;
		}
	}

	public int Taille {
		get {
			return this.taille;
		}
	}

	public Mot Precedent {
		get {
			return this.precedent;
		}
		set {
			this.precedent = value;
		}
	}

	public Mot Suivant {
		get {
			return this.suivant;
		}
		set {
			this.suivant = value;
		}
	}
}
