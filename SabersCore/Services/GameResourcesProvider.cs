using System;
using System.Linq;
using UnityEngine;
using Zenject;
using static BGLib.UnityExtension.AddressablesExtensions;
using static UnityEngine.Object;
using Object = UnityEngine.Object;

namespace SabersCore.Services;

internal class GameResourcesProvider : IInitializable
{
    private readonly DiContainer container;
    private readonly SaberTrailRenderer trailRendererPrefab;
    private readonly GameObject saberModelPrefab;

    private GameResourcesProvider(DiContainer container)
    {
        this.container = container;
        trailRendererPrefab = LoadAsset<SaberTrailRenderer>("Assets/Prefabs/Effects/Sabers/SaberTrailRenderer.prefab");
        saberModelPrefab = LoadPrefab("Assets/Prefabs/Sabers/BasicSaberModel.prefab");
    }

    public Material DefaultTrailMaterial => trailRendererPrefab._meshRenderer.material;
    
    public SaberTrailRenderer CreateNewSaberTrailRenderer() => 
        container.InstantiatePrefabForComponentAt<SaberTrailRenderer>(
            trailRendererPrefab,
            Vector3.zero, Quaternion.identity,
            null);

    public GameObject CreateNewDefaultSaber()
    {
        var saberObject = Instantiate(saberModelPrefab, Vector3.zero, Quaternion.identity);
        saberObject.name = "NewSaberModel";
        foreach (var x in saberObject.GetComponentsInChildren<SetSaberGlowColor>()) x.enabled = false;
        foreach (var x in saberObject.GetComponentsInChildren<SetSaberFakeGlowColor>()) x.enabled = false;
        saberObject.GetComponent<SaberTrail>().enabled = false;
        return saberObject;
    }

    public void Initialize()
    {
        trailRendererPrefab._meshRenderer = trailRendererPrefab.GetComponent<MeshRenderer>();
        trailRendererPrefab._meshFilter = trailRendererPrefab.GetComponent<MeshFilter>();
    }

    private static Exception ResourceException => new InvalidOperationException("An internal resource failed to load");
    
    private static GameObject LoadPrefab(object label) =>
        LoadContent<GameObject>(label).FirstOrDefault() ?? throw ResourceException;
    private static T LoadAsset<T>(object label) where T : Object => 
        LoadContent<GameObject>(label).FirstOrDefault()?.GetComponent<T>() ?? throw ResourceException;
}
