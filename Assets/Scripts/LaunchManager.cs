using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour {

	public Grille grille;
	public Bd bd;
	public TextAsset gridFile;
	public TextAsset[] fichiersDicos;
	public GameObject whiteSpace;
	public GameObject blackSpace;
	public GameObject greySpace;
	public int nbEssaisMaxGlobal;
	public int nbEssaisMaxPourMot;
	public Definisseur definisseur;
	public Remplisseur remplisseur = new Remplisseur();
	public Exporteur exporteur = new Exporteur();

	void Awake () {
		grille = new Grille (gridFile);
		for (int y = 0; y < grille.nbLignes; y++) {
			for (int x = 0; x < grille.nbLignes; x++) {
				if (grille.listeLettres[x, y].valeur != null) {
					grille.listeLettres[x, y].Go = Object.Instantiate(whiteSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
					grille.listeLettres[x, y].GoText = grille.listeLettres[x, y].Go.GetComponentInChildren<TextMesh> ();
					grille.listeLettres[x, y].GoRenderer = grille.listeLettres[x, y].Go.GetComponent<SpriteRenderer>();
				} else {
					grille.listeLettres[x, y].Go = Object.Instantiate(blackSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				}
			}
		}
		for (int y = 0; y < grille.nbLignes; y++) {
			GameObject numero = Object.Instantiate(greySpace, new Vector3(-1.0f, (float)-y, 0.0f), Quaternion.identity);
			numero.GetComponentInChildren<TextMesh>().text = (y + 1).ToString();
		}
		for (int x = 0; x < grille.nbColonnes; x++) {
			GameObject numero = Object.Instantiate(greySpace, new Vector3((float)x, 1.0f, 0.0f), Quaternion.identity);
			numero.GetComponentInChildren<TextMesh>().text = (x + 1).ToString();
		}
		bd = new Bd(fichiersDicos, grille.PlusLongMot);
	}
	
	void Update () {
		if (Input.GetKeyDown("space")) {
			remplisseur.RemplirGrille (bd, grille.listeMots, nbEssaisMaxGlobal, nbEssaisMaxPourMot, false);
			grille.AfficherMots();
		}
		if (Input.GetKeyDown("a")) {
			definisseur.AfficherDefinitions(grille, bd);
		}
		if (Input.GetKeyDown("b")) {
			exporteur.SauvegarderGrille(grille, "grille1");
		}
		if (Input.GetKeyDown("c")) {
			grille = exporteur.ChargerGrille("grille1");
		}
	}
}