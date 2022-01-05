using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Remplisseur {
	private List<MotDico> listeMotsPossibles;

	/// <summary>
	/// Remplir la grille
	/// </summary>
	/// <param name="bd"></param>
	/// <param name="listeMots"></param>
	/// <param name="nbEssaisMaxGlobal"></param>
	/// <param name="nbEssaisMaxPourMot"></param>
	/// <param name="afficher"></param>
	public void RemplirGrille(Bd bd, Grille grille, int nbEssaisMaxGlobal, int nbEssaisMaxPourMot, bool afficher) {
		int nbEssaisGlobal = 0;
		bool trier = false;
		CalculerScoresDesMots(grille.listeMotsARemplir);
		Mot motARemplir = ObtenirProchainMotARemplir(grille, true);
		while (motARemplir != null && nbEssaisGlobal < nbEssaisMaxGlobal) {
			//Debug.Log((motARemplir.Horizontal ? "Horizontal " : "Vertical ") + motARemplir.PositionPrimaire + ":" + motARemplir.PositionSecondaire);
			if (motARemplir.nbTentativesDeRemplissage == 10) {
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
			} else if (motARemplir.nbTentativesDeRemplissage == 20) {
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
			} else if (motARemplir.nbTentativesDeRemplissage == 30) {
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
			} else if (motARemplir.nbTentativesDeRemplissage == 40) {
				RetirerMotsTransversaux(grille, motARemplir, bd, afficher);
				motARemplir.nbTentativesDeRemplissage = 0;
			}
			//RemplirMotSelonScore(motARemplir, bd, nbEssaisMaxPourMot, afficher);
			RemplirMot(motARemplir, bd, nbEssaisMaxPourMot, afficher);
			motARemplir.nbTentativesDeRemplissage++;
			if (!motARemplir.Rempli) {
				RetirerMotAleatoire(grille, motARemplir, bd, afficher);
			} else {
				motARemplir.CalculerScoreDesMotsTransversaux();
				grille.listeMotsARemplir.Remove(motARemplir);
				trier = true;
			}
			nbEssaisGlobal++;
			motARemplir = ObtenirProchainMotARemplir(grille, trier);
		}
		if (nbEssaisGlobal == 0) {
			Debug.Log("Grille déjà remplie");
		}
	}

	/// <summary>
	/// Calcule les scores de tous les mots de la grille
	/// </summary>
	/// <param name="listeMotsARemplir"></param>
	public void CalculerScoresDesMots(List<Mot> listeMotsARemplir) {
		foreach (Mot mot in listeMotsARemplir) {
			mot.CalculerScore();
		}
	}

	/// <summary>
	/// Retourne le prochain mot à remplir
	/// </summary>
	/// <param name="listeMotsARemplir"></param>
	/// <returns></returns>
	public Mot ObtenirProchainMotARemplir(Grille grille, bool trier) {
		grille.listeMotsARemplir.RemoveAll(o => o.Rempli);
		if (grille.listeMotsARemplir.Count > 0) {
			if (trier) {
				grille.TrierListeMotsARemplirParScore();
			}
			return grille.listeMotsARemplir[0];
		} else {
			return null;
		}
	}

	/// <summary>
	/// Rempli le mot spécifié
	/// </summary>
	/// <param name="mot"></param>
	/// <param name="bd"></param>
	/// <param name="nbEssaisMaxPourMot"></param>
	/// <param name="afficher"></param>
	public void RemplirMot(Mot mot, Bd bd, int nbEssaisMaxPourMot, bool afficher) {
		listeMotsPossibles = bd.ListeMotsPossibles(mot.Contenu);
		for (int j = 0, nbMotsPossibles = listeMotsPossibles.Count; j < nbEssaisMaxPourMot && j < nbMotsPossibles; j++) {
			int rnd = Random.Range(0, nbMotsPossibles - 1);
			mot.EnregistrerMot(listeMotsPossibles[rnd], bd);
			if (mot.ExistentMotsTransversaux(bd)) {
				//Debug.Log("Ai écris " + mot.Contenu);
				mot.MarquerCommeRempli(listeMotsPossibles[rnd], bd, true);
				if (afficher) {
					mot.AfficherMot();
				}
				break;
			} else {
				//Debug.Log("Ai tenté d'écrire " + mot.Contenu + " mais pas de mots transversaux existants");
				mot.EffacerMot(bd);
				listeMotsPossibles.RemoveAt(rnd);
				nbMotsPossibles--;
			}
		}
	}

	/// <summary>
	/// Retourne vrai si le programme réussi a remplir le mot spécifié en utilisant les scores des mots du dictionnaire
	/// </summary>
	/// <param name="mot"></param>
	/// <param name="bd"></param>
	/// <param name="nbEssaisMaxPourMot"></param>
	/// <param name="afficher"></param>
	public void RemplirMotSelonScore(Mot mot, Bd bd, int nbEssaisMaxPourMot, bool afficher) {
		listeMotsPossibles = bd.ListeMotsPossiblesTriesParScore(mot.Contenu);
		for (int j = 0, nbMotsPossibles = listeMotsPossibles.Count; j < nbEssaisMaxPourMot && j < nbMotsPossibles; j++) {
			int nbEchange = 0;
			while (mot.ContenusPrecedents.Contains(listeMotsPossibles[j].contenu) && nbEchange < nbMotsPossibles) {
				//Debug.Log("Switch sur " + listeMotsPossibles[j].contenu);
				MotDico motPossible = listeMotsPossibles[j];
				listeMotsPossibles.RemoveAt(j);
				listeMotsPossibles.Add(motPossible);
				nbEchange++;
			}
			if (nbEchange >= nbMotsPossibles) {
				//Debug.Log("Clear");
				mot.ContenusPrecedents.Clear();
			}
			mot.EnregistrerMot(listeMotsPossibles[j], bd);
			if (mot.ExistentMotsTransversaux(bd)) {
				//Debug.Log("Ai écris " + mot.Contenu);
				mot.MarquerCommeRempli(listeMotsPossibles[j], bd, true);
				if (afficher) {
					mot.AfficherMot();
				}
				break;
			} else {
				//Debug.Log("Ai tenté d'écrire " + mot.Contenu + " mais pas de mots transversaux existants");
				mot.EffacerMot(bd);
			}
		}
	}

	/// <summary>
	/// Retire un mot spécifié de la grille
	/// </summary>
	/// <param name="motARetirer"></param>
	/// <param name="grille"></param>
	/// <param name="bd"></param>
	/// <param name="afficher"></param>
	public void RetirerMot(Mot motARetirer, Grille grille, Bd bd, bool afficher) {
		if (motARetirer != null) {
			motARetirer.EffacerMot(bd);
			if (afficher) {
				motARetirer.AfficherMot();
			}
			grille.listeMotsARemplir.Add(motARetirer);
			motARetirer.CalculerScore();
			motARetirer.CalculerScoreDesMotsTransversaux();
		}
	}

	/// <summary>
	/// Retire de la grille tous les transversaux d'un mot spécifié
	/// </summary>
	/// <param name="grille"></param>
	/// <param name="motCourant"></param>
	/// <param name="bd"></param>
	/// <param name="afficher"></param>
	public void RetirerMotsTransversaux(Grille grille, Mot motCourant, Bd bd, bool afficher) {
		foreach (Lettre lettre in motCourant.ListeLettres) {
			RetirerMot(lettre.ObtenirMotDansDirection(!motCourant.Horizontal), grille, bd, afficher);
		}
	}

	/// <summary>
	/// Retire un mot aléatoire de la grille
	/// </summary>
	/// <param name="listeMots"></param>
	/// <param name="bd"></param>
	/// <param name="afficher"></param>
	/// <returns></returns>
	public void RetirerMotAleatoire(Grille grille, Mot motCourant, Bd bd, bool afficher) {
		RetirerMot(ObtenirMotTransversalOuAdjacentAleatoire(motCourant), grille, bd, afficher);
	}

	/// <summary>
	/// Retourne un mot transversal (ou adjacent) au mot spécifié
	/// </summary>
	/// <param name="motCourant"></param>
	/// <returns></returns>
	public Mot ObtenirMotTransversalOuAdjacentAleatoire(Mot motCourant) {
		Mot motARetirer = motCourant.ObtenirMotTransversalRempliAleatoire();
		if (motARetirer == null) {
			return motCourant.ObtenirMotAdjacentRempliAleatoire();
		}
		return motARetirer;
	}

	/// <summary>
	/// Retourne un mot aléatoire de la grille
	/// </summary>
	/// <param name="listeMots"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public Mot ObtenirMotAleatoire(List<Mot> listeMots) {
		List<Mot> listeTemp = listeMots.Where(o => o.Rempli).ToList();
		return listeTemp[Random.Range(0, listeTemp.Count - 1)];
	}
}
