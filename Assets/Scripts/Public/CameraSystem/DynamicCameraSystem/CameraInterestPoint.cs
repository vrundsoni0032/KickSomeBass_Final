using System.Collections;
using UnityEngine;

namespace Core.CameraSystem
{
public class CameraInterestPoint : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] float m_priorityScore;
    float m_orignalPriorityScore;

    private void Start()
    {
        m_orignalPriorityScore = m_priorityScore;
    }
    public float GetPriorityScore() { return m_priorityScore; }


    IEnumerator ResetVisited()
    {
        yield return new WaitForSeconds(5);
        m_priorityScore = m_orignalPriorityScore;
    }
    private void OnTriggerExit(Collider collision)
    {
        //if(collision.GetComponent<BaseCamera>()!=null)
        //{
        //    m_priorityScore=-1;//Disable POI
        //   StartCoroutine(ResetVisited());
        //}
    }
    public void MarkVisited()
    {
        m_priorityScore = -1;//Disable POI
        StartCoroutine(ResetVisited());
    }
}
}