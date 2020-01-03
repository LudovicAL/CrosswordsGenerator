using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selecteur))]
public class LaunchManager : MonoBehaviour {

	public Grille grille;
	public Bd bd;
	public TextAsset[] gridFiles;
	public TextAsset[] fichiersDicos;
	public GameObject whiteSpace;
	public GameObject blackSpace;
	public GameObject greySpace;
	public int nbEssaisMaxGlobal;
	public int nbEssaisMaxPourMot;
	public Definisseur definisseur;
	public Remplisseur remplisseur = new Remplisseur();
	public Exporteur exporteur = new Exporteur();
	[HideInInspector] public Selecteur selecteur;
	[HideInInspector] public List<GameObject> listeNumeros = new List<GameObject>();

	void Awake () {
		selecteur = gameObject.GetComponent<Selecteur>();
		ConstruireGrilleVide(gridFiles[Random.Range(0, gridFiles.Length)].text);
	}

	void Update () {
		if (Input.GetKeyDown("space")) {
			remplisseur.RemplirGrille (bd, grille, nbEssaisMaxGlobal, nbEssaisMaxPourMot, false);
			grille.AfficherMots();
		}
		if (Input.GetKeyDown("a")) {
			definisseur.AfficherDefinitions(grille, bd);
		}
		if (Input.GetKeyDown("b")) {
			exporteur.SauvegarderGrille(grille, "grille12");
		}
		if (Input.GetKeyDown("c")) {
			SupprimerGrille();
			GrilleSerializable grilleSerializable = exporteur.ChargerGrille("grille12");
			string gridAsString = grilleSerializable.ObtenirGridAsString();
			ConstruireGrilleVide(gridAsString);
			grilleSerializable.RemplirGrille(grille, bd);
			definisseur.AfficherDefinitions(grille, bd);
		}
		if (Input.GetKeyDown("r")) {
			SupprimerGrille();
			int rnd = Random.Range(0, (gridFiles.Length));
			ConstruireGrilleVide(gridFiles[rnd].text);
			Debug.Log("Reset " + rnd);
		}
		if (Input.GetKeyDown("y")) {
			foreach (Mot mot in grille.listeMots) {
				Debug.Log((mot.Horizontal ? "Horizontal ":"Vertical ") + mot.PositionPrimaire + ":" + mot.PositionSecondaire + "  " + mot.Contenu + ";" + mot.Taille + ";" + mot.Rempli);
			}
		}
	}

	public void SupprimerGrille() {
		foreach (Lettre lettre in grille.listeLettres) {
			Destroy(lettre.Go);
		}
		grille = null;
		for (int i = (listeNumeros.Count - 1); i >= 0; i--) {
			Destroy(listeNumeros[i]);
		}
		listeNumeros = new List<GameObject>();
		definisseur.ViderDefinitions();
	}

	public void ConstruireGrilleVide(string gridAsString) {
		grille = new Grille(gridAsString);
		for (int y = 0; y < grille.nbLignes; y++) {
			for (int x = 0; x < grille.nbLignes; x++) {
				if (grille.listeLettres[x, y].valeur != null) {
					grille.listeLettres[x, y].Go = Object.Instantiate(whiteSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
					grille.listeLettres[x, y].GoText = grille.listeLettres[x, y].Go.GetComponentInChildren<TextMesh>();
					grille.listeLettres[x, y].GoRenderer = grille.listeLettres[x, y].Go.GetComponent<SpriteRenderer>();
				} else {
					grille.listeLettres[x, y].Go = Object.Instantiate(blackSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				}
			}
		}
		for (int y = 0; y < grille.nbLignes; y++) {
			GameObject numero = Object.Instantiate(greySpace, new Vector3(-1.0f, (float)-y, 0.0f), Quaternion.identity);
			numero.GetComponentInChildren<TextMesh>().text = (y + 1).ToString();
			listeNumeros.Add(numero);
		}
		for (int x = 0; x < grille.nbColonnes; x++) {
			GameObject numero = Object.Instantiate(greySpace, new Vector3((float)x, 1.0f, 0.0f), Quaternion.identity);
			numero.GetComponentInChildren<TextMesh>().text = (x + 1).ToString();
			listeNumeros.Add(numero);
		}
		bd = new Bd(fichiersDicos, grille.PlusLongMot);
		selecteur.Initialiser();
	}
}