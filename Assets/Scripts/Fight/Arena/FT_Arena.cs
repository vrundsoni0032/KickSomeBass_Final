using UnityEngine;

namespace Fight.Arena
{
public class FT_Arena : MonoBehaviour
{
    [SerializeField] private float m_arenaDiameterInMeter;
    [SerializeField] GameObject[] m_spawnPointPairs;//spawn points are stored in pair ,the points in a pair are in front of each other.
    [SerializeField] LayerMask m_obstacleMask;
    [SerializeField] Transform m_arenaCenter;
    //Arena type and soil type at some point could be placed here.


    public Vector3[] GetRandomSpawnPoints()
    {
        if (m_spawnPointPairs == null)
        {
            return null;
        }

        Vector3[] spawnPoints = new Vector3[2];

        int randIndex=Random.Range(0,m_spawnPointPairs.Length-1);

        if(randIndex%2==0)
        {
            spawnPoints[0] = m_spawnPointPairs[randIndex].transform.position;
            spawnPoints[1] = m_spawnPointPairs[randIndex+1].transform.position;
        }
        else
        {
            spawnPoints[0] = m_spawnPointPairs[randIndex].transform.position;
            spawnPoints[1] = m_spawnPointPairs[randIndex-1].transform.position;
        }

        return spawnPoints;
    }
    public Vector3 GetArenaCenter() { return m_arenaCenter.position; }
    public float GetArenaDiameterInMeter() { return m_arenaDiameterInMeter; }
    public LayerMask GetObstacleMask() { return m_obstacleMask; }
}
}