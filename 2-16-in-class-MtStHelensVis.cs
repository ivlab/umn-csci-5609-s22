/* MtStHelensVis.cs
 * CSCI 5609
 *
 * Copyright (c) 2022 University of Minnesota
 * Authors: Bridger Herman <herma582@umn.edu>
 *
 */

using UnityEngine;
using IVLab.ABREngine;
using IVLab.Utilities;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class MtStHelensVis : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Things you may need to access to complete the assignment

    // Constants for data files
    private string dataFilePath;
    public const string beforeFileName = "beforeGrid240x346.csv";
    public const string afterFileName = "afterGrid240x346.csv";
    public const int gridX = 240;
    public const int gridY = 346;

    // Lists of 3D points for you to access the point data for before/after the eruption
    private List<Vector3> beforePointList;
    private List<Vector3> afterPointList;

    // Bounding box of the above lists
    private Bounds pointsBounds;

    // Place to store the elevation change from before to after points
    private Dictionary<string, List<float>> differenceVar = new Dictionary<string, List<float>>();

    // List to store all data impressions (per vis method) in
    private List<List<IDataImpression>> impressionsPerMethod = new List<List<IDataImpression>>();

    //
    ////////////////////////////////////////////////////////////////////////////////

    // Keep track of which vis mode we're currently displaying!
    // You shouldn't need to mess with these.
    private int currentVisMode = 1;
    bool isFirstFrame = true;

    // Method 1: Basic point visualization -- partially implemented for you!
    //
    // The general process is:
    //  1. Obtain a RawDataset by using the RawDatasetAdapter
    //  2. Import the raw dataset into ABR
    //  3. Create your data impression(s) and style them with size, color, texture, etc.
    //  4. Register the data impressions with the ABREngine
    //  5. Save the impressions into this script so we can easily turn them on/off
    void PrepareMethod1()
    {
        // Convert the point lists into ABR format
        RawDataset beforePointsRaw = RawDatasetAdapter.PointsToPoints(beforePointList, pointsBounds, null, null);
        RawDataset afterPointsRaw = RawDatasetAdapter.PointsToPoints(afterPointList, pointsBounds, null, null);

        // Import the point data into ABR
        KeyData before = ABREngine.Instance.Data.ImportRawDataset(beforePointsRaw);
        KeyData after = ABREngine.Instance.Data.ImportRawDataset(afterPointsRaw);

        // Create a layer for before points (blue)
        SimpleGlyphDataImpression di = new SimpleGlyphDataImpression();
        di.useRandomOrientation = false;
        di.keyData = before;
        di.glyph = ABREngine.Instance.VisAssets.GetDefault<GlyphVisAsset>() as GlyphVisAsset;
        di.glyphSize = 0.002f;
        di.colormap = ColormapVisAsset.SolidColor(Color.blue);

        // Create a layer for after points (red)
        SimpleGlyphDataImpression di2 = new SimpleGlyphDataImpression();
        di2.useRandomOrientation = false;
        di2.keyData = after;
        di2.glyph = ABREngine.Instance.VisAssets.GetDefault<GlyphVisAsset>() as GlyphVisAsset;
        di2.glyphSize = 0.002f;
        di2.colormap = ColormapVisAsset.SolidColor(Color.red);

        // Register impressions with the engine
        ABREngine.Instance.RegisterDataImpression(di);
        ABREngine.Instance.RegisterDataImpression(di2);

        // Add a reference to these impressions so we can easily turn them on/off later
        impressionsPerMethod.Add(new List<IDataImpression> {di, di2});
    }

    // Method 2: Surface with a colormap
    void PrepareMethod2()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // TODO: Add your code here to create a surface w/colormap
        ////////////////////////////////////////////////////////////////////////////////
        RawDataset abrBeforePoints = RawDatasetAdapter.GridPointsToSurface(
            beforePointList,
            new Vector2Int(gridX, gridY),
            pointsBounds,
            differenceVar
        );

        KeyData pointKD = ABREngine.Instance.Data.ImportRawDataset(abrBeforePoints);

        SimpleSurfaceDataImpression di = new SimpleSurfaceDataImpression();
        di.keyData = pointKD;
        di.colorVariable = pointKD.GetScalarVariables()[0];
        di.colormap = ABREngine.Instance.VisAssets.GetDefault<ColormapVisAsset>() as ColormapVisAsset;

        ABREngine.Instance.RegisterDataImpression(di);

        // Add a reference to these impressions so we can easily turn them on/off later
        impressionsPerMethod.Add(new List<IDataImpression> {di});
    }

    // Method 3: Your choice!
    void PrepareMethod3()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // TODO: Add your code here to create a vis of your choice!
        ////////////////////////////////////////////////////////////////////////////////
    }

    // Method 4: Above and beyond!
    void PrepareMethod4()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // TODO: If you're going above and beyond, add another method here to create a vis of your choice!
        ////////////////////////////////////////////////////////////////////////////////
    }

    // Method 5: Above and beyond!
    void PrepareMethod5()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // TODO: If you're going above and beyond, add another method here to create a vis of your choice!
        ////////////////////////////////////////////////////////////////////////////////
    }

    // Start is run when you press 'Play' in Unity - this is similar to
    // 'setup()' in Processing.
    void Start()
    {
        // Wait for ABREngine to initialize
        while (!ABREngine.Instance.IsInitialized);

        // Set the data file path to StreamingAssets
        dataFilePath = Application.streamingAssetsPath;

        // Start by loading in the data from CSV files.
        LoadData();

        // Prepare each method here
        PrepareMethod1();
        PrepareMethod2();
        PrepareMethod3();
        // PrepareMethod4();
        // PrepareMethod5();
    }

    // Update is run each frame -- this is similar to Processing's 'draw()' function
    void Update()
    {
        // If the user presses the '1' key, '2' key, '3' key, etc...
        int oldVisMode = currentVisMode;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentVisMode = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            currentVisMode = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            currentVisMode = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            currentVisMode = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            currentVisMode = 5;

        // Set every layer to invisible if the user has pressed any key (need to
        // re-render the visualization with new layers)
        bool modeChanged = oldVisMode != currentVisMode;
        if (modeChanged || isFirstFrame)
        {
            foreach (var impression in ABREngine.Instance.GetAllDataImpressions())
            {
                impression.RenderHints.Visible = false;
            }
            Debug.Log("Showing comparison mode " + currentVisMode);
        }

        // Turn on the impressions associated with this mode
        if (currentVisMode - 1 < impressionsPerMethod.Count)
        {
            foreach (var impression in impressionsPerMethod[currentVisMode - 1])
            {
                impression.RenderHints.Visible = true;
            }
        }

        // Re-render the visualization if user pressed a key
        if (modeChanged || isFirstFrame)
        {
            ABREngine.Instance.Render();
            isFirstFrame = false;
        }
    }

    // Load the data from CSV files into point lists for you to access, and load into ABR data format.
    void LoadData()
    {
        // Load in the raw coordinates from CSV (convert from right-hand z-up to unity's left-hand y-up)
        CoordConversion.CoordSystem rhZUp = new CoordConversion.CoordSystem(
            CoordConversion.CoordSystem.Handedness.RightHanded,
            CoordConversion.CoordSystem.Axis.PosZ,
            CoordConversion.CoordSystem.Axis.PosY
        );
        beforePointList = CSVToPoints.LoadFromCSV(Path.Combine(dataFilePath, beforeFileName), rhZUp);
        afterPointList = CSVToPoints.LoadFromCSV(Path.Combine(dataFilePath, afterFileName), rhZUp);

        // Find the data bounds
        pointsBounds = new Bounds(beforePointList[0], Vector3.zero);
        foreach (var pt in beforePointList)
        {
            pointsBounds.Encapsulate(pt);
        }

        if (beforePointList.Count != afterPointList.Count)
        {
            Debug.LogError("Before and After points must have same length!");
            return;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // TODO: Calculate the elevation change between before and after points!
        ////////////////////////////////////////////////////////////////////////////////
        List<float> differences = new List<float>();
        for (int i = 0; i < beforePointList.Count; i++)
        {
            // or, use afterPointList[i].y - beforePointList[i].y
            Vector3 diff = afterPointList[i] - beforePointList[i];
            float elevationChange = Vector3.Dot(diff, Vector3.up);
            differences.Add(elevationChange);
        }

        // This is how you can save the elevation change you calculate above so
        // you can use it in the RawDatasetAdapter:
        differenceVar = new Dictionary<string, List<float>> {{ "difference", differences }};
    }
}
