using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grille {
	public int nbLignes {get; private set;}
	public int nbColonnes {get; private set;}
	public Lettre[,] listeLettres;
	public List<Mot> listeMotsHorizontaux;
	public List<Mot> listeMotsVerticaux;
	public List<Mot> listeMots;
	
	#region Création

	public Grille(TextAsset gridFile) {
		listeMotsHorizontaux = new List<Mot> ();
		listeMotsVerticaux = new List<Mot> ();
		LireGridFile (gridFile);
		TrouverMotsHorizontaux ();
		TrouverMotsVerticaux ();
		TrouverMotsVoisins (listeMotsHorizontaux);
		TrouverMotsVoisins (listeMotsVerticaux);
		SupprimerMotsUneLettre (listeMotsHorizontaux);
		SupprimerMotsUneLettre (listeMotsVerticaux);
		TrouverMotsVoisins (listeMotsHorizontaux);
		TrouverMotsVoisins (listeMotsVerticaux);
		AttribuerPositionSecondaires(listeMotsHorizontaux);
		AttribuerPositionSecondaires(listeMotsVerticaux);
		listeMots = new List<Mot> ();
		listeMots.AddRange (listeMotsHorizontaux);
		listeMots.AddRange (listeMotsVerticaux);
		CalculerScores();
	}

	#endregion Création

	#region Outils

	public void AfficherMots() {
		foreach (Mot mot in listeMots) {
			mot.AfficherMot();
		}
	}

	#endregion Outils

	#region Initialisation

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

	private void TrouverMotsVoisins(List<Mot> listeMots) {
		for (int i = 0, max = listeMots.Count; i < max; i++) {
			listeMots [i].Precedent = ((i - 1) >= 0) ? listeMots[i - 1] : listeMots[max - 1];
			listeMots [i].Suivant = ((i + 1) < max) ? listeMots[i + 1] : listeMots[0];
		}
	}

	private void SupprimerMotsUneLettre(List<Mot> listeMots) {
		for (int i = listeMots.Count - 1; i >= 0; i--) {
			if (listeMots[i].Taille < 2) {
				listeMots.RemoveAt (i);
			}
		}
	}

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
					listeLettres [x, y].MotHorizontal = listeMotsHorizontaux [listeMotsHorizontaux.Count - 1];
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
	}

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
					listeLettres [x, y].MotVertical = listeMotsVerticaux [listeMotsVerticaux.Count - 1];
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
	}

	//Extracts the black and whites spaces from the text file and creates the corresponding spaces in the grid
	private void LireGridFile(TextAsset gridFile) {
		string gridFilecontent = gridFile.text;
		List<string> eachLine = new List<string>();
		eachLine.AddRange(gridFilecontent.Split("\n"[0]) );
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

	public void CalculerScores() {
		foreach (Mot mot in listeMots) {
			mot.CalculerScore();
		}
		listeMots.Sort((x, y) => x.Score.CompareTo(y.Score));
		listeMots.Reverse();
	}

	public void EffacerTout(Bd bd) {
		foreach (Mot mot in listeMots) {
			if (mot.Rempli) {
				mot.EffacerMot(bd);
			}
		}
	}

	#endregion Initialisation
}