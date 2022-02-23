using System.Collections.Generic;
using UnityEngine;
using IVLab.ABREngine;
using System.Linq;

public class LinkedViewsInClass : IVLab.Plotting.LinkedData
{
    // Step 1: Set up scene correctly
    public IVLab.Plotting.DataManager plotDataManager;

    // Difference from expected y coordinate
    List<float> diffFromExpected = new List<float>();

    // Actual 3D coordinates of each point
    List<Vector3> points = new List<Vector3>();

    // Bounds of the points (no point should be outside this)
    Bounds pointsBounds;

    // Data impression for holding the points
    SimpleGlyphDataImpression pointsDi;

    // Other variables - lines for context etc
    List<List<Vector3>> lineSegments = new List<List<Vector3>>();
    SimpleLineDataImpression lineDi;
    bool needToRender = false;

    void Start()
    {
        LoadData();

        CreateDataImpressions();

        LoadTabularData();
        needToRender = true;
    }

    void Update()
    {
        if (needToRender)
        {
            ABREngine.Instance.Render();
        }
    }

    void LoadTabularData()
    {
        // Step 2: Convert data into tabular format to be used by 2D plotting
        // package and set 2D plotting data

        List<float> tabular = new List<float>();

        // foreach (Vector3 point in points)
        //     tabular.Add(point.x);
        // OR,
        tabular.AddRange(points.Select(p => p.x));
        tabular.AddRange(diffFromExpected);

        List<string> columnNames = new List<string> { "y coord", "y difference" };
        IVLab.Plotting.DataTable table = new IVLab.Plotting.DataTable(tabular.ToArray(), columnNames.ToArray(), "Paraboloid");

        plotDataManager.DataTable = table;
    }

    void CreateDataImpressions()
    {
        RawDataset pointsRaw = RawDatasetAdapter.PointsToPoints(points, pointsBounds, null, null);

        // Import the point data into ABR
        KeyData pointsKd = ABREngine.Instance.Data.ImportRawDataset(pointsRaw);

        pointsDi = new SimpleGlyphDataImpression();
        pointsDi.keyData = pointsKd;

        // Step 3: Set up RenderHints.PerIndexVisibility
        pointsDi.RenderHints.PerIndexVisibility = new System.Collections.BitArray(points.Count, false);

        ABREngine.Instance.RegisterDataImpression(pointsDi);


        // Set up the lines for context
        RawDataset linesRaw = RawDatasetAdapter.PointsToLine(lineSegments, pointsBounds, null);
        KeyData linesKd = ABREngine.Instance.Data.ImportRawDataset(linesRaw);

        lineDi = new SimpleLineDataImpression();
        lineDi.keyData = linesKd;
        lineDi.lineWidth = 0.03f;
        lineDi.colormap = ColormapVisAsset.SolidColor(new Color(0.1f, 0.05f, 0.05f));
        ABREngine.Instance.RegisterDataImpression(lineDi);
    }

    public override void UpdateDataPoint(int index, IVLab.Plotting.LinkedIndices.LinkedAttributes linkedAttributes)
    {
        // Step 4: Update ABR from this method
        if (pointsDi.RenderHints.HasPerIndexVisibility())
        {
            pointsDi.RenderHints.PerIndexVisibility[index] = linkedAttributes.Highlighted;
            pointsDi.RenderHints.StyleChanged = true;
            needToRender = true;
        }
    }

    // Placeholder for the Mt. St. Helens data loader
    void LoadData()
    {
        int numPoints = 10;
        int angles = 10;
        pointsBounds = new Bounds();
        int totalPoint = 0;
        for (int angle = 0; angle < angles; angle++)
        {
            List<Vector3> segment = new List<Vector3>();
            float angleRadians = (angle / (float) angles) * 2.0f * Mathf.PI;
            for (int i = 0; i < numPoints; i++)
            {
                float expectedY = i*i*0.1f;
                Vector3 point = new Vector3(Mathf.Cos(angleRadians) * i, expectedY, Mathf.Sin(angleRadians) * i);
                segment.Add(point);
                if (totalPoint % 12 == 0)
                {
                    point.y += (totalPoint / (float) (numPoints * angles)) * 4.0f - 2.0f;
                }

                pointsBounds.Encapsulate(point);
                diffFromExpected.Add(expectedY - point.y);
                points.Add(point);
                totalPoint++;
            }
            lineSegments.Add(segment);
        }
    }
}