using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour {

	public Text textIdGrilleVide;
	public Text textIdGrillePleine;


	private void Start() {
		majTextIdGrilles();
	}

	public void majTextIdGrilles() {
		textIdGrilleVide.text = LaunchManager.Instance.idGrilleVide.ToString();
		textIdGrillePleine.text = LaunchManager.Instance.idGrillePleine.ToString();
	}

	public void OnButtonRemplirGrilleX1() {
		LaunchManager.Instance.RemplirGrilleNbFois(1);
	}

	public void OnButtonRemplirGrilleX100() {
		LaunchManager.Instance.RemplirGrilleNbFois(100);
	}

	public void OnButtonRemplirGrilleX250() {
		LaunchManager.Instance.RemplirGrilleNbFois(250);
	}

	public void OnButtonRemplirGrilleX500() {
		LaunchManager.Instance.RemplirGrilleNbFois(500);
	}

	public void OnButtonRemplirGrilleX1000() {
		LaunchManager.Instance.RemplirGrilleNbFois(1000);
	}

	public void OnButtonGenererGrilleVide() {
		LaunchManager.Instance.GenererGrilleVide();
	}

	public void OnButtonAfficherDefinitions() {
		LaunchManager.Instance.AfficherDefinitions();
	}

	public void OnButtonModifierIdGrillePleine(int modificateur) {
		LaunchManager.Instance.ModifierIdGrillePleine(modificateur);
		majTextIdGrilles();
	}

	public void OnButtonChargerGrillePleine() {
		LaunchManager.Instance.ChargerGrille(true, LaunchManager.Instance.idGrillePleine);
	}

	public void OnButtonModifierIdGrilleVide(int modificateur) {
		LaunchManager.Instance.ModifierIdGrilleVide(modificateur);
		majTextIdGrilles();
	}

	public void OnButtonChargerGrilleVide() {
		LaunchManager.Instance.ChargerGrille(false, LaunchManager.Instance.idGrilleVide);
	}

	public void OnButtonSauvegarderGrillePleine() {
		LaunchManager.Instance.SauvegarderGrille(true);
	}

	public void OnButtonSauvegarderGrilleVide() {
		LaunchManager.Instance.SauvegarderGrille(false);
	}
}