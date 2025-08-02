using UnityEngine;

namespace SabersCore.Services;

public interface ICustomSaberEventManagerHandler
{
    void InitializeEventManager(GameObject customSaberObject, SaberType saberType);
}