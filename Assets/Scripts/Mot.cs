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
	private List<string> contenusPrecedents = new List<string>();
	private int positionPrimaire;
	private int positionSecondaire;
	private MotDico motDico;

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
	}

	public void AjouterLettre(Lettre l) {
		listeLettres.Add(l);
		taille = listeLettres.Count;
	}

	public void CalculerScore() {
		foreach (Lettre lettre in listeLettres) {
			if (lettre.ObtenirMotDansDirection(!horizontal) != null) {
				score += 1;
			}
		}
	}

	#endregion Création

	#region Écriture

	public void EnregistrerMot(MotDico motDico, Bd bd) {
		for (int i = 0; i < this.taille; i++) {
			listeLettres[i].EnregistrerLettre(motDico.contenu.Substring(i, 1).ToUpper(), bd);
		}
	}

	public void EffacerMot(Bd bd) {
		rempli = false;
		if (this.motDico != null) {
			bd.MarquerMotUtilise(this.motDico, false);
			this.motDico = null;
		}
		contenusPrecedents.Add(Contenu);
		foreach (Lettre lettre in listeLettres) {
			lettre.EffacerLettre();
			lettre.MarquerMotRempli(!horizontal, bd);
		}
	}

	public void AfficherMot() {
		foreach (Lettre lettre in listeLettres) {
			lettre.AfficherLettre();
		}
	}

	public void MarquerCommeRempli(MotDico motDico, Bd bd) {
		rempli = true;
		this.motDico = motDico;
		bd.MarquerMotUtilise(motDico, true);
		foreach (Lettre lettre in listeLettres) {
			lettre.MarquerMotRempli(!horizontal, bd);
		}
	}

	#endregion Écriture

	#region OutilsDeReherche

	public string[] ObtenirDefinitions(Bd bd) {
		MotDico motDef = bd.RechercherMotParContenuPourDefinitions(this.Contenu);
		if (motDef != null && motDef.definitions.Length > 0) {
			return motDef.definitions;
		} else {
			Debug.Log("Le mot " + this.Contenu + " n'a pas été trouvé dans le dictionnaire de définitions.");
			return new string[] { "Définition indéterminée" };
		}
	}

	public bool ExistentMotsTransversaux(Bd bd) {
		foreach (Lettre lettre in listeLettres) {
			if (!lettre.ExistentMots(!horizontal, bd)) {
				return false;
			}
		}
		return true;
	}

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
			return this.motDico;
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
