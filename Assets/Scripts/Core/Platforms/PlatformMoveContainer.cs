using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class PlatformMoveContainer : MonoBehaviour
{
    [SerializeField] private Transform[] arrayPoints;
    [SerializeField] private MoveType moveType;
    [SerializeField] private GameObject[] platforms;
    [SerializeField] float timeDelay;
    [SerializeField] float timeMove;
    private void Start()
    {
        Vector3[] pathPoints = arrayPoints.Select(t => t.position).ToArray();
        StartCoroutine(Move(moveType, pathPoints));
    }
    public IEnumerator Move(MoveType moveType, Vector3[] pathPoints )
    {
        if (arrayPoints.Length < 2) yield break;
        for (int i = 0; i < platforms.Length; i++)
        {
            float distance = Vector2.Distance(platforms[i].transform.position, arrayPoints[0].position);
            float time = CalTimePerUnityUnit() * distance;
            var gameObject = platforms[i];
            platforms[i].transform.DOMove(pathPoints[0], time).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (moveType == MoveType.Repeat)
                {
                    Move(Ease.Linear, LoopType.Restart, pathPoints, gameObject);
                }
                else if (moveType == MoveType.Sequence)
                {
                    Move(Ease.Linear, LoopType.Yoyo, pathPoints, gameObject);
                }
            });


            yield return new WaitForSeconds(timeDelay);
        }
    }
    private void Move(Ease easeMode, LoopType loopMode, Vector3[] pathPoints, GameObject obj)
    {
        obj.transform.DOPath(pathPoints, timeMove, PathType.Linear)
            .SetEase(easeMode)
            .SetLoops(-1, loopMode);
    }

    private void OnDestroy()
    {
        foreach (var platform in platforms)
        {
            if (platform != null)
                DOTween.Kill(platform.transform);
        }
    }
    public float CalTimePerUnityUnit()
    {
        float time = timeMove / (arrayPoints.Length  * Vector2.Distance(arrayPoints[1].position, arrayPoints[0].position));
        Debug.Log(time);
        return time;

    }
    


}
public enum MoveType
{
    Repeat,
    Sequence,
}
