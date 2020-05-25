using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class PtsReader {
    async public static Task<List<(Vector3, Vector3)>> Load(TextAsset ptsfile) {
        var content = ptsfile.text;

        // テキストファイルの読み込みとパースがボトルネックなのでいずれ最適化したい
        // 現状はとりあえず非同期読み込みにしてメインスレッドがブロックされることを回避
        return await Task.Run(() =>
            content.Split('\n').Where(s => s != "").Select(parseRow).ToList()
        );
    }

    private static (Vector3, Vector3) parseRow(string row) {
        var splitted = row.Split(' ').Select(float.Parse).ToList();

        return (new Vector3(
            splitted[0],
            splitted[2], // PTSファイルは通常Z-upなので、ここでZとYを交換しY-upに変換
            splitted[1]
        ), new Vector3(
            splitted[3],
            splitted[4],
            splitted[5]
        ));
    }
}
