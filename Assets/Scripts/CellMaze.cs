﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellMaze
{
    protected class Cell
    {
        private class Shift
        {
            public readonly int Dx;
            public readonly int Dy;
            public Shift(int dx, int dy)
            {
                Dx = dx;
                Dy = dy;
            }
        }

        private static List<Shift> shifts = new List<Shift>()
        {
            new Shift(-1, 0),
            new Shift(1, 0),
            new Shift(0, -1),
            new Shift(0, 1),
        };

        public readonly int X;
        public readonly int Y;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public List<Cell> AdjQuad
        {
            get
            {
                var outList = new List<Cell>();
                foreach (var item in shifts)
                    outList.Add(new Cell(X + item.Dx, Y + item.Dy));
                return outList;
            }
        }
    }

    protected readonly int Width;
    protected readonly int Height;
    protected bool[,] passes;
    protected Cell CurrentCell = null;
    protected Stack<Cell> MazeTrace = new Stack<Cell>();

    public abstract int OutTextureWidth { get; }
    public abstract int OutTextureHeight { get; }
    protected Color[] colorMap;

    public abstract CellMaze SetSize(int width, int height);

    public Color[] ToColor
    {
        get
        {
            if (colorMap == null)
            {
                colorMap = new Color[OutTextureWidth * OutTextureHeight];
                for (int i = 0; i < OutTextureWidth * OutTextureHeight; i++)
                    colorMap[i] = Color.black;
            }
            MazeToColor();
            return colorMap;
        }
    }

    protected abstract void MazeToColor();

    protected bool InMaze(Cell cell)
    {
        return ((cell.X < Width) && (cell.X >= 0) && (cell.Y < Height) && (cell.Y >= 0));
    }

    public CellMaze(int width, int height)
    {
        Width = width;
        Height = height;
        passes = new bool[Width, Height];
    }

    public void Generate()
    {
        while (NextStep());
    }

    public virtual void Clear()
    {
        CurrentCell = new Cell(Random.Range(0, Width), Random.Range(0, Height));
        MazeTrace.Clear();
        for (int i = 0; i < Width; i++)
            for (int n = 0; n < Height; n++)
                passes[i, n] = false;
    }

    public abstract bool NextStep();
}
