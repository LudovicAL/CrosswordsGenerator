using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviour {

	public Grille grille;
	public int idGrilleVide;
	public int idGrillePleine;
	public int idCaptureEcran;
	public Bd bd;
	public TextAsset[] fichiersDicos;
	public GameObject whiteSpace;
	public GameObject blackSpace;
	public GameObject greySpace;
	public int nbEssaisMaxGlobal;
	public int nbEssaisMaxPourMot;
	public Remplisseur remplisseur = new Remplisseur();
	public Exporteur exporteur = new Exporteur();
	public Text afficheurStats;
	public Image workIndicator;
	[Range(0.0f, 2.0f)] public float probabilitesDeCaseNoire;
	public int tailleGrille;

	public static LaunchManager Instance { get; private set; }

	private GrilleMaker gridMaker = new GrilleMaker();
	[HideInInspector] public List<GameObject> listeNumeros = new List<GameObject>();

	void Awake () {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
		GenererGrilleVide();
	}

	void Update () {
		//Ajoute un mot dans la grille
		if (Input.GetKeyDown("space")) {
			RemplirGrilleNbFois(nbEssaisMaxGlobal);
		}
		//Affiche les définitions
		if (Input.GetKeyDown("d")) {
			AfficherDefinitions();
		}
		//Prend une capture d'écran et l'enregistre au format PNG
		if (Input.GetKeyDown("p")) {
			StartCoroutine(Captureur.Instance.CapturerImage(idCaptureEcran));
		}
		//Sauvegarde la grille
		if (Input.GetKeyDown("s")) {
			SauvegarderGrille(true);
		}
		//Charge une grille
		if (Input.GetKeyDown("c")) {
			ChargerGrille(true, idGrillePleine);
		}
		//Génère une grille vide
		if (Input.GetKeyDown("r")) {
			GenererGrilleVide();
		}
		//Blanchi une grille
		if (Input.GetKeyDown("b")) {
			foreach (Mot mot in grille.listeMots) {
				mot.CacherMot();
			}
		}
		//Log les détails de chaque mots de la grille
		if (Input.GetKeyDown("i")) {
			foreach (Mot mot in grille.listeMots) {
				Debug.Log((mot.Horizontal ? "Horizontal ":"Vertical ") + mot.PositionPrimaire + ":" + mot.PositionSecondaire + "  " + mot.Contenu + ";" + mot.Taille + ";" + mot.Rempli);
			}
		}
		//Change la couleur d'une case de la grille
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				foreach (Lettre lettre in grille.listeLettres) {
					if (lettre.Go == hit.collider.gameObject) {
						if (lettre.valeur == null) {
							lettre.valeur = ".";
							lettre.Go.GetComponent<SpriteRenderer>().color = Color.white;
						} else {
							lettre.valeur = null;
							lettre.Go.GetComponent<SpriteRenderer>().color = Color.black;
						}
						afficheurStats.text = "Nb cases noires: " + grille.CompterCasesNoires();
						break;
					}	
				}
			}
		}
	}

	//Effectue x essais pour remplir la grille
	public void RemplirGrilleNbFois(int nbFois) {
		remplisseur.RemplirGrille(bd, grille, nbFois, nbEssaisMaxPourMot, false);
		grille.AfficherMots();
	}

	//Genère une grille vide
	public void GenererGrilleVide() {
		SupprimerGrille();
		ConstruireGrilleVide(gridMaker.ConstruireGrille(tailleGrille, tailleGrille, probabilitesDeCaseNoire));
		afficheurStats.text = "Nb cases noires: " + grille.CompterCasesNoires();
		Debug.Log("Reset ");
	}

	//Affiche les définitions
	public void AfficherDefinitions() {
		Definisseur.Instance.AfficherDefinitions(grille, bd);
	}

	//Charge une grille
	public void ChargerGrille(bool avecSolution, int idGrille) {
		SupprimerGrille();	
		GrilleSerializable grilleSerializable = exporteur.ChargerGrille(avecSolution, idGrille);
		string gridAsString = grilleSerializable.ObtenirGridAsString();
		ConstruireGrilleVide(gridAsString);
		grilleSerializable.RemplirGrille(grille, bd);
		Definisseur.Instance.AfficherDefinitions(grille, bd);
		afficheurStats.text = "Nb cases noires: " + grille.CompterCasesNoires();
	}

	//Sauvegarde une grille
	public void SauvegarderGrille(bool avecSolution) {
		exporteur.SauvegarderGrille(grille, avecSolution);
	}

	public void SupprimerGrille() {
		if (grille != null) {
			foreach (Lettre lettre in grille.listeLettres) {
				Destroy(lettre.Go);
			}
			grille = null;
			for (int i = (listeNumeros.Count - 1); i >= 0; i--) {
				Destroy(listeNumeros[i]);
			}
			listeNumeros = new List<GameObject>();
			Definisseur.Instance.ViderDefinitions();
		}
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
		bd = new Bd(fichiersDicos, tailleGrille);
		Selecteur.Instance.Initialiser();
	}

	public void ModifierIdGrilleVide(int modificateur) {
		idGrilleVide = Mathf.Max(1, idGrilleVide + modificateur);
	}

	public void ModifierIdGrillePleine(int modificateur) {
		idGrillePleine = Mathf.Max(1, idGrillePleine + modificateur);
	}
}