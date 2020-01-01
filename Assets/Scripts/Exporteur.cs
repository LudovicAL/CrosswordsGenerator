using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class Exporteur {
	public void SauvegarderGrille(Grille grille, string nom) {
		string destination = Application.persistentDataPath + "/Grilles/" + nom + ".dat";
		FileStream file;

		if (File.Exists(destination)) {
			file = File.OpenWrite(destination);
		} else {
			file = File.Create(destination);
		}
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, new GrilleSerializable(grille));
		file.Close();
		Debug.Log("Grille sauvegardée sous " + destination);
	}

	public GrilleSerializable ChargerGrille(string nom) {
		string destination = Application.persistentDataPath + "/Grilles/" + nom + ".dat";
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

	public GrilleSerializable(Grille grille) {
		this.nbLignes = grille.nbLignes;
		this.nbColonnes = grille.nbColonnes;
		listeLettres = new string[grille.nbLignes, grille.nbColonnes];
		for (int y = 0; y < grille.nbLignes; y++) {
			for (int x = 0; x < grille.nbColonnes; x++) {
				listeLettres[x, y] = grille.listeLettres[x, y].valeur;
			}
		}
	}

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
			gridAsStringBuilder.AppendLine();
		}
		return gridAsStringBuilder.ToString();
	}

	public void RemplirGrille(Grille grille, Bd bd) {
		foreach (Lettre lettre in grille.listeLettres) {
			if (listeLettres[lettre.X, lettre.Y] != null) {
				lettre.EnregistrerLettre(listeLettres[lettre.X, lettre.Y], bd);
				lettre.MarquerMotRempli(false, bd);
				lettre.MarquerMotRempli(true, bd);
				lettre.AfficherLettre();
			}
		}
	}
}