using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class Grille {
	public int nbLignes {get; private set;}
	public int nbColonnes {get; private set;}
	public Lettre[,] listeLettres;
	public List<Mot> listeMotsHorizontaux;
	public List<Mot> listeMotsVerticaux;

	public Grille(TextAsset gridFile) {
		listeMotsHorizontaux = new List<Mot> ();
		listeMotsVerticaux = new List<Mot> ();
		lireGridFile (gridFile);
		trouverMotsHorizontaux ();
		trouverMotsVerticaux ();
	}

	private void trouverMotsHorizontaux() {
		bool inWord = false;
		for (int y = 0; y < nbLignes; y++) {
			for (int x = 0; x < nbColonnes; x++) {
				if (listeLettres[x, y].valeur != null) {
					if (inWord == false) {
						listeMotsHorizontaux.Add (new Mot());
						inWord = true;
						if (listeMotsHorizontaux.Count > 1){
							listeMotsHorizontaux [listeMotsHorizontaux.Count - 2].Suivant = listeMotsHorizontaux [listeMotsHorizontaux.Count - 1];
							listeMotsHorizontaux [listeMotsHorizontaux.Count - 1].Precedent = listeMotsHorizontaux [listeMotsHorizontaux.Count - 2];
						}
					}
					listeMotsHorizontaux [listeMotsHorizontaux.Count - 1].AjouterLettre (listeLettres [x, y]);
					listeLettres [x, y].MotHorizontal = listeMotsHorizontaux [listeMotsHorizontaux.Count - 1];
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
		if (listeMotsHorizontaux.Count > 1) {
			listeMotsHorizontaux [listeMotsHorizontaux.Count - 1].Suivant = listeMotsHorizontaux [0];
			listeMotsHorizontaux [0].Precedent = listeMotsHorizontaux [listeMotsHorizontaux.Count - 1];
		}
	}

	private void trouverMotsVerticaux() {
		bool inWord = false;
		for (int x = 0; x < nbColonnes; x++) {
			for (int y = 0; y < nbLignes; y++) {
				if (listeLettres[x, y].valeur != null) {
					if (inWord == false) {
						listeMotsVerticaux.Add (new Mot());
						inWord = true;
						if (listeMotsVerticaux.Count > 1){
							listeMotsVerticaux [listeMotsVerticaux.Count - 2].Suivant = listeMotsVerticaux [listeMotsVerticaux.Count - 1];
							listeMotsVerticaux [listeMotsVerticaux.Count - 1].Precedent = listeMotsVerticaux [listeMotsVerticaux.Count - 2];
						}
					}
					listeMotsVerticaux [listeMotsVerticaux.Count - 1].AjouterLettre (listeLettres [x, y]);
					listeLettres [x, y].MotVertical = listeMotsVerticaux [listeMotsVerticaux.Count - 1];
				} else {
					inWord = false;
				}
			}
			inWord = false;
		}
		if (listeMotsVerticaux.Count > 1){
			listeMotsVerticaux [listeMotsVerticaux.Count - 1].Suivant = listeMotsVerticaux [0];
			listeMotsVerticaux [0].Precedent = listeMotsVerticaux [listeMotsVerticaux.Count - 1];
		}
	}

	//Extracts the black and whites spaces from the text file and creates the corresponding spaces in the grid
	private void lireGridFile(TextAsset gridFile) {
		string gridFilecontent = gridFile.text;
		List<string> eachLine = new List<string>();
		eachLine.AddRange(gridFilecontent.Split("\n"[0]) );
		nbLignes = eachLine.Count - 1;
		nbColonnes = eachLine [0].Length - 1;
		listeLettres = new Lettre[nbLignes, nbColonnes];
		for (int y = 0; y < nbLignes; y++) {
			for (int x = 0; x < nbColonnes; x++) {
				if (eachLine[y].Substring(x, 1).CompareTo("1") == 0) {
					listeLettres [x, y] = new Lettre ("", x, y);
				} else {
					listeLettres [x, y] = new Lettre (null, x, y);
				}
			}
		}
	}

	public int PlusLongMot {
		get {
			int taille = 0;
			foreach (Mot mot in listeMotsHorizontaux) {
				if (mot.Taille > taille) {
					taille = mot.Taille;
				}
			}
			foreach (Mot mot in listeMotsVerticaux) {
				if (mot.Taille > taille) {
					taille = mot.Taille;
				}
			}
			return taille;
		}
	}
}