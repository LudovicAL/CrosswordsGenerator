using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LaunchManager))]
public class Selecteur : MonoBehaviour {

	bool horizontalMode;
	Grille grille;
	Lettre lettreActuelle;

	// Use this for initialization
	void Start () {
		horizontalMode = true;
		grille = gameObject.GetComponent<LaunchManager> ().grille;
		MajLettreActuelle(grille.listeLettres [0, 0]);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Horizontal")) {
			horizontalMode = true;
			if (Input.GetAxisRaw("Horizontal") > 0) {
				MajLettreActuelle(lettreActuelle.Suivante(true));
			} else {
				MajLettreActuelle(lettreActuelle.Precedente(true));
			}
		}
		if (Input.GetButtonDown("Vertical")) {
			horizontalMode = false;
			if (Input.GetAxisRaw("Vertical") < 0) {
				MajLettreActuelle(lettreActuelle.Suivante(false));
			} else {
				MajLettreActuelle(lettreActuelle.Precedente(false));
			}
		}
	}

	private void MajLettreActuelle(Lettre nouvelleLettre) {
		if (lettreActuelle != null) {
			blanchirMot (lettreActuelle);
		}
		surlignerMot (nouvelleLettre);
		lettreActuelle = nouvelleLettre;
		transform.position = lettreActuelle.Go.transform.position + Vector3.back;
	}

	private void surlignerMot(Lettre lettre) {
		if (horizontalMode) {
			foreach (Lettre l in lettre.MotHorizontal.ListeLettres) {
				l.GoRenderer.color = Color.cyan;
			}
		} else {
			foreach (Lettre l in lettre.MotVertical.ListeLettres) {
				l.GoRenderer.color = Color.cyan;
			}
		}
	}

	private void blanchirMot(Lettre lettre) {
		foreach (Lettre l in lettre.MotHorizontal.ListeLettres) {
			l.GoRenderer.color = Color.white;
		}
		foreach (Lettre l in lettre.MotVertical.ListeLettres) {
			l.GoRenderer.color = Color.white;
		}
	}
}
