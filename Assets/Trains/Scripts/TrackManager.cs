using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackManager : MonoBehaviour
{
    [SerializeField]
    public TrackData[] tracks;

    public int trackDensity = 10;
    private int trackCount = 10;
    private float trackLength = 5;
    public GameObject obj;


    public float t;
    public int trackNum;
    // Start is called before the first frame update
    void Start()
    {
        createTracks();
        generateTrack();
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t>1)
        {
            t = 0;
            trackNum = (trackNum == trackCount-1)?0:trackNum+1;
        }
        obj.GetComponent<Transform>().position = tracks[trackNum].getPoint(t) + Vector3.up;
    }

    private void createTracks()
    {
        float start = 0;
        tracks = new TrackData[trackCount];
        for(int i = 0; i < trackCount; i++)
        {
            tracks[i] = new TrackData(new Vector3(start,0,0),new Vector3(start+trackLength/2,0,3),new Vector3(start+trackLength,0,0));
            start += trackLength;
        }
    }

    private void generateTrack()
    {
        foreach (TrackData track in tracks)
        {
            for(int i = 0; i < trackDensity; i++)
            {
                GameObject o = Instantiate(obj);
                o.GetComponent<Transform>().position = track.getPoint(i*.1f);
            }
        }
    }
}
