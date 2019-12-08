using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class MazeSwap : MonoBehaviour
{

    public KMBombModule module;
    public GameObject defuser; // Down/Up is z
    public GameObject goal;
    public GameObject indG;
    public GameObject indC;
    public GameObject strikeWallX;
    public GameObject strikeWallY;
    public KMSelectable[] buttons; // 0 = U, 1 = D, 2 = L, 3 = R
    public Material lime;
    public Material cyan;

    const float left = 0.0675f;
    const float top = -0.0344f;
    const float distance = .021f;

    private static string[][] mazes =         // Borders are not walls
    {
        new string[36] // 1
        {
            " ", "S", " ", "S", "S", " ",
            "OS", "NE", "WE", "WN", "NE", "W",
            "N", "SE", "WE", "WE", "WS", "OS",
            "S", "NE", "WE", "W", "NS", "NS",
            "N", "SE", "WE", "WSE", "ESN", "N",
            " ", "NE", "W", "N", "N", " ",
        },
        new string[36] // 2
        {
            " ", "SE", "W", "S", "S", " ",
            " ", "NS", "OSE", "WNE", "WN", " ",
            " ", "NS", "NS", "E", "WE", "W",
            "E", "WN", "NE", "OWSE", "WE", "W",
            "E", "WE", "WSE", "WN", "SE", "W",
            "E", "W", "N", "E", "WN", " ",
        },
        new string[36] // 3
        {
            "S", "S", "S", " ", " ", " ",
            "N", "NS", "NE", "OWSE", "WE", "W",
            "S", "NE", "WE", "WNE", "WE", "WS",
            "N", "SE", "WS", "SE", "W", "N",
            "S", "NS", "NS", "NE", "WSE", "W",
            "N", "N", "N", " ", "ON", " ",
        },
        new string[36] // 4
        {
            " ", "S", "E", "WS", "S", "O",
            "E", "WNE", "WS", "NS", "NE", "W",
            "E", "WE", "WN", "NS", "SE", "W",
            "E", "WS", "SE", "WN", "N", " ",
            "E", "WNS", "NS", "OSE", "WE", "W",
            " ", "N", "N", "N", "E", "W",
        },
        new string[36] // 5
        {
            "O", "OSE", "W", "S", "S", " ",
            "S", "NS", "S", "NS", "NSE", "W",
            "N", "NS", "NE", "WN", "NS", "S",
            "E", "WNS", "SE", "WE", "WN", "N",
            "E", "WN", "NE", "WS", "SE", "W",
            " ", "E", "W", "N", "N", " ",
        },
        new string[36] // 6
        {
            " ", "SE", "W", " ", "S", "S",
            "E", "WN", "SE", "WS", "NS", "ON",
            " ", "SE", "WN", "NS", "NSE", "W",
            "E", "OWNS", "S", "NE", "WNS", "S",
            "E", "WN", "NS", "S", "NS", "N",
            " ", "E", "WN", "N", "N", " ",
        },
        new string[36] // 7
        {
            " ", "S", " ", "S", "S", "S",
            "S", "NE", "WE", "WN", "NS", "N",
            "N", "SE", "WSE", "W", "NSE", "W",
            "OE", "WN", "NS", "SE", "WN", "S",
            "E", "WE", "WN", "NE", "WSE", "WN",
            " ", " ", "E", "W", "N", "O",
        },
        new string[36] // 8
        {
            "S", " ", "S", "OS", "S", " ",
            "N", "OSE", "WN", "NS", "NE", "W",
            "S", "NE", "WS", "NE", "WE", "W",
            "N", "S", "NE", "WE", "WE", "W",
            " ", "NE", "WSE", "WE", "WE", "W",
            "E", "W", "N", "E", "WE", "W",
        },
        new string[36] // 9
        {
            "S", "E", "WE", "WS", "S", " ",
            "N", "E", "WS", "NS", "NS", " ",
            "E", "WE", "WN", "ONS", "NS", "S",
            "E", "WSE", "WE", "WN", "ONS", "N",
            "E", "WN", "SE", "WE", "WN", "S",
            " ", " ", "N", "E", "W", "N",
        },
        new string[36] // 10
        {
            "S", "S", " ", "E", "WE", "W",
            "N", "NE", "WE", "WS", "E", "W",
            "E", "WS", "E", "WNS", "SE", "W",
            "E", "OWN", "SE", "WN", "NE", "OW",
            "E", "WS", "NSE", "WE", "WE", "W",
            " ", "N", "N", "E", "W", " ",
        },
        new string[36] // 11
        {
            "S", " ", "E", "W", "S", " ",
            "N", "SE", "WS", "SE", "WNE", "W",
            "S", "NS", "N", "NE", "W", "S",
            "N", "NE", "WSE", "WE", "WS", "NS",
            "OE", "WS", "NS", "SE", "WN", "N",
            " ", "ON", "N", "N", "E", "W",
        },
        new string[36] // 12
        {
            " ", "SE", "W", "S", "S", " ",
            " ", "NS", "SE", "WN", "ONE", "W",
            "S", "NS", "NE", "WE", "WE", "WS",
            "N", "NE", "WS", "SE", "WS", "N",
            "E", "WE", "WN", "NS", "NE", "W",
            "OE", "W", " ", "NE", "W", " ",
        },
        new string[36] // 13
        {
            " ", "S", "S", "E", "OW", " ",
            "S", "NE", "WNE", "WE", "WE", "W",
            "NE", "WE", "WE", "WS", "SE", "W",
            "E", "WS", "S", "NE", "WNS", "S",
            " ", "NS", "ONE", "WE", "WN", "N",
            " ", "NE", "W", " ", "E", "W",
        },
        new string[36] // 14
        {
            " ", "E", "W", "S", "S", " ",
            "E", "WE", "WSE", "WN", "NSE", "W",
            "OE", "WE", "WN", "E", "WN", "S",
            "E", "WS", "OSE", "WE", "WS", "N",
            "S", "NS", "NE", "WS", "NS", "S",
            "N", "N", " ", "N", "N", "N",
        },
        new string[36] // 15
        {
            " ", "S", "S", "S", "S", " ",
            "S", "NS", "NE", "WN", "NS", "S",
            "N", "NS", "OSE", "WS", "NS", "N",
            " ", "NS", "N", "NE", "WNS", "S",
            "E", "WNE", "WSE", "WS", "NS", "N",
            "E", "W", "N", "ON", "N", " ",
        }
    };
    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private string[] maze1;
    private string[] maze2;
    private string[] currentMaze;
    private int maze1Ind1;
    private int maze1Ind2;
    private int maze2Ind1;
    private int maze2Ind2;
    private int currentPos = 1;
    private int goalPos;
    private bool solved;
    private bool mazeToggle = false;
    private float rotate;

    void Start()
    {
        _moduleId = _moduleIdCounter++;
         rotate = Random.Range(-.1f, -.2f);

        currentPos = Random.Range(0, 36);
        FindPlacePos(defuser, currentPos);

        int a = Random.Range(0, 9); // picks 2 random mazes
        int b = Random.Range(0, 9);
        while (a == b) b = Random.Range(0, 15);
        maze1 = mazes[a];
        maze2 = mazes[b];
        currentMaze = maze1;

        FindPlaceInd();

        PlaceGoal();

        for (int i = 0; i < buttons.Length; i++) // Fixes the highlights on the buttons.
        {
            var position = buttons[i].transform.GetChild(0).GetChild(0).localScale;
            buttons[i].transform.GetChild(0).GetChild(0).localScale = new Vector3(position.x, 1.1f, position.z);
        }

        buttons[0].OnInteract += delegate { ButtonPressed("U"); return false; };
        buttons[1].OnInteract += delegate { ButtonPressed("D"); return false; };
        buttons[2].OnInteract += delegate { ButtonPressed("L"); return false; };
        buttons[3].OnInteract += delegate { ButtonPressed("R"); return false; };

    }

    void ButtonPressed(string buttonDirection)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        if (solved) return;

        if (buttonDirection == "U" && currentPos / 6 != 0)
        {
            if (currentMaze[currentPos].Contains("N"))
            {
                
                HandleStrike("up");
                return;
            }
            else
            {
                currentPos -= 6;
                DebugLog("moved");
            }
            
        }
        else if (buttonDirection == "D" && currentPos / 6 != 5)
        {
            if (currentMaze[currentPos].Contains("S"))
            {

                HandleStrike("down");
                return;
            }
            else
            {
                currentPos += 6;
                DebugLog("moved");
            }

        }
        if (buttonDirection == "L" && currentPos % 6 != 0)
        {
            if (currentMaze[currentPos].Contains("W"))
            {

                HandleStrike("left");
                return;
            }
            else
            {
                currentPos --;
                DebugLog("moved");
            }

        }
        if (buttonDirection == "R" && currentPos % 6 != 5)
        {
            if (currentMaze[currentPos].Contains("E"))
            {

                HandleStrike("right");
                return;
            }
            else
            {
                currentPos++;
                DebugLog("moved");
            }

        }

        defuser.transform.localPosition = getPositionVector(currentPos);
        DebugLog("Current Position: {0}", currentPos);
        DebugLog("Borders: {0}", currentMaze[currentPos]);

        if (currentPos == goalPos) HandleSolve();
        int randNum = Random.Range(0, 99);
        if (Enumerable.Range(0, 44).ToArray().Any(x => x == randNum))
            ChangeMaze();
    }

    private void ChangeMaze()
    {
        mazeToggle = !mazeToggle;
        currentMaze = mazeToggle ? maze2 : maze1;
        goal.GetComponent<Renderer>().material = mazeToggle ? cyan : lime;
    }

    private void HandleSolve()
    {
        module.HandlePass();
        solved = true;
        Destroy(defuser);
    }

    private void HandleStrike(string direction)
    {
        module.HandleStrike();
        bool wallIsX = direction.Contains("t");
        var wallCopy = direction.Contains("t") ? Instantiate(strikeWallX) : Instantiate(strikeWallY);
        wallCopy.transform.parent = strikeWallX.transform.parent;
        wallCopy.GetComponent<Renderer>().material = mazeToggle ? cyan : lime;
        wallCopy.transform.localEulerAngles = new Vector3(0f, wallIsX ? 0f : 90f, 0f);
        int pos;
        if (direction.Contains("t"))
        {
            pos = direction == "right" ? currentPos : currentPos - 1;
        }
        else
        {
            pos = direction == "down" ? currentPos : currentPos - 6;
        }

        FindPlacePos(wallCopy, pos, direction); // right and left both contain "t"
    }

    void FindPlaceInd()
    {
        var inds1 = Enumerable.Range(0, 36).Where(i => maze1[i].Contains("O")).ToArray();
        maze1Ind1 = inds1[0];
        maze1Ind2 = inds1[1];

        var inds2 = Enumerable.Range(0, 36).Where(i => maze2[i].Contains("O")).ToArray();
        maze2Ind1 = inds2[0];
        maze2Ind2 = inds2[1];

        FindPlacePos(indG, maze1Ind1);
        var copy = Instantiate(indG);
        copy.transform.parent = indG.transform.parent;
        FindPlacePos(copy, maze1Ind2);
        copy.transform.localScale = indG.transform.localScale;

        FindPlacePos(indC, maze2Ind1);
        copy = Instantiate(indC);
        copy.transform.parent = indG.transform.parent;
        FindPlacePos(copy, maze2Ind2);
        copy.transform.localScale = indC.transform.localScale;
    }

    void PlaceGoal()
    {
        var list = Enumerable.Range(0, 36).Except(new[] { currentPos, currentPos + 1, currentPos - 1, currentPos + 6, currentPos - 6, currentPos + 5, currentPos + 7, currentPos - 5, currentPos - 7, maze1Ind1, maze1Ind2, maze2Ind1, maze2Ind2 }).ToList();
        var rndIx = Random.Range(0, list.Count);
        goalPos = list[rndIx];
        FindPlacePos(goal, goalPos);
    }

    void FindPlacePos(GameObject thing, int pos, string direction = "") // Couldn't think of an algorithm :/
    { 
        thing.transform.localPosition = getPositionVector(pos, direction);
    }

    private Vector3 getPositionVector(int pos, string direction = "")
    {
        float wallX = direction.Contains("t") ? -.0102f : 0;
        float wallY = direction == "down" || direction == "up" ? .0102f : 0;

        return new Vector3((left - distance * (pos % 6)) + wallX, direction != "" ? 0.00502f : 0.00501f, (top + distance * (pos / 6)) + wallY);
    }

    private void DebugLog(string log, params object[] args)
    {
        var logData = string.Format(log, args);
        Debug.LogFormat("[Maze Swap #{0}] {1}", _moduleId, logData);
    }

    private void Update()
    {
        goal.transform.Rotate(new Vector3(0, 0, rotate));
    }
}
