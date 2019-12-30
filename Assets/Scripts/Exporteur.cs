using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Exporteur {
	public void SauvegarderGrille(Grille grille, string nom) {
		string destination = Application.persistentDataPath + "/" + nom + ".dat";
		FileStream file;

		if (File.Exists(destination)) {
			file = File.OpenWrite(destination);
		} else {
			file = File.Create(destination);
		}
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, grille);
		file.Close();
	}

	public Grille ChargerGrille(string nom) {
		string destination = Application.persistentDataPath + "/" + nom + ".dat";
		FileStream file;
		if (File.Exists(destination)) {
			file = File.OpenRead(destination);
		} else {
			Debug.LogError("File not found");
			return null;
		}
		BinaryFormatter bf = new BinaryFormatter();
		Grille grille = (Grille)bf.Deserialize(file);
		file.Close();
		return grille;
	}
}