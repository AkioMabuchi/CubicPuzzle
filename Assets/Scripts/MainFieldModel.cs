using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum FloorColor
{
    White,
    Yellow,
    Blue
}
public enum SwitchPanel
{
    None,
    YellowUp,
    YellowDown,
    BlueUp,
    BlueDown
}

public class MainFieldFloor
{
    public int position;
    public int level;
    public FloorColor floorColor;
    public SwitchPanel switchPanel;
}
public class MainFieldPlayer
{
    public int position;
    public int level;
}
public class MainFieldFlag
{
    public int position;
    public int level;
    public bool doesExist;
}
public class MainFieldBox
{
    public int position;
    public int level;
    public int size;
}

public interface IMainFieldModel
{
    public IObservable<List<MainFieldFloor>> InitializeFloors
    {
        get;
    }

    public IObservable<List<MainFieldPlayer>> InitializePlayers
    {
        get;
    }

    public IObservable<List<MainFieldFlag>> InitializeFlags
    {
        get;
    }

    public IObservable<List<MainFieldBox>> InitializeBoxes
    {
        get;
    }

    public IObservable<List<MainFieldPlayer>> MovingPlayers
    {
        get;
    }

    public IObservable<List<MainFieldBox>> MovingBoxes
    {
        get;
    }

    public IObservable<List<MainFieldPlayer>> FallingPlayers
    {
        get;
    }

    public IObservable<List<MainFieldBox>> FallingBoxes
    {
        get;
    }

    public IObservable<List<MainFieldFloor>> ElevateFloors
    {
        get;
    }

    public IObservable<List<MainFieldPlayer>> ElevatePlayers
    {
        get;
    }

    public IObservable<List<MainFieldFlag>> ElevateFlags
    {
        get;
    }

    public IObservable<List<MainFieldBox>> ElevateBoxes
    {
        get;
    }

    public IObservable<List<MainFieldFloor>> DrawFloors
    {
        get;
    }
    public IObservable<List<MainFieldFlag>> DrawFlags
    {
        get;
    }

    public void InitializeMap(EditorMap editorMap);
    public bool MoveObjects(MoveDirection moveDirection);
    public bool FallObjects();
    public bool ElevateObjects();
    public bool IsClear();
    public bool UpdateFloors();
    public bool UpdateFlags();
}
public class MainFieldModel: MonoBehaviour, IMainFieldModel
{
    private readonly Subject<List<MainFieldFloor>> _initializeFloors = new Subject<List<MainFieldFloor>>();
    private readonly Subject<List<MainFieldPlayer>> _initializePlayers = new Subject<List<MainFieldPlayer>>();
    private readonly Subject<List<MainFieldFlag>> _initializeFlags = new Subject<List<MainFieldFlag>>();
    private readonly Subject<List<MainFieldBox>> _initializeBoxes = new Subject<List<MainFieldBox>>();

    private readonly Subject<List<MainFieldPlayer>> _movingPlayers = new Subject<List<MainFieldPlayer>>();
    private readonly Subject<List<MainFieldBox>> _movingBoxes = new Subject<List<MainFieldBox>>();
    
    private readonly Subject<List<MainFieldPlayer>> _fallingPlayers = new Subject<List<MainFieldPlayer>>();
    private readonly Subject<List<MainFieldBox>> _fallingBoxes = new Subject<List<MainFieldBox>>();
    
    private readonly Subject<List<MainFieldFloor>> _elevateFloors = new Subject<List<MainFieldFloor>>();
    private readonly Subject<List<MainFieldPlayer>> _elevatePlayers = new Subject<List<MainFieldPlayer>>();
    private readonly Subject<List<MainFieldFlag>> _elevateFlags = new Subject<List<MainFieldFlag>>();
    private readonly Subject<List<MainFieldBox>> _elevateBoxes = new Subject<List<MainFieldBox>>();
    
    private readonly Subject<List<MainFieldFloor>> _drawFloors = new Subject<List<MainFieldFloor>>();
    private readonly Subject<List<MainFieldFlag>> _drawFlags = new Subject<List<MainFieldFlag>>();

    public IObservable<List<MainFieldFloor>> InitializeFloors => _initializeFloors;
    public IObservable<List<MainFieldPlayer>> InitializePlayers => _initializePlayers;
    public IObservable<List<MainFieldFlag>> InitializeFlags => _initializeFlags;
    public IObservable<List<MainFieldBox>> InitializeBoxes => _initializeBoxes;
    public IObservable<List<MainFieldPlayer>> MovingPlayers => _movingPlayers;
    public IObservable<List<MainFieldBox>> MovingBoxes => _movingBoxes;
    public IObservable<List<MainFieldPlayer>> FallingPlayers => _fallingPlayers;
    public IObservable<List<MainFieldBox>> FallingBoxes => _fallingBoxes;
    public IObservable<List<MainFieldFloor>> ElevateFloors => _elevateFloors;
    public IObservable<List<MainFieldPlayer>> ElevatePlayers => _elevatePlayers;
    public IObservable<List<MainFieldFlag>> ElevateFlags => _elevateFlags;
    public IObservable<List<MainFieldBox>> ElevateBoxes => _elevateBoxes;
    public IObservable<List<MainFieldFloor>> DrawFloors => _drawFloors;
    public IObservable<List<MainFieldFlag>> DrawFlags => _drawFlags;
    
    
    
    private readonly List<MainFieldFloor> _floors = new List<MainFieldFloor>();
    private readonly List<MainFieldPlayer> _players = new List<MainFieldPlayer>();
    private readonly List<MainFieldFlag> _flags = new List<MainFieldFlag>();
    private readonly List<MainFieldBox> _boxes = new List<MainFieldBox>();
 
    private int _changeYellowLevel;
    private int _changeBlueLevel;

    private bool _hasFloorsUpdated;
    private bool _hasFlagsUpdated;
    
    public void InitializeMap(EditorMap editorMap)
    {
        _floors.Clear();
        _players.Clear();
        _flags.Clear();
        _boxes.Clear();

        for (int i = 0; i < 169; i++)
        {
            int level = 0;
            switch (editorMap.levels[i])
            {
                case '1':
                    level = 1;
                    break;
                case '2':
                    level = 2;
                    break;
                case '3':
                    level = 3;
                    break;
            }
            
            MainFieldFloor floor = new MainFieldFloor
            {
                position = i,
                level = level,
                switchPanel = SwitchPanel.None
            };

            MainFieldPlayer player = new MainFieldPlayer
            {
                position = i,
                level = level
            };

            MainFieldFlag flag = new MainFieldFlag
            {
                position = i,
                level = level,
                doesExist = true
            };

            MainFieldBox box = new MainFieldBox
            {
                position = i,
                level = level,
            };
            
            switch (editorMap.objects[i])
            {
                case 'Y':
                    floor.switchPanel = SwitchPanel.YellowUp;
                    break;
                case 'y':
                    floor.switchPanel = SwitchPanel.YellowDown;
                    break;
                case 'B':
                    floor.switchPanel = SwitchPanel.BlueUp;
                    break;
                case 'b':
                    floor.switchPanel = SwitchPanel.BlueDown;
                    break;
                case 'P':
                    _players.Add(player);
                    break;
                case 'F':
                    _flags.Add(flag);
                    break;
                case '1':
                    box.size = 1;
                    _boxes.Add(box);
                    break;
                case '2':
                    box.size = 2;
                    _boxes.Add(box);
                    break;
                case '3':
                    box.size = 3;
                    _boxes.Add(box);
                    break;
            }

            switch (editorMap.floors[i])
            {
                case 'W':
                    floor.floorColor = FloorColor.White;
                    _floors.Add(floor);
                    break;
                case 'Y':
                    floor.floorColor = FloorColor.Yellow;
                    _floors.Add(floor);
                    break;
                case 'B':
                    floor.floorColor = FloorColor.Blue;
                    _floors.Add(floor);
                    break;
            }
        }

        _initializeFloors.OnNext(_floors);
        _initializePlayers.OnNext(_players);
        _initializeFlags.OnNext(_flags);
        _initializeBoxes.OnNext(_boxes);
    }

    public bool MoveObjects(MoveDirection direction) //移動が発生すればtrueを返す。
    {
        bool r = false;

        int[] floorsMap = new int[169];
        int[] playersMap = new int[169];
        int[] flagsMap = new int[169];
        int[][] boxesMap = new int[169][];
        for (int index = 0; index < 169; index++)
        {
            boxesMap[index] = new int[8];
        }

        for (int i = 0; i < 169; i++)
        {
            floorsMap[i] = -1;
            playersMap[i] = -1;
            flagsMap[i] = -1;
            for (int j = 0; j < 8; j++)
            {
                boxesMap[i][j] = -1;
            }
        }

        for (int i = 0; i < _floors.Count; i++)
        {
            floorsMap[_floors[i].position] = i;
        }

        for (int i = 0; i < _players.Count; i++)
        {
            playersMap[_players[i].position] = i;
        }

        for (int i = 0; i < _flags.Count; i++)
        {
            if (_flags[i].doesExist)
            {
                flagsMap[_flags[i].position] = i;
            }
        }

        for (int i = 0; i < _boxes.Count; i++)
        {
            boxesMap[_boxes[i].position][_boxes[i].level] = i;
        }
        
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
                if (playersMap[indexes[j]] >= 0)
                {
                    if (playersMap[indexes[j - 1]] < 0 && floorsMap[indexes[j - 1]] >= 0)
                    {
                        if (_players[playersMap[indexes[j]]].level >= _floors[floorsMap[indexes[j - 1]]].level)
                        {
                            bool isMovable = true;
                            bool doesBoxExist = false;
                            for (int k = _players[playersMap[indexes[j]]].level; k < 8; k++) 
                            {
                                if (boxesMap[indexes[j - 1]][k] >= 0)
                                {
                                    doesBoxExist = true;
                                }
                            }
                            if (doesBoxExist)
                            {
                                if (j >= 2)
                                {
                                    if (flagsMap[indexes[j - 2]] < 0 && floorsMap[indexes[j - 2]] >= 0) 
                                    {
                                        bool doesPlayerExist = false;
                                        if (playersMap[indexes[j - 2]] >= 0)
                                        {
                                            if (_players[playersMap[indexes[j - 2]]].level ==
                                                _floors[floorsMap[indexes[j - 2]]].level) 
                                            {
                                                doesPlayerExist = true;
                                            }
                                        }

                                        if (doesPlayerExist)
                                        {
                                            isMovable = false;
                                        }
                                        else
                                        {
                                            for (int k = 0; k < 8; k++)
                                            {
                                                if (boxesMap[indexes[j - 1]][k] >= 0)
                                                {
                                                    if (_players[playersMap[indexes[j]]].level <
                                                        _boxes[boxesMap[indexes[j - 1]][k]].level +
                                                        _boxes[boxesMap[indexes[j - 1]][k]].size)
                                                    {
                                                        if (_boxes[boxesMap[indexes[j - 1]][k]].level >=
                                                            _floors[floorsMap[indexes[j - 2]]].level)
                                                        {
                                                            for (int l = _boxes[boxesMap[indexes[j - 1]][k]].level;
                                                                l < 8;
                                                                l++)
                                                            {
                                                                if (boxesMap[indexes[j - 2]][l] >= 0)
                                                                {
                                                                    isMovable = false;
                                                                }
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
                                for (int k = 0; k < 8; k++)
                                {
                                    if (boxesMap[indexes[j - 1]][k] >= 0)
                                    {
                                        if (_players[playersMap[indexes[j]]].level <
                                            _boxes[boxesMap[indexes[j - 1]][k]].level +
                                            _boxes[boxesMap[indexes[j - 1]][k]].size)
                                        {
                                            if (_boxes[boxesMap[indexes[j - 1]][k]].level ==
                                                _floors[floorsMap[indexes[j - 2]]].level)
                                            {
                                                switch (_floors[floorsMap[indexes[j - 2]]].switchPanel)
                                                {
                                                    case SwitchPanel.YellowUp:
                                                        _floors[floorsMap[indexes[j - 2]]].switchPanel =
                                                            SwitchPanel.YellowDown;
                                                        _changeYellowLevel++;
                                                        _hasFloorsUpdated = true;
                                                        break;
                                                    case SwitchPanel.YellowDown:
                                                        _floors[floorsMap[indexes[j - 2]]].switchPanel =
                                                            SwitchPanel.YellowUp;
                                                        _changeYellowLevel--;
                                                        _hasFloorsUpdated = true;
                                                        break;
                                                    case SwitchPanel.BlueUp:
                                                        _floors[floorsMap[indexes[j - 2]]].switchPanel =
                                                            SwitchPanel.BlueDown;
                                                        _changeBlueLevel++;
                                                        _hasFloorsUpdated = true;
                                                        break;
                                                    case SwitchPanel.BlueDown:
                                                        _floors[floorsMap[indexes[j - 2]]].switchPanel =
                                                            SwitchPanel.BlueUp;
                                                        _changeBlueLevel--;
                                                        _hasFloorsUpdated = true;
                                                        break;
                                                }
                                            }
                                            _boxes[boxesMap[indexes[j - 1]][k]].position = indexes[j - 2];
                                            boxesMap[indexes[j - 2]][k] = boxesMap[indexes[j - 1]][k];
                                            boxesMap[indexes[j - 1]][k] = -1;
                                        }
                                    }
                                }

                                if (_players[playersMap[indexes[j]]].level ==
                                    _floors[floorsMap[indexes[j - 1]]].level) 
                                {
                                    switch (_floors[floorsMap[indexes[j - 1]]].switchPanel)
                                    {
                                        case SwitchPanel.YellowUp:
                                            _floors[floorsMap[indexes[j - 1]]].switchPanel =
                                                SwitchPanel.YellowDown;
                                            _changeYellowLevel++;
                                            _hasFloorsUpdated = true;
                                            break;
                                        case SwitchPanel.YellowDown:
                                            _floors[floorsMap[indexes[j - 1]]].switchPanel =
                                                SwitchPanel.YellowUp;
                                            _changeYellowLevel--;
                                            _hasFloorsUpdated = true;
                                            break;
                                        case SwitchPanel.BlueUp:
                                            _floors[floorsMap[indexes[j - 1]]].switchPanel =
                                                SwitchPanel.BlueDown;
                                            _changeBlueLevel++;
                                            _hasFloorsUpdated = true;
                                            break;
                                        case SwitchPanel.BlueDown:
                                            _floors[floorsMap[indexes[j - 1]]].switchPanel =
                                                SwitchPanel.BlueUp;
                                            _changeBlueLevel--;
                                            _hasFloorsUpdated = true;
                                            break;
                                    }
                                }

                                if (flagsMap[indexes[j - 1]] >= 0)
                                {
                                    if (_players[playersMap[indexes[j]]].level ==
                                        _flags[flagsMap[indexes[j - 1]]].level)
                                    {
                                        _flags[flagsMap[indexes[j - 1]]].doesExist = false;
                                        _hasFlagsUpdated = true;
                                    }
                                }

                                _players[playersMap[indexes[j]]].position = indexes[j - 1];
                                playersMap[indexes[j - 1]] = playersMap[indexes[j]];
                                playersMap[indexes[j]] = -1;
                                
                                r = true;
                            }
                        }
                    }
                }
            }
        }

        _movingPlayers.OnNext(_players);
        _movingBoxes.OnNext(_boxes);
        return r;
    }

    public bool FallObjects()
    {
        bool r = false;
        int[] floorsMap = new int[169];
        int[] playersMap = new int[169];
        int[][] boxesMap = new int[169][];
        for (int i = 0; i < 169; i++)
        {
            boxesMap[i] = new int[8];
        }

        int[] flagsMap = new int[169];
        int[] currentLevel = new int[169];

        for (int i = 0; i < 169; i++)
        {            
            floorsMap[i] = -1;
            playersMap[i] = -1;
            flagsMap[i] = -1;
            for (int j = 0; j < 8; j++)
            {
                boxesMap[i][j] = -1;
            }
            currentLevel[i] = -1;
        }

        for (int i = 0; i < _floors.Count; i++)
        {
            floorsMap[_floors[i].position] = i;
            currentLevel[_floors[i].position] = _floors[i].level;
        }

        for (int i = 0; i < _players.Count; i++)
        {
            playersMap[_players[i].position] = i;
        }
        
        for (int i = 0; i < _flags.Count; i++)
        {
            if (_flags[i].doesExist)
            {
                flagsMap[_flags[i].position] = i;
            }
        }

        for (int i = 0; i < _boxes.Count; i++)
        {
            boxesMap[_boxes[i].position][_boxes[i].level] = i;
        }

        for (int i = 0; i < 169; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (boxesMap[i][j] >= 0)
                {
                    if (_boxes[boxesMap[i][j]].level > currentLevel[i])
                    {
                        _boxes[boxesMap[i][j]].level = currentLevel[i];
                        if (_floors[floorsMap[i]].level == currentLevel[i])
                        {
                            switch (_floors[floorsMap[i]].switchPanel)
                            {
                                case SwitchPanel.YellowUp:
                                    _floors[floorsMap[i]].switchPanel =
                                        SwitchPanel.YellowDown;
                                    _changeYellowLevel++;
                                    _hasFloorsUpdated = true;
                                    break;
                                case SwitchPanel.YellowDown:
                                    _floors[floorsMap[i]].switchPanel =
                                        SwitchPanel.YellowUp;
                                    _changeYellowLevel--;
                                    _hasFloorsUpdated = true;
                                    break;
                                case SwitchPanel.BlueUp:
                                    _floors[floorsMap[i]].switchPanel =
                                        SwitchPanel.BlueDown;
                                    _changeBlueLevel++;
                                    _hasFloorsUpdated = true;
                                    break;
                                case SwitchPanel.BlueDown:
                                    _floors[floorsMap[i]].switchPanel =
                                        SwitchPanel.BlueUp;
                                    _changeBlueLevel--;
                                    _hasFloorsUpdated = true;
                                    break;
                            }
                        }
                        currentLevel[i] += _boxes[boxesMap[i][j]].size;
                        
                        r = true;
                    }
                    else if (_boxes[boxesMap[i][j]].level == currentLevel[i])
                    {
                        currentLevel[i] += _boxes[boxesMap[i][j]].size;
                    }
                }
            }
        }

        for (int i = 0; i < 169; i++)
        {
            if (playersMap[i] >= 0)
            {
                if (_players[playersMap[i]].level > currentLevel[i])
                {
                    _players[playersMap[i]].level = currentLevel[i];
                    if (_floors[floorsMap[i]].level == currentLevel[i])
                    {
                        switch (_floors[floorsMap[i]].switchPanel)
                        {
                            case SwitchPanel.YellowUp:
                                _floors[floorsMap[i]].switchPanel =
                                    SwitchPanel.YellowDown;
                                _changeYellowLevel++;
                                _hasFloorsUpdated = true;
                                break;
                            case SwitchPanel.YellowDown:
                                _floors[floorsMap[i]].switchPanel =
                                    SwitchPanel.YellowUp;
                                _changeYellowLevel--;
                                _hasFloorsUpdated = true;
                                break;
                            case SwitchPanel.BlueUp:
                                _floors[floorsMap[i]].switchPanel =
                                    SwitchPanel.BlueDown;
                                _changeBlueLevel++;
                                _hasFloorsUpdated = true;
                                break;
                            case SwitchPanel.BlueDown:
                                _floors[floorsMap[i]].switchPanel =
                                    SwitchPanel.BlueUp;
                                _changeBlueLevel--;
                                _hasFloorsUpdated = true;
                                break;
                        }
                    }

                    if (flagsMap[i] >= 0)
                    {
                        if (_flags[flagsMap[i]].level == currentLevel[i])
                        {
                            _flags[flagsMap[i]].doesExist = false;
                            _hasFlagsUpdated = true;
                        }
                    }

                    r = true;
                }
            }
        }

        _fallingPlayers.OnNext(_players);
        _fallingBoxes.OnNext(_boxes);
        return r;
    }

    public bool ElevateObjects()
    {
        bool r = false;

        if (_changeYellowLevel != 0)
        {
            for (int i = 0; i < _floors.Count; i++)
            {
                if (_floors[i].floorColor == FloorColor.Yellow)
                {
                    if (_changeYellowLevel > 0 && _floors[i].level < 3)
                    {
                        _floors[i].level += _changeYellowLevel;
                        if (_floors[i].level > 3)
                        {
                            _floors[i].level = 3;
                        }

                        r = true;
                    }

                    if (_changeYellowLevel < 0 && _floors[i].level > 0)
                    {
                        _floors[i].level += _changeYellowLevel;
                        if (_floors[i].level < 0)
                        {
                            _floors[i].level = 0;
                        }

                        r = true;
                    }
                }
            }
            _changeYellowLevel = 0;
        }

        if (_changeBlueLevel != 0)
        {
            for (int i = 0; i < _floors.Count; i++)
            {
                if (_floors[i].floorColor == FloorColor.Blue)
                {
                    if (_changeBlueLevel > 0 && _floors[i].level < 3)
                    {
                        _floors[i].level += _changeBlueLevel;
                        if (_floors[i].level > 3)
                        {
                            _floors[i].level = 3;
                        }

                        r = true;
                    }

                    if (_changeBlueLevel < 0 && _floors[i].level > 0)
                    {
                        _floors[i].level += _changeBlueLevel;
                        if (_floors[i].level < 0)
                        {
                            _floors[i].level = 0;
                        }

                        r = true;
                    }
                }
            }

            _changeBlueLevel = 0;
        }

        if (r)
        {
            int[][] boxesMap = new int[169][];
            for (int index = 0; index < 169; index++)
            {
                boxesMap[index] = new int[8];
            }

            int[] currentLevel = new int[169];
            for (int i = 0; i < 169; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boxesMap[i][j] = -1;
                }

                currentLevel[i] = -1;
            }

            for (int i = 0; i < _floors.Count; i++)
            {
                currentLevel[_floors[i].position] = _floors[i].level;
            }

            for (int i = 0; i < _boxes.Count; i++)
            {
                boxesMap[_boxes[i].position][_boxes[i].level] = i;
            }

            for (int i = 0; i < 169; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boxesMap[i][j] >= 0)
                    {
                        _boxes[boxesMap[i][j]].level = currentLevel[i];
                        currentLevel[i] += _boxes[boxesMap[i][j]].size;
                    }
                }
            }

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].level = currentLevel[_players[i].position];
            }

            for (int i = 0; i < _flags.Count; i++)
            {
                _flags[i].level = currentLevel[_flags[i].position];
            }

            _elevateFloors.OnNext(_floors);
            _elevatePlayers.OnNext(_players);
            _elevateFlags.OnNext(_flags);
            _elevateBoxes.OnNext(_boxes);
        }

        return r;
    }

    public bool UpdateFloors()
    {
        if (_hasFloorsUpdated)
        {
            _drawFloors.OnNext(_floors);
            _hasFloorsUpdated = false;
            return true;
        }

        return false;
    }

    public bool UpdateFlags()
    {
        if (_hasFlagsUpdated)
        {
            _drawFlags.OnNext(_flags);
            _hasFlagsUpdated = false;
            return true;
        }

        return false;
    }
    public bool IsClear()
    {
        for (int i = 0; i < _flags.Count; i++)
        {
            if (_flags[i].doesExist)
            {
                return false;
            }
        }
        return true;
    }
}
