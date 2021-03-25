using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public interface IMainFieldDrawer
{
    public void InitializeFloors(List<MainFieldFloor> floors);
    public void InitializePlayers(List<MainFieldPlayer> players);
    public void InitializeFlags(List<MainFieldFlag> flags);
    public void InitializeBoxes(List<MainFieldBox> boxes);
    public void MovePlayers(List<MainFieldPlayer> players);
    public void MoveBoxes(List<MainFieldBox> boxes);
    public void FallPlayers(List<MainFieldPlayer> players);
    public void FallBoxes(List<MainFieldBox> boxes);
    public void ElevateFloors(List<MainFieldFloor> floors);
    public void ElevatePlayers(List<MainFieldPlayer> players);
    public void ElevateFlags(List<MainFieldFlag> flags);
    public void ElevateBoxes(List<MainFieldBox> boxes);
    public void UpdateFloors(List<MainFieldFloor> floors);
    public void UpdateFlags(List<MainFieldFlag> flags);
    public void ClearField();
    public void DrawMap(EditorMap editorMap);
    public void ChangeFloorHeight(int floorHeight);
}
public class MainFieldDrawer : MonoBehaviour, IMainFieldDrawer
{
    [SerializeField] private GameObject prefabFloor;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabFlag;
    [SerializeField] private GameObject prefabBox;

    private readonly List<IPuzzleFloor> _floors = new List<IPuzzleFloor>();
    private readonly List<IPuzzlePlayer> _players = new List<IPuzzlePlayer>();
    private readonly List<IPuzzleFlag> _flags = new List<IPuzzleFlag>();
    private readonly List<IPuzzleBox> _boxes = new List<IPuzzleBox>();

    private int _floorHeight;
    
    public void InitializeFloors(List<MainFieldFloor> floors)
    {
        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].Diminish();
        }
        _floors.Clear();

        for (int i = 0; i < floors.Count; i++)
        {
            IPuzzleFloor floor = Instantiate(prefabFloor).GetComponent<IPuzzleFloor>();
            floor.Initialize(floors[i], _floorHeight);
            _floors.Add(floor);
        }
    }

    public void InitializePlayers(List<MainFieldPlayer> players)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Diminish();
        }
        _players.Clear();

        for (int i = 0; i < players.Count; i++)
        {
            IPuzzlePlayer player = Instantiate(prefabPlayer).GetComponent<IPuzzlePlayer>();
            player.Initialize(players[i], _floorHeight);
            _players.Add(player);
        }
    }

    public void InitializeFlags(List<MainFieldFlag> flags)
    {
        for (int i = 0; i < _flags.Count; i++)
        {
            _flags[i].Diminish();
        }
        _flags.Clear();

        for (int i = 0; i < flags.Count; i++)
        {
            IPuzzleFlag flag = Instantiate(prefabFlag).GetComponent<IPuzzleFlag>();
            flag.Initialize(flags[i], _floorHeight);
            _flags.Add(flag);
        }
    }

    public void InitializeBoxes(List<MainFieldBox> boxes)
    {
        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].Diminish();
        }
        _boxes.Clear();

        for (int i = 0; i < boxes.Count; i++)
        {
            IPuzzleBox box = Instantiate(prefabBox).GetComponent<IPuzzleBox>();
            box.Initialize(boxes[i], _floorHeight);
            _boxes.Add(box);
        }
    }

    public void MovePlayers(List<MainFieldPlayer> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            _players[i].Move(players[i].position);
        }
    }

    public void MoveBoxes(List<MainFieldBox> boxes)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            _boxes[i].Move(boxes[i].position);
        }
    }

    public void FallPlayers(List<MainFieldPlayer> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            _players[i].Fall(players[i].level);
        }
    }

    public void FallBoxes(List<MainFieldBox> boxes)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            _boxes[i].Fall(boxes[i].level);
        }
    }

    public void ElevateFloors(List<MainFieldFloor> floors)
    {
        for (int i = 0; i < floors.Count; i++)
        {
            _floors[i].Elevate(floors[i].level);
        }
    }

    public void ElevatePlayers(List<MainFieldPlayer> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            _players[i].Elevate(players[i].level);
        }
    }

    public void ElevateFlags(List<MainFieldFlag> flags)
    {
        for (int i = 0; i < flags.Count; i++)
        {
            _flags[i].Elevate(flags[i].level);
        }
    }

    public void ElevateBoxes(List<MainFieldBox> boxes)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            _boxes[i].Elevate(boxes[i].level);
        }
    }

    public void UpdateFloors(List<MainFieldFloor> floors)
    {
        for (int i = 0; i < floors.Count; i++)
        {
            _floors[i].Draw(floors[i]);
        }
    }

    public void UpdateFlags(List<MainFieldFlag> flags)
    {
        for (int i = 0; i < flags.Count; i++)
        {
            _flags[i].Draw(flags[i].doesExist);
        }
    }

    public void ClearField()
    {
        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].Diminish();
        }
        
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Diminish();
        }
        
        for (int i = 0; i < _flags.Count; i++)
        {
            _flags[i].Diminish();
        }
        
        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].Diminish();
        }
        
        _floors.Clear();
        _players.Clear();
        _flags.Clear();
        _boxes.Clear();
    }

    public void DrawMap(EditorMap editorMap)
    {
        List<MainFieldFloor> floors = new List<MainFieldFloor>();
        List<MainFieldPlayer> players = new List<MainFieldPlayer>();
        List<MainFieldFlag> flags = new List<MainFieldFlag>();
        List<MainFieldBox> boxes = new List<MainFieldBox>();
        
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
                    players.Add(player);
                    break;
                case 'F':
                    flags.Add(flag);
                    break;
                case '1':
                    box.size = 1;
                    boxes.Add(box);
                    break;
                case '2':
                    box.size = 2;
                    boxes.Add(box);
                    break;
                case '3':
                    box.size = 3;
                    boxes.Add(box);
                    break;
            }

            switch (editorMap.floors[i])
            {
                case 'W':
                    floor.floorColor = FloorColor.White;
                    floors.Add(floor);
                    break;
                case 'Y':
                    floor.floorColor = FloorColor.Yellow;
                    floors.Add(floor);
                    break;
                case 'B':
                    floor.floorColor = FloorColor.Blue;
                    floors.Add(floor);
                    break;
            }
        }
        InitializeFloors(floors);
        InitializePlayers(players);
        InitializeFlags(flags);
        InitializeBoxes(boxes);
    }
    public void ChangeFloorHeight(int floorHeight)
    {
        _floorHeight = floorHeight;
        
        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].ChangeFloorHeight(floorHeight);
        }
        
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].ChangeFloorHeight(floorHeight);
        }
        
        for (int i = 0; i < _flags.Count; i++)
        {
            _flags[i].ChangeFloorHeight(floorHeight);
        }
        
        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].ChangeFloorHeight(floorHeight);
        }
    }
}
