using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ThrowInfo
{
    public RollState result;
    public float radius;
    public float depth;
    public float massInGrams;
    public bool hasValidData;
}

public struct CoinInfo
{
    public Resizer OurResizer;
    public Roll OurRoll;
    public Rigidbody OurRB;
    public GameObject OurCoin;
}
[System.Serializable]
public struct TossLocationInfo
{
    public Vector3 Offset;
    public bool InUse;
    public float NextUse;
}

public class RollManager : MonoBehaviour {

    public GameObject CoinPrefab;

    public int NumTosses;
    public float SpawnLocationDelay;

    public List<TossLocationInfo> TossLocations;

    private List<CoinInfo> CoinsInfo;

    [SerializeField]
    private List<ThrowInfo> Tosses;

    public int NumHeads;
    public int NumTails;
    public int NumEdge;

	// Use this for initialization
	void Start () {
        CoinsInfo = new List<CoinInfo>();
        Tosses = new List<ThrowInfo>();
        NumHeads = NumTails = NumEdge = 0;
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0;i < TossLocations.Count;i++)
        {
            TossLocationInfo info = TossLocations[i];
            if(info.InUse && info.NextUse < Time.time)
            {
                info.InUse = false;
                TossLocations[i] = info;
            }
        }
        if (CoinsInfo.Count > 0)
        {
            for(int i = CoinsInfo.Count - 1;i >= 0;i--)
            {
                CoinInfo Info = CoinsInfo[i];
                if(Info.OurRB)
                {
                    if(Info.OurRB.IsSleeping())
                    {
                        HandleCoinCleanup(Info);
                    }
                }
            }
        }
        HandleCoinToss();
	}

    void HandleCoinCleanup(CoinInfo InCoinInfo)
    {
        ThrowInfo TossResults = new ThrowInfo();
        if(InCoinInfo.OurRoll && InCoinInfo.OurResizer)
        {
            TossResults.result = InCoinInfo.OurRoll.GetRollstate();
            TossResults.radius = InCoinInfo.OurResizer.GetRadius();
            TossResults.depth = InCoinInfo.OurResizer.GetDepth();
            TossResults.hasValidData = true;
            switch(TossResults.result)
            {
                case RollState.Edge:
                    NumEdge++;
                    break;
                case RollState.Heads:
                    NumHeads++;
                    break;
                case RollState.Tails:
                    NumTails++;
                    break;
            }
        }
        else
        {
            if(!InCoinInfo.OurRoll)
            {
                Debug.LogError("Invalid Current Roll Component.");
            }
            if(!InCoinInfo.OurResizer)
            {
                Debug.LogError("Invalid Current Resizer Component.");
            }
        }
        if(TossResults.hasValidData)
        {
            Tosses.Add(TossResults);
        }
        else
        {
            Debug.LogError("Invalid Toss Data.");
        }
        if(InCoinInfo.OurCoin)
        {
            Destroy(InCoinInfo.OurCoin);
            CoinsInfo.Remove(InCoinInfo);
        }
    }

    void HandleCoinToss()
    {
        if (CoinPrefab && Tosses.Count < NumTosses)
        {
            List<TossLocationInfo> NotInUse = TossLocations.FindAll(delegate (TossLocationInfo Info) { return Info.InUse == false; });
            if (NotInUse.Count > 0)
            {
                int index = Random.Range(0, NotInUse.Count - 1);
                index = TossLocations.IndexOf(NotInUse[index]);
                TossLocationInfo Info = TossLocations[index];
                Info.InUse = true;
                Info.NextUse = Time.time + SpawnLocationDelay;
                TossLocations[index] = Info;
                CoinInfo TempInfo;
                TempInfo.OurCoin = Instantiate(CoinPrefab, transform.position + Info.Offset, Quaternion.identity);
                TempInfo.OurRB = TempInfo.OurCoin.GetComponent<Rigidbody>();
                TempInfo.OurRoll = TempInfo.OurCoin.GetComponent<Roll>();
                TempInfo.OurResizer = TempInfo.OurCoin.GetComponent<Resizer>();
                CoinsInfo.Add(TempInfo);
            }
        }
    }
}
