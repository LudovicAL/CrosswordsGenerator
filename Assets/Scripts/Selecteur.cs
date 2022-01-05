using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecteur : MonoBehaviour {

	private bool horizontalMode;
	private Lettre lettreActuelle;

	public static Selecteur Instance { get; private set; }

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
	}

	void Start () {
		Initialiser();
	}
	
	void Update () {
		if (LaunchManager.Instance.grille != null) {
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
		MajLettreActuelle(LaunchManager.Instance.grille.listeLettres[0, 0]);
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
