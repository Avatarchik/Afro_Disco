﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutController : MonoBehaviour {

    public Transform tutorialUIRoot;

    private int Stage = -1; //index for the tutorial step

    public Text speech;
    public Transform afrodotjpeg;
    public Button advanceButt;
    public Button endTurn;
    public Button Conga;

    public float speed;
    public float jiggleMagnitude;
    private Vector2 originpos;

    public Board _board;

    public AudioClip voice;

    private Dancer lead;
    private Dancer enemy;
    private Dancer fernando;
    private Dancer johhny;

    private Animator _afroanimatior;
    private AudioSource _myAudio;

    private Transform pointyboys;

    //steps where text won't advance until something happens
    //Array value is the index of the speech line
    private readonly int[] interaction = { 6, 9, 18, 19, 22, 29, 30};
    private readonly int[] UI = { 9, 18, 19, 22, 29, 30 };

    string[] Text =
    {
        "hey kiddo. it's me. your friendly neighbourhood disco teacher",
        "so i hear you wanna beat that punk, rock mullet in a dance fight huh?", 
        "well you're gunna have to learn how to get down on the d-floor and boogie first",
        "but don't worry. stick with me and you'll be booty callin' and boogalooin' in no time",
        "lets start on your footwork first",
        //"you can't make any sweet moves unless you're bouncin' around that floor",
        "each turn you have, you can move each of your boys two spaces",
        "give that a shot now.", //I-6

        "groovy man",
        "and don't worry if you mess up",
        "it might not be very stylin', but just press the undo button to go back a step",  //I-9 UI-1

        "no biggie right?", 
        "", //11

        "uh oh",
        "there's some dude over there crampin your style",
        "we better show him who's boss",
        "", //15

        "around here, we do that by bustin' moves",
        "lets try a conga line to start off with",
        "first arrange your boys in a line aiming at your target", //I-18 //UI-2
        "and then bust that move, homie", //I-19 //UI-3

        "nice. that was a good shove",
        "lets get him out of here and knock him off the floor",
        "end our turn first, though. Since you can only perform one of each move per turn" , //I-22 //UI-4  //weirdly worded?

        "wait.",
        "now he's too far away now",
        "that's not cool",
        "hey johnny get over here", 
        "", //27

        "some moves let you extend their range by adding another dancer to the end",
        "conga is one of those moves, so lets try that now", //29 //UI-5
        "and bust that move", //30 //UI-6

        "look at him go.",
        "groovaaaay.",
        "groovay",
        "to win a dance battle knock out three other dudes, or the other lead dancer",
        "you'll know what the lead dancer looks like when you see them",
        "uh-oh",
        "looks like he's calling his boys over to the floor",
        "there are also a whole bunch of other moves you should check out",
        "try them out in practice mode, or press the help button for a quick reference",
        "i gotta bounce",
        "say hi to Rock Mullet for me",
        "good luck, kiddo."
    };

	// Use this for initialization
	void Start ()
    {
        originpos = afrodotjpeg.position;

        _myAudio = GetComponent<AudioSource>();
        _afroanimatior = tutorialUIRoot.GetComponentInChildren<Animator>();

        pointyboys = tutorialUIRoot.GetChild(0);
    }

    void OnEnable()
    {
        lead = _board.GetDancer(new Vector2(3, 1));
        if (!lead) return; //gets called on start for some reason
        lead.canMove = false;

        Invoke("Kickoff", 4);

        advanceButt.interactable = false;
        endTurn.interactable = false;
    }

    void Kickoff()
    {
        NextStep();
        _afroanimatior.SetBool("active", true);
        advanceButt.interactable = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(Stage)
        {
            case 6:
                lead.canMove = true;
                if (lead.StartRoundPos != lead.GetBoardPos() && !lead.selected)
                    NextStep();
                break;

            case 11:
                enemy = _board.SpawnDancer(false, new Vector2(4, 3), _board.Player2, "tut enemy");
                NextStep();
                break;

            case 15:
                fernando = _board.SpawnDancer(false, new Vector2(2, 1), _board.Player1, "Fernando, the dance teacher");
                fernando.canMove = true;
                _board.BakeMovement(false,false);
                NextStep();
                break;

            case 18:
                if(lead.GetBoardPos() == new Vector2(3,3) &&
                    fernando.GetBoardPos() == new Vector2(2, 3))
                {
                    NextStep();
                    _board._backStack.Clear();
                }
                break;

            case 19:
                if (enemy.StartRoundPos != enemy.GetBoardPos())
                    NextStep();
                break;

            case 22:
                if(!endTurn.interactable)
                    endTurn.interactable = true;
                break;

            case 27:
                johhny = _board.SpawnDancer(false, new Vector2(1, 1), _board.Player1, "Johhny \"rocket fingers\" Gots");
                johhny.canMove = true;
                lead.canMove = false;
                _board.BakeMovement(false, false);
                NextStep();
                break;

            case 29:
                if (johhny.GetBoardPos() == new Vector2(1, 3))
                    NextStep();
                break;

            case 30:
                if (!enemy.isDancing)
                    NextStep();
                break;
        }
	}

    public void NextStep()
    {
        if (Stage < Text.Length - 1)
        {
            Stage++;

            StartCoroutine(SNESText(Text[Stage], speed, speech, Stage));

            //Interaction
            if (interaction.Contains(Stage))
            {
                advanceButt.gameObject.SetActive(false);
            }
            else if (!advanceButt.gameObject.activeInHierarchy)
            {
                advanceButt.gameObject.SetActive(true);
            }

            //UI
            //Turn off all
            foreach (Transform t in pointyboys)
                t.gameObject.SetActive(false);

            int UIindex = System.Array.IndexOf(UI, Stage);
            if (UIindex != -1)
            {
                pointyboys.GetChild(UIindex).gameObject.SetActive(true);
            }
        }
        else //end
        {
            speech.text = "";
            Stage = -1;
            _afroanimatior.SetBool("active",false);
            Invoke("end", 1);
            advanceButt.interactable = false;
        }
    }

    void end()
    {
        GameState.me.ChangeState(eGameState.MENU);
    }

    //advances button interactions
    public void BackwasPressed()
    {
        if(Stage == interaction[1] || Stage == interaction[4])
        {
            if (Stage == interaction[4])
            {
                endTurn.interactable = false;
                _board.EndTurn(false);
            }

            NextStep();
            
        }
         
    }

    IEnumerator SNESText(string text, float speed, Text t, int currstage)
    {
        string partialText = "";
        for(int i = 0; i < text.Length; i++)
        {
            if (currstage == Stage) //Check the user hasn't skipped
            {
                partialText += text[i]; //add char by char
                t.text = partialText;
                var rand = new Vector2(Random.Range(-jiggleMagnitude, jiggleMagnitude), Random.Range(-jiggleMagnitude, jiggleMagnitude));
                afrodotjpeg.position = originpos + rand;
                _myAudio.pitch = Random.Range(0.9f, 1.1f);
                _myAudio.PlayOneShot(voice);                
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(speed);
        }
    }
}
