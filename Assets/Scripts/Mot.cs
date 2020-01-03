using System.Collections.Generic;
using UnityEngine;

public class Mot {
	private List<Lettre> listeLettres;
	private bool rempli;
	private bool horizontal;
	private int taille;
	private Mot precedent;
	private Mot suivant;
	private int score;
	private int scoreDeBase;
	private List<string> contenusPrecedents = new List<string>();
	private int positionPrimaire;
	private int positionSecondaire;
	private MotDico motDico;
	public int nbTentativesDeRemplissage;

	#region Création

	public Mot (bool horizontal, int positionPrimaire) {
		this.listeLettres = new List<Lettre> ();
		this.rempli = false;
		this.horizontal = horizontal;
		this.taille = 0;
		this.precedent = null;
		this.suivant = null;
		this.positionPrimaire = positionPrimaire;
		this.positionSecondaire = 0;
		this.nbTentativesDeRemplissage = 0;
	}

	/// <summary>
	/// Ajoute une lettre au mot courant
	/// </summary>
	/// <param name="l"></param>
	public void AjouterLettre(Lettre l) {
		listeLettres.Add(l);
		taille = listeLettres.Count;
	}

	/// <summary>
	/// Calcule le score de base du mot courant
	/// </summary>
	public void CalculerScoreDeBase() {
		scoreDeBase = 0;
		foreach (Lettre lettre in listeLettres) {
			if (lettre.ObtenirMotDansDirection(!horizontal) != null) {
				scoreDeBase++;
			}
		}
	}

	/// <summary>
	/// Calcule le score du mot courant
	/// </summary>
	public void CalculerScore() {
		score = scoreDeBase;
		foreach (Lettre lettre in listeLettres) {
			if (lettre.valeur != ".") {
				score += 5;
			}
		}
	}

	/// <summary>
	/// Calcule les scores des mots transversaux au mot courant
	/// </summary>
	public void CalculerScoreDesMotsTransversaux() {
		foreach (Lettre lettre in listeLettres) {
			Mot motTransversal = lettre.ObtenirMotDansDirection(!this.horizontal);
			if (motTransversal != null) {
				motTransversal.CalculerScore();
			}
		}
	}

	#endregion Création

	#region Écriture

	/// <summary>
	/// Enregistre une valeur dans le mot courant (mais ne le marque pas immédiatement comme rempli)
	/// </summary>
	/// <param name="motDico"></param>
	/// <param name="bd"></param>
	public void EnregistrerMot(MotDico motDico, Bd bd) {
		for (int i = 0; i < this.taille; i++) {
			listeLettres[i].EnregistrerLettre(motDico.contenu.Substring(i, 1).ToUpper());
		}
	}

	/// <summary>
	/// Efface le mot courant
	/// </summary>
	/// <param name="bd"></param>
	public void EffacerMot(Bd bd) {
		contenusPrecedents.Add(Contenu);
		foreach (Lettre lettre in listeLettres) {
			lettre.EffacerLettre(horizontal);
		}
		MarquerCommeRempli(motDico, bd, false);
	}

	/// <summary>
	/// Effectue l'affichage du mot courant
	/// </summary>
	public void AfficherMot() {
		foreach (Lettre lettre in listeLettres) {
			lettre.AfficherLettre();
		}
	}

	/// <summary>
	/// Marque le mot courant comme rempli
	/// </summary>
	/// <param name="motDico"></param>
	/// <param name="bd"></param>
	public void MarquerCommeRempli(MotDico motDico, Bd bd, bool verifierTransversaux) {
		if (rempli) {
			if (Contenu.Contains(".")) {
				rempli = false;
				bd.MarquerMotUtilise(this.motDico, false);
				this.motDico = null;
			}
		} else {
			if (!Contenu.Contains(".")) {
				rempli = true;
				this.motDico = motDico;
				bd.MarquerMotUtilise(motDico, true);
			}
		}
		if (verifierTransversaux) {
			foreach (Lettre lettre in listeLettres) {
				lettre.MarquerMotRempliDansDirection(!horizontal, bd);
			}
		}
	}

	#endregion Écriture

	#region OutilsDeReherche

	/// <summary>
	/// Retourne les définitions du mot courant
	/// </summary>
	/// <param name="bd"></param>
	/// <returns></returns>
	public string[] ObtenirDefinitions(Bd bd) {
		MotDico motDef = bd.RechercherMotParContenuPourDefinitions(this.Contenu);
		if (motDef != null && motDef.definitions.Length > 0) {
			return motDef.definitions;
		} else {
			Debug.Log("Le mot " + this.Contenu + " n'a pas été trouvé dans le dictionnaire de définitions.");
			return new string[] { "Définition indéterminée" };
		}
	}

	/// <summary>
	/// Retourne vrai si le dictionnaire contient des mots pouvant s'insérer transversalement au mot courant
	/// </summary>
	/// <param name="bd"></param>
	/// <returns></returns>
	public bool ExistentMotsTransversaux(Bd bd) {
		foreach (Lettre lettre in listeLettres) {
			if (!lettre.ExistentMots(!horizontal, bd)) {
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Retourne au hasard l'un des mots transversux remplis
	/// </summary>
	/// <returns></returns>
	public Mot ObtenirMotTransversalRempliAleatoire() {
		List<Mot> listeMotsTransversaux = new List<Mot>();
		foreach (Lettre lettre in listeLettres) {
			Mot motTransversal = lettre.ObtenirMotDansDirection(!horizontal);
			if (motTransversal != null && motTransversal.Rempli) {
				listeMotsTransversaux.Add(motTransversal);
			}
		}
		if (listeMotsTransversaux.Count != 0) {
			return listeMotsTransversaux[Random.Range(0, listeMotsTransversaux.Count)];
		} else {
			return null;
		}
	}

	/// <summary>
	/// Retourne au hasard l'un des mots adjacents remplis
	/// </summary>
	/// <returns></returns>
	public Mot ObtenirMotAdjacentRempliAleatoire() {
		List<Mot> listeMotsAdjacents = new List<Mot>();
		foreach (Lettre lettre in listeLettres) {
			Mot motTransversal = lettre.ObtenirMotDansDirection(!horizontal);
			if (motTransversal != null) {
				foreach (Lettre lettreTransversale in motTransversal.listeLettres) {
					Mot motAdjacent = lettreTransversale.ObtenirMotDansDirection(horizontal);
					if (motAdjacent != null && motAdjacent.Rempli) {
						listeMotsAdjacents.Add(motAdjacent);
					}
				}
			}
		}
		if (listeMotsAdjacents.Count != 0) {
			return listeMotsAdjacents[Random.Range(0, listeMotsAdjacents.Count)];
		} else {
			return null;
		}
	}

	#endregion OutilsDeReherche

	#region AccesseursMutateurs

	public List<Lettre> ListeLettres {
		get {
			return this.listeLettres;
		}
	}

	public bool Rempli {
		get {
			return this.rempli;
		}
		set {
			this.rempli = value;
		}
	}

	public bool Horizontal {
		get {
			return this.horizontal;
		}
	}

	public int Taille {
		get {
			return this.taille;
		}
	}

	public Mot Precedent {
		get {
			return this.precedent;
		}
		set {
			this.precedent = value;
		}
	}

	public Mot Suivant {
		get {
			return this.suivant;
		}
		set {
			this.suivant = value;
		}
	}

	public string Contenu {
		get {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < taille; i++) {
				sb.Append(listeLettres[i].valeur);
			}
			return sb.ToString ();
		}
	}

	public int Score {
		get {
			return this.score;
		}
	}

	public int PositionPrimaire {
		get {
			return this.positionPrimaire;
		}
	}

	public int PositionSecondaire {
		get {
			return this.positionSecondaire;
		}
		set {
			this.positionSecondaire = value;
		}
	}

	public MotDico MotDico {
		get {
			return motDico;
		}
		set {
			this.motDico = value;
		}
	}

	public List<string> ContenusPrecedents {
		get {
			return this.contenusPrecedents;
		}
	}

	#endregion AccesseursMutateurs
}
