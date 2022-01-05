using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class Exporteur {
	/// <summary>
	/// Sauvegarde la grille dans un fichier
	/// </summary>
	/// <param name="grille"></param>
	/// <param name="nom"></param>
	public void SauvegarderGrille(Grille grille, bool avecSolution) {
		string destination = "";
		if (avecSolution) {
			destination = Application.persistentDataPath + "/GrillesPleines/";
		} else {
			destination = Application.persistentDataPath + "/GrillesVides/";
		}

		int idNumber = Directory.GetFiles(destination, "*", SearchOption.TopDirectoryOnly).Length + 1;
		string nomFichier = "Grille" + idNumber + ".dat";
		FileStream file;
		if (File.Exists(destination)) {
			Debug.Log("ERREUR: Une grille portant le ID généré existe déjà.");
			return;
		} else {
			file = File.Create(destination + nomFichier);
		}
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, new GrilleSerializable(grille, avecSolution));
		file.Close();
		Debug.Log("Grille sauvegardée sous " + nomFichier);
	}

	/// <summary>
	/// Charge une grille depuis un fichier
	/// </summary>
	/// <param name="nom"></param>
	/// <returns></returns>
	public GrilleSerializable ChargerGrille(bool avecSolution, int idFichier) {
		string destination = "";
		if (avecSolution) {
			destination = Application.persistentDataPath + "/GrillesPleines/Grille" + idFichier + ".dat";
		} else {
			destination = Application.persistentDataPath + "/GrillesVides/Grille" + idFichier + ".dat";
		}
		FileStream file;
		if (File.Exists(destination)) {
			file = File.OpenRead(destination);
		} else {
			Debug.LogError("File not found");
			return null;
		}
		BinaryFormatter bf = new BinaryFormatter();
		GrilleSerializable grilleSerialisable = (GrilleSerializable)bf.Deserialize(file);
		file.Close();
		return grilleSerialisable;
	}
}

[System.Serializable]
public class GrilleSerializable {
	public int nbLignes;
	public int nbColonnes;
	public string[,] listeLettres;

	public GrilleSerializable(Grille grille, bool avecSolution) {
		this.nbLignes = grille.nbLignes;
		this.nbColonnes = grille.nbColonnes;
		listeLettres = new string[grille.nbLignes, grille.nbColonnes];
		for (int y = 0; y < grille.nbLignes; y++) {
			for (int x = 0; x < grille.nbColonnes; x++) {
				if (avecSolution) {
					listeLettres[x, y] = grille.listeLettres[x, y].valeur;
				} else {
					if (grille.listeLettres[x, y].valeur != null) {
						listeLettres[x, y] = ".";
					}
				}
			}
		}
	}

	/// <summary>
	/// Retourne la grille sous forme de string
	/// </summary>
	/// <returns></returns>
	public string ObtenirGridAsString() {
		StringBuilder gridAsStringBuilder = new StringBuilder();
		for (int y = 0; y < nbLignes; y++) {
			for (int x = 0; x < nbColonnes; x++) {
				if (listeLettres[x, y] == null) {
					gridAsStringBuilder.Append("0");
				} else {
					gridAsStringBuilder.Append("1");
				}
			}
			if (y < (nbLignes - 1)) {
				gridAsStringBuilder.AppendLine();
			}
		}
		return gridAsStringBuilder.ToString();
	}

	/// <summary>
	/// Rempli la grille fournie avec les données de l'objet GrilleSerializable
	/// </summary>
	/// <param name="grille"></param>
	/// <param name="bd"></param>
	public void RemplirGrille(Grille grille, Bd bd) {
		foreach (Lettre lettre in grille.listeLettres) {
			if (listeLettres[lettre.X, lettre.Y] != null) {
				lettre.EnregistrerLettre(listeLettres[lettre.X, lettre.Y]);
				lettre.AfficherLettre();
			}
		}
		foreach (Mot mot in grille.listeMots) {
			mot.MarquerCommeRempli(bd.RechercherMotParContenu(mot.Contenu), bd, true);
		}
	}
}