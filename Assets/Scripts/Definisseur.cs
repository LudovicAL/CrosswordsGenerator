using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Definisseur : MonoBehaviour {

	public GameObject panel;
	public Text text;

	void Start() {

	}

	void Update() {

	}

	/// <summary>
	/// Efface toutes les définitions
	/// </summary>
	public void ViderDefinitions() {
		text.text = "";
	}

	/// <summary>
	/// Affiche toutes les définitions
	/// </summary>
	/// <param name="grille"></param>
	/// <param name="bd"></param>
	public void AfficherDefinitions(Grille grille, Bd bd) {
		panel.SetActive(true);
		text.text = "";
		int positionPrimairePrecedente = int.MinValue;
		bool dejaEcritSurCetteLigne = false;
		text.text += "HORIZONTAL";
		foreach (Mot mot in grille.listeMotsHorizontaux) {
			if (mot.PositionPrimaire != positionPrimairePrecedente) {
				dejaEcritSurCetteLigne = false;
				text.text += "\n";
				text.text += mot.PositionPrimaire;
				text.text += ": ";
			} else {
				if (dejaEcritSurCetteLigne) {
					text.text += "; ";
				}
			}
			string definition = "";
			if (mot.Rempli) {
				string[] definitions = mot.ObtenirDefinitions(bd);
				definition = definitions[Random.Range(0, definitions.Length)];
			}
			text.text += definition;
			positionPrimairePrecedente = mot.PositionPrimaire;
			dejaEcritSurCetteLigne = true;
		}
		positionPrimairePrecedente = int.MinValue;
		dejaEcritSurCetteLigne = false;
		text.text += "\n\nVERTICAL";
		foreach (Mot mot in grille.listeMotsVerticaux) {
			if (mot.PositionPrimaire != positionPrimairePrecedente) {
				dejaEcritSurCetteLigne = false;
				text.text += "\n";
				text.text += mot.PositionPrimaire;
				text.text += ": ";
			} else {
				if (dejaEcritSurCetteLigne) {
					text.text += "; ";
				}
			}
			string definition = "";
			if (mot.Rempli) {
				string[] definitions = mot.ObtenirDefinitions(bd);
				definition = definitions[Random.Range(0, definitions.Length)];
			}
			text.text += definition;
			positionPrimairePrecedente = mot.PositionPrimaire;
			dejaEcritSurCetteLigne = true;
		}
	}
}
