using System;
using System.Linq;
using SaberComponents.Components;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Services;

public class CustomSaberEventManagerHandler : ICustomSaberEventManagerHandler, IDisposable
{
    private readonly BeatmapObjectManager beatmapObjectManager;
    private readonly GameEnergyCounter gameEnergyCounter;
    private readonly ObstacleSaberSparkleEffectManager obstacleCollisionManager;
    private readonly RelativeScoreAndImmediateRankCounter relativeScoreCounter;
    private readonly PlayerHeadAndObstacleInteraction playerHeadAndObstacleInteraction;
    private readonly IScoreController scoreController;
    private readonly IComboController comboController;
    private readonly IReadonlyBeatmapData beatmapData;

    public CustomSaberEventManagerHandler(
        BeatmapObjectManager beatmapObjectManager,
        GameEnergyCounter gameEnergyCounter,
        ObstacleSaberSparkleEffectManager obstacleCollisionManager,
        RelativeScoreAndImmediateRankCounter relativeScoreCounter,
        PlayerHeadAndObstacleInteraction playerHeadAndObstacleInteraction,
        IScoreController scoreController,
        IComboController comboController,
        IReadonlyBeatmapData beatmapData)
    {
        this.beatmapObjectManager = beatmapObjectManager;
        this.gameEnergyCounter = gameEnergyCounter;
        this.obstacleCollisionManager = obstacleCollisionManager;
        this.relativeScoreCounter = relativeScoreCounter;
        this.playerHeadAndObstacleInteraction = playerHeadAndObstacleInteraction;
        this.scoreController = scoreController;
        this.comboController = comboController;
        this.beatmapData = beatmapData;
    }

    private EventManager? eventManager;
    private float? lastNoteTime;
    private float previousScore;
    private int previousCombo;
    private SaberType saberType;

    public void InitializeEventManager(GameObject customSaberObject, SaberType saberType)
    {
        eventManager = customSaberObject.GetComponent<EventManager>();
        if (eventManager == null) return;
        
        this.saberType = saberType;
        lastNoteTime = GetLastNoteTime(beatmapData);
        
        scoreController.multiplierDidChangeEvent += MultiplierChanged;
        beatmapObjectManager.noteWasCutEvent += NoteWasCut;
        beatmapObjectManager.noteWasMissedEvent += NoteWasMissed;
        comboController.comboDidChangeEvent += ComboChanged;
        obstacleCollisionManager.sparkleEffectDidStartEvent += SaberStartedCollision;
        obstacleCollisionManager.sparkleEffectDidEndEvent += SaberEndedCollision;
        gameEnergyCounter.gameEnergyDidReach0Event += LevelWasFailed;
        relativeScoreCounter.relativeScoreOrImmediateRankDidChangeEvent += ScoreChangedEvent;

        eventManager.levelStarted?.Invoke();
    }

    public void Dispose()
    {
        scoreController.multiplierDidChangeEvent -= MultiplierChanged;
        beatmapObjectManager.noteWasCutEvent -= NoteWasCut;
        beatmapObjectManager.noteWasMissedEvent -= NoteWasMissed;
        comboController.comboDidChangeEvent -= ComboChanged;
        obstacleCollisionManager.sparkleEffectDidStartEvent -= SaberStartedCollision;
        obstacleCollisionManager.sparkleEffectDidEndEvent -= SaberEndedCollision;
        gameEnergyCounter.gameEnergyDidReach0Event -= LevelWasFailed;
        relativeScoreCounter.relativeScoreOrImmediateRankDidChangeEvent -= ScoreChangedEvent;
    }

    private void NoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
    {
        if (noteCutInfo.allIsOK && noteCutInfo.saberType == saberType)
        {
            eventManager!.noteCut?.Invoke();
        }

        if (lastNoteTime != null && noteController.noteData.time.Approximately(lastNoteTime.Value))
        {
            lastNoteTime = 0;
            eventManager!.onLevelEnded?.Invoke();
        }
    }

    private void NoteWasMissed(NoteController noteController)
    {
        if (lastNoteTime != null && noteController.noteData.time.Approximately(lastNoteTime.Value))
        {
            lastNoteTime = 0;
            eventManager!.onLevelEnded?.Invoke();
        }
    }

    private void MultiplierChanged(int multiplier, float progress)
    {
        if (multiplier > 1 && progress < 0.1f)
        {
            eventManager!.multiplierUp?.Invoke();
        }
    }

    private void ComboChanged(int combo)
    {
        eventManager!.comboChanged?.Invoke(combo);
        if (combo < previousCombo)
        {
            eventManager.comboBroken?.Invoke();
        }
        previousCombo = combo;
    }

    private void SaberStartedCollision(SaberType saberType)
    {
        eventManager!.saberStartColliding?.Invoke();
    }

    private void SaberEndedCollision(SaberType saberType)
    {
        eventManager!.saberStopColliding?.Invoke();
    }

    private void LevelWasFailed()
    {
        eventManager!.levelFailed?.Invoke();
    }

    private void ScoreChangedEvent()
    {
        var relativeScore = relativeScoreCounter.relativeScore;
        if (Math.Abs(previousScore - relativeScore) > 0f)
        {
            eventManager!.accuracyChanged?.Invoke(relativeScore);
            previousScore = relativeScore;
        }
    }

    private static float GetLastNoteTime(IReadonlyBeatmapData beatmapData) => beatmapData
        .GetBeatmapDataItems<NoteData>(0)
        .LastOrDefault(data => data.colorType != ColorType.None)?.time ?? 0.0f;
}
