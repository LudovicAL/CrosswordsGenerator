using System.Collections.Generic;
using UnityEngine;

public class Remplisseur {
	private List<MotDico> listeMotsPossibles;
	private int i = 0;

	public void Initialiser() {
		i = 0;
	}

	public void RemplirGrille(Bd bd, List<Mot> listeMots, int nbEssaisMaxGlobal, int nbEssaisMaxPourMot, bool afficher) {
		for (int max = listeMots.Count, nbEssaisGlobal = 0; i < max && nbEssaisGlobal < nbEssaisMaxGlobal; i++) {
			if (!listeMots[i].Rempli) {
				nbEssaisGlobal++;
				//Debug.Log("Vais remplir: " + listeMots[i].PositionPrimaire + ":" + listeMots[i].PositionSecondaire);
				//RemplirMot(listeMots[i], bd, nbEssaisMaxPourMot, afficher);
				RemplirMotSelonScore(listeMots[i], bd, nbEssaisMaxPourMot, afficher);
				if (!listeMots[i].Rempli) {
					RetirerMotAleatoire(listeMots, bd, afficher);
				}
			}
		}
	}

	public void RemplirMot(Mot mot, Bd bd, int nbEssaisMaxPourMot, bool afficher) {
		listeMotsPossibles = bd.ListeMotsPossibles(mot.Contenu);
		for (int j = 0, nbMotsPossibles = listeMotsPossibles.Count; j < nbEssaisMaxPourMot && j < nbMotsPossibles; j++) {
			int rnd = Random.Range(0, nbMotsPossibles - 1);
			mot.EnregistrerMot(listeMotsPossibles[rnd], bd);
			if (mot.ExistentMotsTransversaux(bd)) {
				//Debug.Log("Ai écris " + mot.Contenu);
				mot.MarquerCommeRempli(listeMotsPossibles[rnd], bd);
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
				mot.MarquerCommeRempli(listeMotsPossibles[j], bd);
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

	public void RetirerMotAleatoire(List<Mot> listeMots, Bd bd, bool afficher) {
		Mot motARetirer = ObtenirMotTransversalOuAdjacentAleatoire(listeMots[i]);
		if (motARetirer == null) {
			motARetirer = ObtenirMotAleatoire(listeMots, i);
		}
		//Debug.Log("Retrait du mot Aleatoire " + motARetirer.Contenu + " en position " + (motARetirer.Horizontal ? "Horizontal" : "Vertical") + " " + motARetirer.PositionPrimaire + ":" + motARetirer.PositionSecondaire);
		motARetirer.EffacerMot(bd);
		if (afficher) {
			motARetirer.AfficherMot();
		}
		listeMots.Remove(motARetirer);
		listeMots.Insert(i, motARetirer);
		i = i - 2;
	}

	public Mot ObtenirMotTransversalOuAdjacentAleatoire(Mot motCourant) {
		Mot motARetirer = motCourant.ObtenirMotTransversalRempliAleatoire();
		if (motARetirer == null) {
			return motCourant.ObtenirMotAdjacentRempliAleatoire();
		}
		return motARetirer;
	}

	public Mot ObtenirMotAleatoire(List<Mot> listeMots, int index) {
		if (index < 2) {
			Debug.Log("Erreur critique: aucun mot disponible pour retrait");
			return null;
		}
		return listeMots[Random.Range(0, (i - 1))];
	}
}
