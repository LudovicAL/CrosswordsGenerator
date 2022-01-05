using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Definisseur : MonoBehaviour {

	public GameObject panel;
	public Text text;

	public static Definisseur Instance { get; private set; }

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
	}

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
		string cheminRapport = Application.persistentDataPath + "/rapport.txt";
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
				if (definition == "Définition indéterminée") {
					using (StreamWriter sw = File.AppendText(cheminRapport)) {
						sw.WriteLine(mot.Contenu);
					}
				}
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
				if (definition == "Définition indéterminée") {
					using (StreamWriter sw = File.AppendText(cheminRapport)) {
						sw.WriteLine(mot.Contenu);
					}
				}
			}
			text.text += definition;
			positionPrimairePrecedente = mot.PositionPrimaire;
			dejaEcritSurCetteLigne = true;
		}
	}
}
