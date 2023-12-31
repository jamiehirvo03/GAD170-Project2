using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public GameObject crewmatePrefab;
    public CrewmateGenerator crewmate;
    public TextDisplay textDisplay;

    private string candidateFirstName;
    private string candidateLastName;
    private string candidateHobby;

    private int randomIndex; 
    private bool isParasite;
    private bool waitForInput;
    private bool gameEnded;


    //Add list here to store new crewmates
    public List<Crewmate> crewList = new List<Crewmate>();

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        Debug.Log("A new game has been started\n");
        waitForInput = false;
        gameEnded = false;

        textDisplay.NextTurnDisable();
        textDisplay.NextTurnButton();
        textDisplay.ClearText();

        CrewmateApplication();
    }

    public void EndTurn()
    {
        //Check if all 10 crewmate spots are filled, if so, call the end game function
        if (crewList.Count == 10)
        {
            if (!gameEnded)
            {
                GameOver();
            }
            else
            {
                //This will reload the scene, resetting the game.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }
        else
        {
            NewTurn();
        }
    }
    private void NewTurn()
    {
        //Clear any UI elements to prepare for next turn
        textDisplay.ClearText();
        textDisplay.NextTurnDisable();

        CrewmateApplication();
    }

    // Update is called once per frame
    //void Update()
    //{
        //if (waitForInput)
        //{
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //  ApproveCandidate();
            //}

            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //  DeclineCandidate();   
            //}
        //}
    //}

    //Function that generates a candidates name and hobby
    private void CrewmateApplication()
    {
        HumanOrParasite();

        candidateFirstName = crewmate.GetFirstName();
        candidateLastName = crewmate.GetLastName();

        if (isParasite)
        {
            candidateHobby = crewmate.GetParasiteHobby();
        }
        else
        {
            candidateHobby = crewmate.GetHumanHobby();
        }

        Debug.Log($"Is a Parasite: {isParasite}");

        Debug.Log($"This candidates name is {candidateFirstName} {candidateLastName} , Their favourite thing to do is {candidateHobby}.\n|A| - Accept |D| - Decline");
        textDisplay.AddText($"This candidates name is {candidateFirstName} {candidateLastName} , Their favourite thing to do is {candidateHobby}.");

        waitForInput = true;
    }

    //Function that randomly decideds if the candidate is a human or a parasite
    private void HumanOrParasite()
    {
        randomIndex = Random.Range(0, 10);
        Debug.Log($"Random Index: {randomIndex}");

        if (randomIndex > 5)
        {
            isParasite = true;
        }
        else
        {
            isParasite = false;
        }
    }

    //Function for declining candidate and not adding them to the list
    public void DeclineCandidate()
    {
        if (waitForInput)
        {
            if (!isParasite)
            {
                waitForInput = false;
                Debug.Log("Candidate has been rejected!\n");
                textDisplay.AddText("\n\nCandidate has been rejected!");

                //Enable End Turn Button
                textDisplay.NextTurnEnable();
            }
            else
            {
                waitForInput = false;
                Debug.Log("Woah, I think they were a parasite!\nGood thing you rejected them");
                textDisplay.AddText("\n\nWoah, I think they were a parasite!\nGood thing you rejected them");

                textDisplay.NextTurnEnable();
            }
        }
    }

    //Function for adding candidate to crew list, while instantiating an object
    public void ApproveCandidate()
    {
        if (waitForInput)
        {
            if (!isParasite)
            {
                waitForInput = false;
                Debug.Log("Candidate has been approved!\n");
                textDisplay.AddText("\n\nCandidate has been approved!");

                //Instantiate crewmate object from prefab
                float x = Random.Range(-17f, 17f);
                float y = 0f;
                float z = Random.Range(7f, 10f);
                Vector3 spawnPosition = new Vector3(x, y, z);
                GameObject clone = Instantiate(crewmatePrefab, spawnPosition, Quaternion.identity) as GameObject;
                crewList.Add(clone.GetComponent<Crewmate>());

                //Enable End Turn Button
                textDisplay.NextTurnEnable();
            }
            else
            {
                if (crewList.Count > 0)
                {
                    CrewmateMurderTime();
                }
                else
                {
                    Debug.Log($"{candidateFirstName} {candidateLastName} was secretly a parasite! \nThey couldn't find any humans to kill so they got bored and left!");
                    textDisplay.AddText($"\n\n{candidateFirstName} {candidateLastName} was secretly a parasite! \nThey couldn't find any humans to kill so they got bored and left!");

                    waitForInput = false;

                    //Enable End Turn Button
                    textDisplay.NextTurnEnable();
                }
            }
        }
       
    }

    public string GetFirstName()
    {
        return candidateFirstName;
    }

    public string GetLastName()
    {
        return candidateLastName;
    }

    public string GetHobby()
    {
        return candidateHobby;
    }

    //Function for eliminating crewmates if a parasite is accepted (all crewmates with a randomly picked hobby)
    private void CrewmateMurderTime()
    {
        //Randomly pick a hobby from current crewmates
        string targetHobby = crewList[(Random.Range(0, crewList.Count))].GetComponent<Crewmate>().hobby;

        Debug.Log($"{candidateFirstName} {candidateLastName} was secretly a parasite! \nThey killed everyone who enjoys {targetHobby}");
        textDisplay.AddText($"\n\n{candidateFirstName} {candidateLastName} was secretly a parasite! \nThey killed everyone who enjoys {targetHobby}");

        //Remove crewmates from list (loop through collection and check 'if' any have the same hobby)

        for (int i = 0; i < crewList.Count; i++)
        {
            //Check each item in the list for the targeted hobby
            //Remove each 'targeted' crewmate and destroy their objects 

            if (crewList[i].GetComponent<Crewmate>().hobby == targetHobby)
            {
                crewList[i].GetComponent<Crewmate>().KillCrewmate();
            }
        }

        //Enable End Turn Button
        textDisplay.NextTurnEnable();

    }
    public void RemoveCrewmate(Crewmate crewmateToRemove)
    {
        crewList.Remove(crewmateToRemove);
    }
  
    //Function that ends the game, showing a final screen displaying all final crew members and their hobbies
    private void GameOver()
    {
        textDisplay.ClearText();
        
        Debug.Log("You have Successfully Filled all 10 spots!");
        textDisplay.AddText("You have Successfully Filled All 10 spots!:\n");

        //For loop that will check each item in it for crewmate info
        for (int i = 0; i < crewList.Count; i++)
        {
            textDisplay.AddText($"\n{crewList[i].GetComponent<Crewmate>().firstName} {crewList[i].GetComponent<Crewmate>().lastName}   Favourite Hobby: {crewList[i].GetComponent<Crewmate>().hobby}");
        }
        
        textDisplay.NewGameButton();

        gameEnded = true;
    }
}
