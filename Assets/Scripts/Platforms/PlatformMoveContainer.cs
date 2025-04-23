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
        for (int i = 0; i < platforms.Length; i++)
        {
            if (moveType == MoveType.Repeat)
            {
                Move(Ease.Linear, LoopType.Restart, pathPoints, platforms[i]);
            }
            else if(moveType == MoveType.Sequence)
            {
                Move(Ease.Linear, LoopType.Yoyo, pathPoints, platforms[i]);
            }
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
        DOTween.Kill(gameObject);
    }


}
public enum MoveType
{
    Repeat,
    Sequence,
}
