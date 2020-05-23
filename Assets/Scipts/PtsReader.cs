using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public static class PtsReader {
    public static List<(Vector3, Vector3)> Load(TextAsset csv) {
        var rows = csv.text.Split('\n');
        return rows.Where(s => s != "").Select(row => parseRow(row)).ToList();
    }
    public static List<(Vector3, Vector3)> LoadWithPath(string path = null) {
        var csv = Resources.Load(path, typeof(TextAsset)) as TextAsset;
        return Load(csv);
    }

    private static (Vector3, Vector3) parseRow(string row) {
        var parsed = row.Split(' ');
        return (new Vector3(
            float.Parse(parsed[0]),
            float.Parse(parsed[2]), // Z-up -> Y-up
            float.Parse(parsed[1])  // Z-up -> Y-up
        ), new Vector3(
            float.Parse(parsed[3]),
            float.Parse(parsed[4]),
            float.Parse(parsed[5])
        ));
    }
}
