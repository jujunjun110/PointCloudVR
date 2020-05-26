# PointCloud VR
![PointCloudVR](./Public/pointcloudvr.gif)

点群データをUnityで表示するプロジェクト。

解説は[こちらの記事](https://note.com/jujunjun110/n/nddc6da415ae9)



## ファイル構成
（主要ファイルのみ）

```
Assets/
├── Data
│   ├── shiraito.txt # 点群データ
│   └── ...
├── Prefabs
│   └── PointCloudAnchor.prefab # これをシーン内に配置して利用
├── Scenes
│   └── SampleScene.unity
├── Scipts
│   ├── ControllerManager.cs
│   ├── PointCloudLoader.cs # 点群データをバッファにセットするスクリプト
│   └── PtsReader.cs # 点群データファイルを読み込んでパースするスクリプト
└── Shaders
    └── PointCloud.shader # 点群を描画するシェーダー
```
