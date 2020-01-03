using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LaunchManager))]
public class Selecteur : MonoBehaviour {

	bool horizontalMode;
	Lettre lettreActuelle;
	private LaunchManager launchManager;

	private void Awake() {
		launchManager = gameObject.GetComponent<LaunchManager>();
	}

	void Start () {
		Initialiser();
	}
	
	void Update () {
		if (launchManager.grille != null) {
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
	}

	public void Initialiser() {
		horizontalMode = true;
		if (launchManager == null) {
			launchManager = gameObject.GetComponent<LaunchManager>();
		}
		MajLettreActuelle(launchManager.grille.listeLettres[0, 0]);
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
			if (lettre.MotHorizontal != null) {
				foreach (Lettre l in lettre.MotHorizontal.ListeLettres) {
					l.GoRenderer.color = Color.cyan;
				}
			}
		} else {
			if (lettre.MotVertical != null) {
				foreach (Lettre l in lettre.MotVertical.ListeLettres) {
					l.GoRenderer.color = Color.cyan;
				}
			}
		}
	}

	private void blanchirMot(Lettre lettre) {
		if (lettre.MotHorizontal != null) {
			foreach (Lettre l in lettre.MotHorizontal.ListeLettres) {
				l.GoRenderer.color = Color.white;
			}
		}
		if (lettre.MotVertical != null) {
			foreach (Lettre l in lettre.MotVertical.ListeLettres) {
				l.GoRenderer.color = Color.white;
			}
		}
	}
}
