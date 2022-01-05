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

	/// <summary>
	/// Enregistre une valeur dans la lettre courante (mais ne la marque pas immédiatement comme remplie)
	/// </summary>
	/// <param name="contenu"></param>
	public void EnregistrerLettre(string contenu) {
		valeur = contenu;
	}

	/// <summary>
	/// Efface la valeur de la lettre courante
	/// </summary>
	public void EffacerLettre(bool direction) {
		Mot motDansAutreDirection = ObtenirMotDansDirection(!direction);
		if (motDansAutreDirection == null || !motDansAutreDirection.Rempli) {
			valeur = ".";
		}
	}

	/// <summary>
	/// Effectue l'affichage de la lettre courante
	/// </summary>
	public void AfficherLettre() {
		this.GoText.text = (this.valeur);
	}

	/// <summary>
	/// Cache la lettre courante
	/// </summary>
	public void CacherLettre() {
		this.GoText.text = ".";
	}

	/// <summary>
	/// Marque le mots utilisant la lettre courante dans la direction spécifiée comme rempli
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="bd"></param>
	public void MarquerMotRempliDansDirection(bool direction, Bd bd) {
		Mot motAMarquer = ObtenirMotDansDirection(direction);
		if (motAMarquer != null) {
			motAMarquer.MarquerCommeRempli(bd.RechercherMotParContenu(motAMarquer.Contenu), bd, false);
		}
	}

	#endregion Écriture

	#region OutilsDeRecherche

	/// <summary>
	/// Retourne vrai si le dictionnaire contient au moins un mot pouvant s'inscrire dans les mots qui contiennent la lettre courante
	/// </summary>
	/// <param name="horizontal"></param>
	/// <param name="bd"></param>
	/// <returns></returns>
	public bool ExistentMots(bool horizontal, Bd bd) {
		if (ObtenirMotDansDirection(horizontal) == null || ObtenirMotDansDirection(horizontal).Rempli || bd.ExistentMotsPossibles(ObtenirMotDansDirection(horizontal).Contenu)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Retourne la lettre suivante
	/// </summary>
	/// <param name="horizontal"></param>
	/// <returns></returns>
	public Lettre Suivante(bool direction) {
		int index = ObtenirMotDansDirection(direction).ListeLettres.IndexOf(this);
		if (index < (ObtenirMotDansDirection(direction).Taille - 1)) {
			return ObtenirMotDansDirection(direction).ListeLettres[index + 1];
		} else {
			return ObtenirMotDansDirection(direction).Suivant.ListeLettres[0];
		}
	}

	/// <summary>
	/// Retourne la lettre précédente
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public Lettre Precedente(bool direction) {
		int index = ObtenirMotDansDirection(direction).ListeLettres.IndexOf(this);
		if (index > 0) {
			return ObtenirMotDansDirection(direction).ListeLettres[index - 1];
		} else {
			return ObtenirMotDansDirection(direction).Precedent.ListeLettres[ObtenirMotDansDirection(direction).Precedent.Taille - 1];
		}
	}

	/// <summary>
	/// Retourne le mot d'une direction spécifiée qui contient la lettre courante
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
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