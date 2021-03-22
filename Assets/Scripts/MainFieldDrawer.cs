using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public class MainFieldDrawerPresenter
{
    public MainFieldDrawerPresenter()
    {
        IMainFieldDrawer mainFieldDrawer = GameObject.Find("MainFieldDrawer").GetComponent<IMainFieldDrawer>();

        MainFieldModel.InitializeFloors.Subscribe(map =>
        {
            mainFieldDrawer.InitializeFloors(map);
        });
        
        MainFieldModel.InitializePlayers.Subscribe(players =>
        {
            mainFieldDrawer.InitializePlayers(players);
        });
        
        MainFieldModel.InitializeFlags.Subscribe(flagLevels =>
        {
            mainFieldDrawer.InitializeFlags(flagLevels);
        });

        MainFieldModel.InitializeBoxes.Subscribe(boxData =>
        {
            mainFieldDrawer.InitializeBoxes(boxData);
        });
        
        MainFieldModel.DrawSwitches.Subscribe(switches =>
        {
            mainFieldDrawer.DrawSwitches(switches);
        });
        
        MainFieldModel.PositioningPlayers.Subscribe(playerLevels =>
        {
            mainFieldDrawer.PositioningPlayers(playerLevels);
        });

        MainFieldModel.MovingPlayers.Subscribe(players =>
        {
            mainFieldDrawer.MovingPlayers(players);
        });

        MainFieldModel.MovingBoxes.Subscribe(boxData =>
        {
            mainFieldDrawer.MovingBoxes(boxData);
        });
        MainFieldModel.FallingBoxes.Subscribe(boxData =>
        {
            mainFieldDrawer.FallingBoxes(boxData);
        });

        MainFieldModel.ElevateFloors.Subscribe(floorLevels =>
        {
            mainFieldDrawer.ElevateFloors(floorLevels);
        });

        MainFieldModel.ElevatePlayers.Subscribe(playerLevels =>
        {
            mainFieldDrawer.ElevatePlayers(playerLevels);
        });

        MainFieldModel.ElevateFlags.Subscribe(flagLevels =>
        {
            mainFieldDrawer.ElevateFlags(flagLevels);
        });
        
        MainFieldModel.ElevateBoxes.Subscribe(boxData =>
        {
            mainFieldDrawer.ElevateBoxes(boxData);
        });
        
        MainFieldModel.GetFlag.Subscribe(index =>
        {
            mainFieldDrawer.GetFlag(index);
        });
    }
}

public interface IMainFieldDrawer
{
    public void InitializeFloors(EditorMap map);
    public void InitializePlayers(int[] players);
    public void InitializeFlags(int[] flagLevels);
    public void InitializeBoxes(BoxData boxData);
    public void DrawSwitches(char[] switches);
    public void PositioningPlayers(int[] playersLevel);
    public void MovingPlayers(int[] players);
    public void MovingBoxes(PushedBoxData boxData);
    public void FallingBoxes(FallBoxData boxData);
    public void ElevateFloors(int[] floorLevels);
    public void ElevatePlayers(int[] playerLevels);
    public void ElevateFlags(int[] flagLevels);
    public void ElevateBoxes(ElevateBoxData boxData);
    public void GetFlag(int index);
    public void ClearField();
}
public class MainFieldDrawer : MonoBehaviour, IMainFieldDrawer
{
    [SerializeField] private GameObject prefabFloorWhite;
    [SerializeField] private GameObject prefabFloorYellow;
    [SerializeField] private GameObject prefabFloorBlue;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabFlag;
    [SerializeField] private GameObject prefabBox1;
    [SerializeField] private GameObject prefabBox2;
    [SerializeField] private GameObject prefabBox3;

    private readonly IPuzzleFloor[] _puzzleFloors = new IPuzzleFloor[169];
    private readonly IPuzzleObject[] _playerObjects = new IPuzzleObject[169];
    private readonly IPuzzleObject[] _flagObjects = new IPuzzleObject[169];
    private readonly IPuzzleObject[] _boxObjects = new IPuzzleObject[169];

    public void InitializeFloors(EditorMap map)
    {
        for (int i = 0; i < 169; i++)
        {
            _puzzleFloors[i]?.Diminish();
            _puzzleFloors[i] = null;

            IPuzzleFloor puzzleFloor = null;
            
            switch (map.floors[i])
            {
                case 'W':
                    puzzleFloor = Instantiate(prefabFloorWhite).GetComponent<IPuzzleFloor>();
                    break;
                case 'Y':
                    puzzleFloor = Instantiate(prefabFloorYellow).GetComponent<IPuzzleFloor>();
                    break;
                case 'B':
                    puzzleFloor = Instantiate(prefabFloorBlue).GetComponent<IPuzzleFloor>();
                    break;
            }
            
            puzzleFloor?.SetPosition(i);

            switch (map.levels[i])
            {
                case '0':
                    puzzleFloor?.SetLevel(0);
                    break;
                case '1':
                    puzzleFloor?.SetLevel(1);
                    break;
                case '2':
                    puzzleFloor?.SetLevel(2);
                    break;
                case '3':
                    puzzleFloor?.SetLevel(3);
                    break;
            }

            switch (map.objects[i])
            {
                case 'Y':
                    puzzleFloor?.SetModel(PuzzleFloorMode.YellowUp);
                    break;
                case 'y':
                    puzzleFloor?.SetModel(PuzzleFloorMode.YellowDown);
                    break;
                case 'B':
                    puzzleFloor?.SetModel(PuzzleFloorMode.BlueUp);
                    break;
                case 'b':
                    puzzleFloor?.SetModel(PuzzleFloorMode.BlueDown);
                    break;
                default:
                    puzzleFloor?.SetModel(PuzzleFloorMode.Normal);
                    break;
            }
            
            _puzzleFloors[i] = puzzleFloor;
        }
    }

    public void InitializePlayers(int[] players)
    {
        for (int i = 0; i < 169; i++)
        {
            _playerObjects[i]?.Diminish();
            _playerObjects[i] = null;
        }

        for (int i = 0; i < 169; i++)
        {
            if (players[i] >= 0)
            {
                _playerObjects[players[i]] = Instantiate(prefabPlayer).GetComponent<IPuzzleObject>();
                _playerObjects[players[i]].SetPosition(i);
            }
        }
    }

    public void InitializeFlags(int[] flagLevels)
    {
        for (int i = 0; i < 169; i++)
        {
            _flagObjects[i]?.Diminish();
            _flagObjects[i] = null;
        }

        for (int i = 0; i < 169; i++)
        {
            if (flagLevels[i] >= 0)
            {
                _flagObjects[i] = Instantiate(prefabFlag).GetComponent<IPuzzleObject>();
                _flagObjects[i].SetPosition(i);
                _flagObjects[i].SetLevel(flagLevels[i]);
            }
        }
    }

    public void InitializeBoxes(BoxData boxData)
    {
        for (int i = 0; i < 169; i++)
        {
            _boxObjects[i]?.Diminish();
            _boxObjects[i] = null;
        }

        for (int i = 0; i < 169; i++)
        {
            if (boxData.objects[i] >= 0)
            {
                switch (boxData.sizes[boxData.objects[i]])
                {
                    case 1:
                        _boxObjects[boxData.objects[i]] = Instantiate(prefabBox1).GetComponent<IPuzzleObject>();
                        break;
                    case 2:
                        _boxObjects[boxData.objects[i]] = Instantiate(prefabBox2).GetComponent<IPuzzleObject>();
                        break;
                    case 3:
                        _boxObjects[boxData.objects[i]] = Instantiate(prefabBox3).GetComponent<IPuzzleObject>();
                        break;
                }

                _boxObjects[boxData.objects[i]]?.SetPosition(boxData.positions[boxData.objects[i]]);
                _boxObjects[boxData.objects[i]]?.SetLevel(boxData.levels[boxData.objects[i]]);
            }
        }
    }

    public void DrawSwitches(char[] switches)
    {
        for (int i = 0; i < 169; i++)
        {
            switch (switches[i])
            {
                case 'Y':
                    _puzzleFloors[i]?.SetModel(PuzzleFloorMode.YellowUp);
                    break;
                case 'y':
                    _puzzleFloors[i]?.SetModel(PuzzleFloorMode.YellowDown);
                    break;
                case 'B':
                    _puzzleFloors[i]?.SetModel(PuzzleFloorMode.BlueUp);
                    break;
                case 'b':
                    _puzzleFloors[i]?.SetModel(PuzzleFloorMode.BlueDown);
                    break;
                default:
                    _puzzleFloors[i]?.SetModel(PuzzleFloorMode.Normal);
                    break;
            }
        }
    }

    public void PositioningPlayers(int[] playersLevel)
    {
        for (int i = 0; i < 169; i++)
        {
            _playerObjects[i]?.SetLevel(playersLevel[i]);
        }
    }

    public void MovingPlayers(int[] players)
    {
        for (int i = 0; i < 169; i++)
        {
            if (players[i] >= 0)
            {
                _playerObjects[players[i]]?.MovePosition(i);
            }
        }
    }

    public void MovingBoxes(PushedBoxData boxData)
    {
        for (int i = 0; i < 169; i++)
        {
            if (boxData.indexes[i] >= 0)
            {
                _boxObjects[boxData.indexes[i]]?.MovePosition(boxData.positions[boxData.indexes[i]]);
            }
        }
    }

    public void FallingBoxes(FallBoxData boxData)
    {
        for (int i = 0; i < 169; i++)
        {
            if (boxData.indexes[i] >= 0)
            {
                _boxObjects[boxData.indexes[i]]?.SetLevel(boxData.levels[boxData.indexes[i]]);
            }
        }
    }

    public void ElevateFloors(int[] floorLevels)
    {
        for (int i = 0; i < 169; i++)
        {
            _puzzleFloors[i]?.Elevate(floorLevels[i]);
        }
    }

    public void ElevatePlayers(int[] playerLevels)
    {
        for (int i = 0; i < 169; i++)
        {
            _playerObjects[i]?.Elevate(playerLevels[i]);
        }
    }

    public void ElevateFlags(int[] flagLevels)
    {
        for (int i = 0; i < 169; i++)
        {
            if (flagLevels[i] >= 0)
            {
                _flagObjects[i]?.Elevate(flagLevels[i]);
            }
        }
    }

    public void ElevateBoxes(ElevateBoxData boxData)
    {
        for (int i = 0; i < 169; i++)
        {
            if (boxData.indexes[i] >= 0)
            {
                _boxObjects[boxData.indexes[i]]?.Elevate(boxData.levels[boxData.indexes[i]]);
            }
        }
    }

    public void GetFlag(int index)
    {
        _flagObjects[index]?.Diminish();
        _flagObjects[index] = null;
    }

    public void ClearField()
    {
        for (int i = 0; i < 169; i++)
        {
            _puzzleFloors[i]?.Diminish();
            _playerObjects[i]?.Diminish();
            _flagObjects[i]?.Diminish();
            _boxObjects[i]?.Diminish();

            _puzzleFloors[i] = null;
            _playerObjects[i] = null;
            _flagObjects[i] = null;
            _boxObjects[i] = null;
        }
    }
}
