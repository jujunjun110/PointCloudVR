using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public static class PtsReader {
    public static List<(Vector3, Vector3)> Load(TextAsset ptsfile) {
        var rows = ptsfile.text.Split('\n');
        return rows.Where(s => s != "").Select(row => parseRow(row)).ToList();
    }

    private static (Vector3, Vector3) parseRow(string row) {
        var splitted = row.Split(' ');
        return (new Vector3(
            float.Parse(splitted[0]),
            float.Parse(splitted[2]), // Z-up -> Y-up
            float.Parse(splitted[1])  // Z-up -> Y-up
        ), new Vector3(
            float.Parse(splitted[3]),
            float.Parse(splitted[4]),
            float.Parse(splitted[5])
        ));
    }
}
