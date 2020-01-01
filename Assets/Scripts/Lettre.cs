using UnityEngine;

public class Lettre {

	public string valeur;
	private int x;
	private int y;
	private Mot motHorizontal;
	private Mot motVertical;
	private GameObject go;
	private TextMesh goText;
	private SpriteRenderer goRenderer;

	#region Création

	public Lettre (string valeur, int x, int y) {
		this.valeur = valeur;
		this.x = x;
		this.y = y;
		this.motHorizontal = null;
		this.motVertical = null;
		this.go = null;
		this.goText = null;
		this.goRenderer = null;
	}

	#endregion Création

	#region Écriture

	public void EnregistrerLettre(string contenu, Bd bd) {
		valeur = contenu;
		/*
		if (motHorizontal != null && !motHorizontal.Rempli && !motHorizontal.Contenu.Contains(".")) {
			motHorizontal.Rempli = true;
			MotDico motDico = bd.RechercherMotParContenu(motHorizontal.Contenu);
			motHorizontal.MotDico = motDico;
			bd.MarquerMotUtilise(motDico, true);
		}
		if (motVertical != null && !motVertical.Rempli && !motVertical.Contenu.Contains(".")) {
			motVertical.Rempli = true;
			MotDico motDico = bd.RechercherMotParContenu(motVertical.Contenu);
			motVertical.MotDico = motDico;
			bd.MarquerMotUtilise(motDico, true);
		}
		*/
	}

	public void EffacerLettre() {
		if ((motHorizontal == null || !motHorizontal.Rempli) && (motVertical == null || !motVertical.Rempli)) {
			valeur = ".";
		}
	}

	public void AfficherLettre() {
		this.GoText.text = (this.valeur);
	}

	public void MarquerMotRempli(bool direction, Bd bd) {
		Mot mot = ObtenirMotDansDirection(direction);
		if (mot != null) {
			if (mot.Rempli) {
				if (mot.Contenu.Contains(".")) {
					mot.Rempli = false;
					bd.MarquerMotUtilise(mot.MotDico, false);
					mot.MotDico = null;
				}
			} else {
				if (!mot.Contenu.Contains(".")) {
					mot.Rempli = true;
					mot.MotDico = bd.RechercherMotParContenu(mot.Contenu);
					bd.MarquerMotUtilise(mot.MotDico, true);
				}
			}
		}
	}

	#endregion Écriture

	#region OutilsDeRecherche

	public bool ExistentMots(bool horizontal, Bd bd) {
		if (ObtenirMotDansDirection(horizontal) == null || ObtenirMotDansDirection(horizontal).Rempli || bd.ExistentMotsPossibles(ObtenirMotDansDirection(horizontal).Contenu)) {
			return true;
		}
		return false;
	}

	public Lettre Suivante(bool horizontal) {
		int index = ObtenirMotDansDirection(horizontal).ListeLettres.IndexOf(this);
		if (index < (ObtenirMotDansDirection(horizontal).Taille - 1)) {
			return ObtenirMotDansDirection(horizontal).ListeLettres[index + 1];
		} else {
			return ObtenirMotDansDirection(horizontal).Suivant.ListeLettres[0];
		}
	}

	public Lettre Precedente(bool horizontal) {
		int index = ObtenirMotDansDirection(horizontal).ListeLettres.IndexOf(this);
		if (index > 0) {
			return ObtenirMotDansDirection(horizontal).ListeLettres[index - 1];
		} else {
			return ObtenirMotDansDirection(horizontal).Precedent.ListeLettres[ObtenirMotDansDirection(horizontal).Precedent.Taille - 1];
		}
	}

	public Mot ObtenirMotDansDirection(bool horizontal) {
		if (horizontal) {
			return MotHorizontal;
		} else {
			return motVertical;
		}
	}

	#endregion OutilsDeRecherche

	#region AccesseursMutateurs

	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public Mot MotHorizontal {
		get {
			return this.motHorizontal;
		}
		set {
			this.motHorizontal = value;
		}
	}

	public Mot MotVertical {
		get {
			return this.motVertical;
		}
		set {
			this.motVertical = value;
		}
	}

	public GameObject Go {
		get {
			return this.go;
		}
		set {
			this.go = value;
		}
	}

	public TextMesh GoText {
		get {
			return this.goText;
		}
		set {
			this.goText = value;
		}
	}

	public SpriteRenderer GoRenderer {
		get {
			return this.goRenderer;
		}
		set {
			this.goRenderer = value;
		}
	}

	#endregion AccesseursMutateurs
}