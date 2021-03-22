using System;
using UniRx;
using UnityEngine;

public struct BoxData
{
    public int[] objects;
    public int[] positions;
    public int[] levels;
    public int[] sizes;
}

public struct PushedBoxData
{
    public int[] indexes;
    public int[] positions;
}

public struct FallBoxData
{
    public int[] indexes;
    public int[] levels;
}

public struct ElevateBoxData
{
    public int[] indexes;
    public int[] levels;
}

public static class MainFieldModel
{
    public static readonly Subject<int[]> InitializePlayers = new Subject<int[]>();
    public static readonly Subject<EditorMap> InitializeFloors = new Subject<EditorMap>();
    public static readonly Subject<int[]> InitializeFlags = new Subject<int[]>();
    public static readonly Subject<BoxData> InitializeBoxes = new Subject<BoxData>();
    public static readonly Subject<char[]> DrawSwitches = new Subject<char[]>();
    public static readonly Subject<int[]> PositioningPlayers = new Subject<int[]>();
    public static readonly Subject<int[]> MovingPlayers = new Subject<int[]>();
    public static readonly Subject<PushedBoxData> MovingBoxes = new Subject<PushedBoxData>();
    public static readonly Subject<FallBoxData> FallingBoxes = new Subject<FallBoxData>();
    public static readonly Subject<int[]> ElevatePlayers = new Subject<int[]>();
    public static readonly Subject<int[]> ElevateFloors = new Subject<int[]>();
    public static readonly Subject<int[]> ElevateFlags = new Subject<int[]>();
    public static readonly Subject<ElevateBoxData> ElevateBoxes = new Subject<ElevateBoxData>();
    public static readonly Subject<int> GetFlag = new Subject<int>();
    
    private static readonly int[] Players = new int[169];
    private static readonly int[] PlayerLevels = new int[169];
    private static readonly char[] Floors = new char[169];
    private static readonly int[] FloorLevels = new int[169];
    private static readonly char[] Switches = new char[169];
    private static readonly int[] FlagLevels = new int[169];
    private static readonly int[] Boxes = new int[169];
    private static readonly int[] BoxPositions = new int[169];
    private static readonly int[] BoxLevels = new int[169];
    private static readonly int[] BoxSizes = new int[169];
 
    private static int _changeYellowLevel;
    private static int _changeBlueLevel;
    
    static MainFieldModel()
    {
        for (int i = 0; i < 169; i++)
        {
            Players[i] = -1;
            FlagLevels[i] = -1;
        }
    }
    public static void InitializeMap(EditorMap map)
    {
        int playerIndex = 0;
        int boxIndex = 0;
        for (int i = 0; i < 169; i++)
        {
            Players[i] = -1;
            FlagLevels[i] = -1;
            PlayerLevels[i] = 0;
            Boxes[i] = -1;
            BoxLevels[i] = 0;
            BoxSizes[i] = 0;
        }
        
        for (int i = 0; i < 169; i++)
        {
            switch (map.objects[i])
            {
                case 'P':
                    Players[i] = playerIndex;
                    playerIndex++;
                    break;
                case '1':
                    Boxes[boxIndex] = boxIndex;
                    BoxPositions[boxIndex] = i;
                    BoxSizes[boxIndex] = 1;
                    boxIndex++;
                    break;
                case '2':
                    Boxes[boxIndex] = boxIndex;
                    BoxPositions[boxIndex] = i;
                    BoxSizes[boxIndex] = 2;
                    boxIndex++;
                    break;
                case '3':
                    Boxes[boxIndex] = boxIndex;
                    BoxPositions[boxIndex] = i;
                    BoxSizes[boxIndex] = 3;
                    boxIndex++;
                    break;
            }
        }

        for (int i = 0; i < 169; i++)
        {
            Floors[i] = map.floors[i];
            
            switch (map.levels[i])
            {
                case '0':
                    FloorLevels[i] = 0;
                    break;
                case '1':
                    FloorLevels[i] = 1;
                    break;
                case '2':
                    FloorLevels[i] = 2;
                    break;
                case '3':
                    FloorLevels[i] = 3;
                    break;
            }

            switch (map.objects[i])
            {
                case 'Y':
                    Switches[i] = 'Y';
                    break;
                case 'y':
                    Switches[i] = 'y';
                    break;
                case 'B':
                    Switches[i] = 'B';
                    break;
                case 'b':
                    Switches[i] = 'b';
                    break;
                default:
                    Switches[i] = '.';
                    break;
            }
        }

        for (int i = 0; i < 169; i++)
        {
            int index = Players[i];
            if (index >= 0)
            {
                PlayerLevels[index] = FloorLevels[i];
            }
        }

        for (int i = 0; i < 169; i++)
        {
            if (Boxes[i] >= 0)
            {
                if (BoxPositions[Boxes[i]] >= 0)
                {
                    BoxLevels[Boxes[i]] = FloorLevels[BoxPositions[Boxes[i]]];
                }
            }
        }
        for (int i = 0; i < 169; i++)
        {
            if (map.objects[i] == 'F')
            {
                FlagLevels[i] = FloorLevels[i];
            }
            else
            {
                FlagLevels[i] = -1;
            }
        }
        
        InitializePlayers.OnNext(Players);
        InitializeFloors.OnNext(map);
        InitializeFlags.OnNext(FlagLevels);
        InitializeBoxes.OnNext(new BoxData {objects = Boxes, positions = BoxPositions, levels = BoxLevels, sizes = BoxSizes});
        PositioningPlayers.OnNext(PlayerLevels);
        DrawSwitches.OnNext(Switches);
    }

    public static bool MoveObjects(MoveDirection direction) //移動が発生すればtrueを返す。
    {
        bool r = false;
        for (int i = 0; i < 13; i++)
        {
            int[] indexes = new int[13];
            for (int j = 0; j < 13; j++)
            {
                switch (direction)
                {
                    case MoveDirection.Up:
                        indexes[j] = 156 - j * 13 + i;
                        break;
                    case MoveDirection.Right:
                        indexes[j] = 12 - j + 13 * i;
                        break;
                    case MoveDirection.Down:
                        indexes[j] = j * 13 + i;
                        break;
                    case MoveDirection.Left:
                        indexes[j] = i * 13 + j;
                        break;
                }
            }

            for (int j = 1; j < 13; j++)
            {
                int playerIndex = Players[indexes[j]];
                if (playerIndex >= 0)
                {
                    if (Players[indexes[j - 1]] == -1 && Floors[indexes[j - 1]] != '_' &&
                        FloorLevels[indexes[j - 1]] <= PlayerLevels[playerIndex])
                    {
                        bool isMovable = true;
                        bool hasBox = false;
                        for (int k = 0; k < 169; k++)
                        {
                            if (Boxes[k] >= 0)
                            {
                                if (BoxPositions[Boxes[k]] == indexes[j - 1])
                                {
                                    if (BoxLevels[Boxes[k]] + BoxSizes[Boxes[k]] > PlayerLevels[playerIndex])
                                    {
                                        hasBox = true;
                                    }
                                }
                            }
                        }

                        if (hasBox)
                        {
                            if (j >= 2)
                            {
                                if (FlagLevels[indexes[j - 2]] < 0 && Floors[indexes[j - 2]] != '_')
                                {
                                    bool hasHuman = false;
                                    if (Players[indexes[j - 2]] >= 0)
                                    {
                                        if (PlayerLevels[Players[indexes[j - 2]]] ==
                                            FloorLevels[indexes[j - 2]]) 
                                        {
                                            hasHuman = true;
                                        }
                                    }
                                    if (!hasHuman)
                                    {
                                        int[] boxIndexes = new int[8];
                                        for (int k = 0; k < 8; k++)
                                        {
                                            boxIndexes[k] = -1;
                                        }

                                        int nextLevel = FloorLevels[indexes[j - 2]];
                                        for (int k = 0; k < 169; k++)
                                        {
                                            if (Boxes[k] >= 0)
                                            {
                                                if (BoxPositions[Boxes[k]] == indexes[j - 1])
                                                {
                                                    boxIndexes[BoxLevels[Boxes[k]]] = Boxes[k];
                                                }

                                                if (BoxPositions[Boxes[k]] == indexes[j - 2])
                                                {
                                                    nextLevel += BoxSizes[Boxes[k]];
                                                }
                                            }
                                        }

                                        bool hasDoneSwitched = false;
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if (boxIndexes[k] >= 0)
                                            {
                                                if (BoxLevels[boxIndexes[k]] + BoxSizes[boxIndexes[k]] >
                                                    PlayerLevels[playerIndex])
                                                {
                                                    if (isMovable && BoxLevels[boxIndexes[k]] >= nextLevel)
                                                    {
                                                        BoxPositions[boxIndexes[k]] = indexes[j - 2];
                                                        if (!hasDoneSwitched)
                                                        {
                                                            switch (Switches[indexes[j - 2]])
                                                            {
                                                                case 'Y':
                                                                    Switches[indexes[j - 2]] = 'y';
                                                                    _changeYellowLevel++;
                                                                    break;
                                                                case 'y':
                                                                    Switches[indexes[j - 2]] = 'Y';
                                                                    _changeYellowLevel--;
                                                                    break;
                                                                case 'B':
                                                                    Switches[indexes[j - 2]] = 'b';
                                                                    _changeBlueLevel++;
                                                                    break;
                                                                case 'b':
                                                                    Switches[indexes[j - 2]] = 'B';
                                                                    _changeBlueLevel--;
                                                                    break;
                                                            }
                                                            hasDoneSwitched = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isMovable = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isMovable = false;
                                    }
                                }
                                else
                                {
                                    isMovable = false;
                                }
                            }
                            else
                            {
                                isMovable = false;
                            }
                        }

                        if (isMovable)
                        {
                            Players[indexes[j - 1]] = playerIndex;
                            Players[indexes[j]] = -1;

                            if (PlayerLevels[playerIndex] == FloorLevels[indexes[j - 1]]) 
                            {
                                switch (Switches[indexes[j - 1]])
                                {
                                    case 'Y':
                                        Switches[indexes[j - 1]] = 'y';
                                        _changeYellowLevel++;
                                        break;
                                    case 'y':
                                        Switches[indexes[j - 1]] = 'Y';
                                        _changeYellowLevel--;
                                        break;
                                    case 'B':
                                        Switches[indexes[j - 1]] = 'b';
                                        _changeBlueLevel++;
                                        break;
                                    case 'b':
                                        Switches[indexes[j - 1]] = 'B';
                                        _changeBlueLevel--;
                                        break;
                                }
                            }
                            
                            if (FlagLevels[indexes[j - 1]] == PlayerLevels[playerIndex])
                            {
                                FlagLevels[indexes[j - 1]] = -1;
                                GetFlag.OnNext(indexes[j - 1]);
                            }

                            r = true;
                        }
                    }
                }
            }
        }

        MovingPlayers.OnNext(Players);
        MovingBoxes.OnNext(new PushedBoxData {indexes = Boxes, positions = BoxPositions});
        return r;
    }

    public static void FallObjects()
    {
        int[] currentLevels = new int[169];
        for (int i = 0; i < 169; i++)
        {
            currentLevels[i] = FloorLevels[i];
        }

        for (int i = 0; i < 169; i++)
        {
            int[] boxIndexes = new int[8];
            for (int j = 0; j < 8; j++)
            {
                boxIndexes[j] = -1;
            }

            for (int j = 0; j < 169; j++)
            {
                if (Boxes[j] >= 0)
                {
                    if (BoxPositions[Boxes[j]] == i)
                    {
                        boxIndexes[BoxLevels[Boxes[j]]] = Boxes[j];
                    }
                }
            }

            for (int j = 0; j < 8; j++)
            {
                if (boxIndexes[j] >= 0)
                {
                    BoxLevels[boxIndexes[j]] = currentLevels[i];
                    currentLevels[i] += BoxSizes[boxIndexes[j]];

                }
            }
        }

        for (int i = 0; i < 169; i++)
        {
            if (Players[i] >= 0)
            {
                if (PlayerLevels[Players[i]] != currentLevels[i])
                {
                    PlayerLevels[Players[i]] = currentLevels[i];
                    if (PlayerLevels[Players[i]] == FloorLevels[i])
                    {
                        switch (Switches[i])
                        {
                            case 'Y':
                                Switches[i] = 'y';
                                _changeYellowLevel++;
                                break;
                            case 'y':
                                Switches[i] = 'Y';
                                _changeYellowLevel--;
                                break;
                            case 'B':
                                Switches[i] = 'b';
                                _changeBlueLevel++;
                                break;
                            case 'b':
                                Switches[i] = 'B';
                                _changeBlueLevel--;
                                break;
                        }
                    }

                    if (PlayerLevels[Players[i]] == FlagLevels[i])
                    {
                        FlagLevels[i] = -1;
                        GetFlag.OnNext(i);
                    }
                }

            }
        }

        PositioningPlayers.OnNext(PlayerLevels);
        FallingBoxes.OnNext(new FallBoxData {indexes = Boxes, levels = BoxLevels});
    }

    public static bool ElevateObjects()
    {
        bool r = false;

        if (_changeYellowLevel != 0)
        {
            for (int i = 0; i < 169; i++)
            {
                if (Floors[i] == 'Y')
                {
                    FloorLevels[i] += _changeYellowLevel;
                    if (FloorLevels[i] > 3)
                    {
                        FloorLevels[i] = 3;
                    }

                    if (FloorLevels[i] < 0)
                    {
                        FloorLevels[i] = 0;
                    }
                }
            }
            r = true;
        }
        _changeYellowLevel = 0;
        
        if (_changeBlueLevel != 0)
        {
            for (int i = 0; i < 169; i++)
            {
                if (Floors[i] == 'B')
                {
                    FloorLevels[i] += _changeBlueLevel;
                    if (FloorLevels[i] > 3)
                    {
                        FloorLevels[i] = 3;
                    }

                    if (FloorLevels[i] < 0)
                    {
                        FloorLevels[i] = 0;
                    }
                }
            }
            r = true;
        }
        _changeBlueLevel = 0;
        
        if (r)
        {
            int[] currentLevels = new int[169];
            for (int i = 0; i < 169; i++)
            {
                currentLevels[i] = FloorLevels[i];
            }

            for (int i = 0; i < 169; i++)
            {
                int[] boxIndexes = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    boxIndexes[j] = -1;
                }

                for (int j = 0; j < 169; j++)
                {
                    if (Boxes[j] >= 0)
                    {
                        if (BoxPositions[Boxes[j]] == i)
                        {
                            boxIndexes[BoxLevels[Boxes[j]]] = Boxes[j];
                        }
                    }
                }

                for (int j = 0; j < 8; j++)
                {
                    if (boxIndexes[j] >= 0)
                    {
                        BoxLevels[boxIndexes[j]] = currentLevels[i];
                        currentLevels[i] += BoxSizes[boxIndexes[j]];
                    }
                }
            }
            for (int i = 0; i < 169; i++)
            {
                if (Players[i] >= 0)
                {
                    PlayerLevels[Players[i]] = currentLevels[i];
                }
            }

            for (int i = 0; i < 169; i++)
            {
                if (FlagLevels[i] >= 0)
                {
                    FlagLevels[i] = FloorLevels[i];
                }
            }

            ElevateFloors.OnNext(FloorLevels);
            ElevatePlayers.OnNext(PlayerLevels);
            ElevateFlags.OnNext(FlagLevels);
            ElevateBoxes.OnNext(new ElevateBoxData {indexes = Boxes, levels = BoxLevels});
            DrawSwitches.OnNext(Switches);
        }


        return r;
    }

    public static bool IsClear()
    {
        for (int i = 0; i < 169; i++)
        {
            if (FlagLevels[i] >= 0)
            {
                return false;
            }
        }

        return true;
    }
}
