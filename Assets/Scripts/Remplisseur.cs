using System.Collections.Generic;
using UnityEngine;

public class Remplisseur {

	private List<MotDico> listeMotsPossibles;
	int i = 0;

	public void RemplirGrille(Bd bd, List<Mot> listeMots, int nbEssaisMaxGlobal, int nbEssaisMaxPourMot, bool afficher) {
		for (int max = listeMots.Count, nbEssaisGlobal = 0; i < max && nbEssaisGlobal < nbEssaisMaxGlobal; i++, nbEssaisGlobal++) {
			if (!listeMots[i].Rempli) {
				listeMotsPossibles = bd.ListeMotsPossibles(listeMots[i].Contenu);
				//Debug.Log("listeMotsPossibles.Count: " + listeMotsPossibles.Count);
				for (int j = 0, nbMotsPossibles = listeMotsPossibles.Count; j < nbEssaisMaxPourMot && j < nbMotsPossibles; j++) {
					//Debug.Log(j);
					int rnd = Random.Range(0, nbMotsPossibles - 1);
					listeMots[i].EnregistrerMot(listeMotsPossibles[rnd], bd);
					if (listeMots[i].ExistentMotsTransversaux(bd)) {
						//Debug.Log("Ai écris " + listeMots[i].Contenu);
						listeMots[i].MarquerCommeRempli(listeMotsPossibles[rnd], bd);
						if (afficher) {
							listeMots[i].AfficherMot();
						}
						break;
					} else {
						//Debug.Log("Ai tenté d'écrire " + listeMots[i].Contenu + " mais pas de mots transversaux existants");
						listeMots[i].EffacerMot(bd);
						listeMotsPossibles.RemoveAt(rnd);
						nbMotsPossibles--;
					}
					/*
					int nbEchange = 0;
					while (listeMots[i].ContenusPrecedents.Contains(listeMotsPossibles[j].contenu) && nbEchange < nbMotsPossibles) {
						//Debug.Log("Switch sur " + listeMotsPossibles[j].contenu);
						MotDico motPossible = listeMotsPossibles[j];
						listeMotsPossibles.RemoveAt(j);
						listeMotsPossibles.Add(motPossible);
						nbEchange++;
					}
					if (nbEchange >= nbMotsPossibles) {
						//Debug.Log("Clear");
						listeMots[i].ContenusPrecedents.Clear();
					}			
					listeMots[i].EnregistrerMot(listeMotsPossibles[j], bd);
					if (listeMots[i].ExistentMotsTransversaux(bd)) {
						//Debug.Log("Ai écris " + listeMots[i].Contenu);
						listeMots[i].MarquerCommeRempli(listeMotsPossibles[j], bd);
						if (afficher) {
							listeMots[i].AfficherMot();
						}
						break;
					} else {
						//Debug.Log("Ai tenté d'écrire " + listeMots[i].Contenu + " mais pas de mots transversaux existants");
						listeMots[i].EffacerMot(bd);
					}
					*/
				}
				if (!listeMots[i].Rempli) {
					if (listeMotsPossibles.Count == 0) {
						//Debug.Log("Pas de mot possible pour " + (listeMots[i].Horizontal?"Horizontal":"Vertical") + " " + listeMots[i].PositionPrimaire + ":" + listeMots[i].PositionSecondaire);
					} else {
						//Debug.Log("Pas de mot transversal pour " + (listeMots[i].Horizontal ? "Horizontal" : "Vertical") + " " + listeMots[i].PositionPrimaire + ":" + listeMots[i].PositionSecondaire);
					}

					Mot motTransversalAleatoire = listeMots[i].ObtenirMotTransversalRempliAleatoire();
					if (motTransversalAleatoire != null) {
						//Debug.Log("Retrait du motTransversalAleatoire " + motTransversalAleatoire.Contenu + " en position " + (motTransversalAleatoire.Horizontal ? "Horizontal" : "Vertical") + " " + motTransversalAleatoire.PositionPrimaire + ":" + motTransversalAleatoire.PositionSecondaire);
						motTransversalAleatoire.EffacerMot(bd);
						if (afficher) {
							motTransversalAleatoire.AfficherMot();
						}
						listeMots.Remove(motTransversalAleatoire);
						listeMots.Insert(i, motTransversalAleatoire);
						i = i - 2;
					} else {
						Mot motAdjacentAleatoire = listeMots[i].ObtenirMotAdjacentRempliAleatoire();
						if (motAdjacentAleatoire != null) {
							//Debug.Log("Retrait du motAdjacentAleatoire " + motAdjacentAleatoire.Contenu + " en position " + (motAdjacentAleatoire.Horizontal ? "Horizontal" : "Vertical") + " " + motAdjacentAleatoire.PositionPrimaire + ":" + motAdjacentAleatoire.PositionSecondaire);
							motAdjacentAleatoire.EffacerMot(bd);
							if (afficher) {
								motAdjacentAleatoire.AfficherMot();
							}
							listeMots.Remove(motAdjacentAleatoire);
							listeMots.Insert(i, motAdjacentAleatoire);
							i = i - 2;
						} else {
							//Debug.Log("Retrait aléatoire... ");
							if (i == 0) {
								Debug.Log("Erreur critique");
								break;
							}
							Mot motAleatoire = listeMots[Random.Range(0, (i - 1))];
							//Debug.Log("Retrait du motAleatoire " + motAleatoire.Contenu + " en position " + (motAleatoire.Horizontal ? "Horizontal" : "Vertical") + " " + motAleatoire.PositionPrimaire + ":" + motAleatoire.PositionSecondaire);
							motAleatoire.EffacerMot(bd);
							if (afficher) {
								motAleatoire.AfficherMot();
							}
							listeMots.Remove(motAleatoire);
							listeMots.Insert(i, motAleatoire);
							break;
						}
					}
				}
			}
		}
	}
}
