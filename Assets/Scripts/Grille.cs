using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grille {
	public int nbLignes {get; private set;}
	public int nbColonnes {get; private set;}
	public Lettre[,] listeLettres;
	public List<Mot> listeMotsHorizontaux;
	public List<Mot> listeMotsVerticaux;
	public List<Mot> listeMots;
	public List<Mot> listeMotsARemplir;
	
	#region Création

	public Grille(string gridAsString) {
		listeMotsHorizontaux = new List<Mot> ();
		listeMotsVerticaux = new List<Mot> ();
		LireGridFile (gridAsString);
		TrouverMotsHorizontaux ();
		TrouverMotsVerticaux ();
		SupprimerMotsUneLettre (listeMotsHorizontaux);
		SupprimerMotsUneLettre (listeMotsVerticaux);
		TrouverMotsVoisins (listeMotsHorizontaux);
		TrouverMotsVoisins (listeMotsVerticaux);
		AttribuerPositionSecondaires(listeMotsHorizontaux);
		AttribuerPositionSecondaires(listeMotsVerticaux);
		listeMots = new List<Mot> ();
		listeMots.AddRange (listeMotsHorizontaux);
		listeMots.AddRange (listeMotsVerticaux);
		AjouterReferenceLettreAMots();
		CalculerScoresDeBase();
		CalculerScores();
		listeMotsARemplir = ClonerListeMots(listeMots);
		TrierListeMotsARemplirParScore();
	}

	#endregion Création

	#region Outils

	/// <summary>
	/// Effectue l'affichage de tous les mots de la grille
	/// </summary>
	public void AfficherMots() {
		foreach (Mot mot in listeMots) {
			mot.AfficherMot();
		}
	}

	#endregion Outils

	#region Initialisation

	//Retourne un clone (partiel) d'une liste de mot
	private List<Mot> ClonerListeMots(List<Mot> l) {
		List<Mot> clone = new List<Mot>();
		foreach (Mot mot in l) {
			clone.Add(mot);
		}
		return clone;
	}

	/// <summary>
	/// Pour chaque lettres de chaque mot, ajoute une référence dans l'objet lettre pointant vers l'objet mot
	/// </summary>
	private void AjouterReferenceLettreAMots() {
		foreach (Mot mot in listeMots) {
			foreach (Lettre lettre in mot.ListeLettres) {
				if (mot.Horizontal) {
					lettre.MotHorizontal = mot;
				} else {
					lettre.MotVertical = mot;
				}
			}
		}
	}

	/// <summary>
	/// Attribue à chaque mot sa position secondaire
	/// </summary>
	/// <param name="listeMots"></param>
	private void AttribuerPositionSecondaires(List<Mot> listeMots) {
		int index = 0;
		int i = 1;
		foreach (Mot mot in listeMots) {
			if (mot.PositionPrimaire != index) {
				index = mot.PositionPrimaire;
				i = 1;
			}
			mot.PositionSecondaire = i;
			i++;
		}
	}

	/// <summary>
	/// Trouve les références vers les mots suivants et précédents de chaque mot
	/// </summary>
	/// <param name="listeMots"></param>
	private void TrouverMotsVoisins(List<Mot> listeMots) {
		for (int i = 0, max = listeMots.Count; i < max; i++) {
			listeMots [i].Precedent = ((i - 1) >= 0) ? listeMots[i - 1] : listeMots[max - 1];
			listeMots [i].Suivant = ((i + 1) < max) ? listeMots[i + 1] : listeMots[0];
		}
	}

	/// <summary>
	/// Supprime tous les mots d'une seule lettre
	/// </summary>
	/// <param name="listeMots"></param>
	private void SupprimerMotsUneLettre(List<Mot> listeMots) {
		for (int i = listeMots.Count - 1; i >= 0; i--) {
			if (listeMots[i].Taille < 2) {
				listeMots.RemoveAt (i);
			}
		}
	}

	/// <summary>
	/// Trouve tous les mots horizontaux de la grille
	/// </summary>
	private void TrouverMotsHorizontaux() {
		bool inWord = false;
		for (int y = 0; y < nbLignes; y++) {    //For every line
			for (int x = 0; x < nbColonnes; x++) {	//For every column
				if (listeLettres[x, y].valeur != null) {	//If Lettre is not a black space
					if (inWord == false) {	//If a new word is starting
						listeMotsHorizontaux.Add (new Mot(true, (y + 1)));
						inWord = true;
					}
					listeMotsHorizontaux [listeMotsHorizontaux.Count - 1].AjouterLettre (listeLettres [x, y]);
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
	}

	/// <summary>
	/// Trouve tous les mots verticaux de la grille
	/// </summary>
	private void TrouverMotsVerticaux() {
		bool inWord = false;
		for (int x = 0; x < nbColonnes; x++) {
			for (int y = 0; y < nbLignes; y++) {
				if (listeLettres[x, y].valeur != null) {
					if (inWord == false) {
						listeMotsVerticaux.Add (new Mot(false, x + 1));
						inWord = true;
					}
					listeMotsVerticaux [listeMotsVerticaux.Count - 1].AjouterLettre (listeLettres [x, y]);
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
	}

	/// <summary>
	/// Lit le plan des cases et crée une grille
	/// </summary>
	/// <param name="gridAsString"></param>
	private void LireGridFile(string gridAsString) {
		List<string> eachLine = new List<string>();
		eachLine.AddRange(gridAsString.Split("\n"[0]) );
		nbLignes = eachLine.Count - 1;
		nbColonnes = eachLine[0].Length - 1;
		listeLettres = new Lettre[nbLignes, nbColonnes];
		for (int y = 0; y < nbLignes; y++) {
			for (int x = 0; x < nbColonnes; x++) {
				if (eachLine[y].Substring(x, 1).CompareTo("1") == 0) {
					listeLettres [x, y] = new Lettre (".", x, y);
				} else {
					listeLettres [x, y] = new Lettre (null, x, y);
				}
			}
		}
	}

	/// <summary>
	/// Retourne la taille du plus long mot
	/// </summary>
	public int PlusLongMot {
		get {
			int taille = 0;
			foreach (Mot mot in listeMots) {
				if (mot.Taille > taille) {
					taille = mot.Taille;
				}
			}
			return taille;
		}
	}

	/// <summary>
	/// Calcule les scores de tous les mots de la grille
	/// </summary>
	public void CalculerScoresDeBase() {
		foreach (Mot mot in listeMots) {
			mot.CalculerScoreDeBase();
		}
	}

	/// <summary>
	/// Calcule les scores de tous les mots de la grille
	/// </summary>
	public void CalculerScores() {
		foreach (Mot mot in listeMots) {
			mot.CalculerScore();
		}
	}

	/// <summary>
	/// Retourne la liste de mot passée en paramètre triée par scores
	/// </summary>
	public void TrierListeMotsARemplirParScore() {
		listeMotsARemplir = listeMotsARemplir.OrderByDescending(o => o.Score).ToList();
	}

	/// <summary>
	/// Efface tous les mots de la grille
	/// </summary>
	/// <param name="bd"></param>
	public void EffacerTout(Bd bd) {
		foreach (Mot mot in listeMots) {
			if (mot.Rempli) {
				mot.EffacerMot(bd);
			}
		}
	}

	#endregion Initialisation
}