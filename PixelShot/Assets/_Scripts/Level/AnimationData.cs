using System.Collections.Generic;
using UnityEngine;
using LoopType = DG.Tweening.LoopType;
using EaseType = DG.Tweening.Ease;

[System.Serializable]
public class AnimationData 
{
    public AnimationType animationType;
    public List<Vector3> animationPoints;
    public float animationDuration;
    public Vector3 animationParentLocation;
    public LoopType loopType;
    public EaseType easeType;

    public List<int> pixelIDs;
}
