﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class game_controller : MonoBehaviour {

    public Grid battlefield;

    public Camera mainCam;

    public GameObject action_text;
    public GameObject StatusNo;
    bool turnOver = false;

    public GameObject camera;
    bool toggleCam = false;
    public int gamePhase = 0; // 1 = Ship placement, 2 = mainphase

    Player player1;
    AI player2;

    Player currentPlayerTurn;
    
    // Only type that changes is "hot seat" atm...
    public string game_type;

    Vector3 attackCoordinate;
    bool attacking = false;
    //GUI
    public GameObject attackCanvas;
    public Text status;  //Currently displays player's turn 
    public GameObject turn_indicator; // Player Transition Screen
    public bool between_turns = true;

    public GameObject bullet;

    GameObject selected_ship;
    Ship selected_ship_script;
    Renderer selected_ship_render;



    // Use this for initialization
    void Start () {
        status.text = "Turn: Player 1";
        player1 = gameObject.AddComponent<Player>();
        player1.id = 1;
        player2 = gameObject.AddComponent<AI>();
        player2.id = 2;
        Application.targetFrameRate = 60;
        currentPlayerTurn = player1;
        // Firt turn needs actions or they just have to end turn
        currentPlayerTurn.actions = 3;

        // Turn indicator for hot seat
        if (game_type == "hot seat")
        {
            turn_indicator.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        if(currentPlayerTurn.actions == 0 && !turnOver)
        {
            StatusNo.SetActive(true);
            turnOver = true; 
        }

        if (between_turns)
            Hide_Everything();

        if (Input.GetMouseButtonDown(0)) //This whole bit handles selecting ships now.
        {
            bool found = false;
            RaycastHit[] hits;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 100.0f);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.transform.tag == "Ship" && !found && !attacking) //Tags ships
                {
                    // Check for clicking invisble ships
                    if (hit.transform.Find("Shape").gameObject.activeInHierarchy)
                    {
                        if (selected_ship_script != null)
                        {
                            UnselectShip();
                        }
                        found = true;

                        selected_ship_script = hit.transform.GetComponent<Ship>();
                        selected_ship = hit.transform.gameObject;
                        selected_ship_render = selected_ship.GetComponentInChildren<Renderer>();
                        selected_ship_render.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                        selected_ship_script.selected = true;
                        break;
                    }
                }

                if (attacking && hit.transform.tag == "Cell") //Tags ships
                {

                    // Clicking a highlighted cell
                    int _x = (int)(hit.point.x);
                    int _y = (int)(hit.point.y);
                    int _z = (int)(hit.point.z);
                    LaunchAttack(_x, _y, _z);
                }

            }
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!found && selected_ship_script != null)
                {
                    UnselectShip();
                }
            }


        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selected_ship_script != null)
            {
                UnselectShip();
            }
        }
    }

    public void UnselectShip()
    {

        selected_ship_render.material.shader = Shader.Find("Standard");
        selected_ship = null;
        selected_ship_render = null;
        selected_ship_script.selected = false;
        selected_ship_script = null;

    }

    public void camera_change()
    {
        
        if(toggleCam)
        {
            camera.transform.position = new Vector3(-9, 11, 8);
            camera.transform.Rotate(49,180,0);
            toggleCam = false;
        }
        else
        {
            camera.transform.position = new Vector3(17, 11, 8);
            camera.transform.Rotate(49, 180, 0);
            toggleCam = true;
        }

    }

    public void turn_change()
    {
        // Between Turns
        between_turns = false;
        if(selected_ship_script != null)
        {
            UnselectShip();
        }

        BetweenTurnView();

        if (currentPlayerTurn.id == 1)
        {
            status.text = "Turn: Player 2";
            currentPlayerTurn = player2;
            currentPlayerTurn.actions = 3;
            player2.is_turn = true;

            StatusNo.SetActive(false);


        }
        else
        {
            status.text = "Turn: Player 1";
            currentPlayerTurn = player1;
            player2.is_turn = false;
            currentPlayerTurn.actions = 3;
            StatusNo.SetActive(false);

        }
        turnOver = false;
        action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;

        foreach (Ship ship in battlefield.ships)
        {
            if (ship.player != currentPlayerTurn.id)
                ship.can_select = false;
            else
                ship.can_select = true;
        }

        // Hides information from player, still needs to disable clicking on ships you can't see and
        //  an inbetween ok button to toggle correctly
        if (game_type == "hot seat")
        {
            Hide_Everything();
            Change_Player_View(currentPlayerTurn.id);
        }

        UpdatePlayerVision();
        UpdateCellViewValues();

    }

    public void Hide_Everything()
    {
        foreach(Ship ship in battlefield.ships)
        {
            ship.transform.Find("Shape").gameObject.SetActive(false);
        }
    }

    public void Change_Player_View(int player)
    {
        foreach(Ship ship in battlefield.ships)
        {
            if(ship.player == player)
                ship.transform.Find("Shape").gameObject.SetActive(true);
        }
    }
    // Clears board for an end of turn transition
    public void BetweenTurnView()
    {
        // On change of turn, hide all information
        foreach (Cell c in battlefield.cell_list)
        {
            c.visible = 0;
        }
    }

    // updates the view of the player 
    public void UpdateCellViewValues()
    {
        // Change cell visibility
        // For each ship on the battle field
        foreach (Ship ship in battlefield.ships)
        {
            if (ship.player == currentPlayerTurn.id)
            {
                // For each ship component on that ship
                foreach (Grid.Coordinate coord in battlefield.GetShipOccupation(ship.ship_number))
                {
                    // For all cells in range of that component
                    foreach (Grid.Coordinate coord1 in 
                        battlefield.GetCellsInRange(coord, battlefield.cell_list[coord.x, coord.y, coord.z].current_occupant.GetComponent<ShipComponent>().vision))
                    {
                        battlefield.cell_list[coord1.x, coord1.y, coord1.z].visible = currentPlayerTurn.id;
                    }
                }
            }
        }
    }

    public void UpdatePlayerVision()
    {
        // Update Vision
        foreach (Cell cell in battlefield.cell_list)
        {
            //Debug.Log(cell.current_occupant);
            if (cell.current_occupant != null)
            {
                if (cell.current_occupant.GetComponent<ShipComponent>().hit)
                    cell.transform.Find("Cube").gameObject.SetActive(true);
            }

            if (cell.visible == currentPlayerTurn.id && cell.current_occupant != null)
            {
                if (cell.current_occupant.GetComponentInParent<Ship>().player != currentPlayerTurn.id)
                    cell.transform.Find("Cube").gameObject.SetActive(true);
                
            }
            if(cell.current_occupant != null)
                if (cell.current_occupant.GetComponentInParent<Ship>().player == currentPlayerTurn.id && !cell.current_occupant.hit)
                    cell.transform.Find("Cube").gameObject.SetActive(false); 
        }
    }

    public void moveForward(int x)
    {
        if (currentPlayerTurn.actions != 0)
        {
            if (selected_ship_script != null)
            {
                if (Can_Move(selected_ship_script.transform.forward * x, this.selected_ship_script))
                {
                    Vector3 forward = selected_ship_script.transform.forward; //Grabs the direction //basically useless, but may come in handy 
                    if (selected_ship_script.player == currentPlayerTurn.id)
                    {
                        selected_ship_script.transform.Translate(0, 0, x); //Always moves in the Z-axis
                        currentPlayerTurn.actions--;
                        action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;
                        
                        UpdateCellViewValues();
                    }
                    else
                    {
                        selected_ship_script.ship_information.text = "That is not your ship.";
                    }

                }
            }
        }
    }

    public void moveShipUp(int x)
    {

        if (currentPlayerTurn.actions != 0)
        {
            if (selected_ship_script != null)
            {
                if (Can_Move(selected_ship_script.transform.up * x, this.selected_ship_script))
                {
                    Vector3 forward = selected_ship_script.transform.forward; //Grabs the direction //basically useless, but may come in handy 
                    if (selected_ship_script.player == currentPlayerTurn.id)
                    {
                        selected_ship_script.transform.Translate(0, x, 0); //Always moves in the Z-axis
                        currentPlayerTurn.actions--;
                        action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;
                        
                        UpdateCellViewValues();
                    }
                    else
                    {
                        selected_ship_script.ship_information.text = "That is not your ship.";
                    }
                }
            }
        }
    }
    // Right: x = 1
    // Left: x = 2
    public void turnShip(int x)
    {

        if (currentPlayerTurn.actions != 0)
        {
            if (selected_ship_script != null)
            {
                if (Can_Move(selected_ship_script.transform.right * x, this.selected_ship_script))
                {
                    Vector3 forward = selected_ship_script.transform.forward; //Grabs the direction //basically useless, but may come in handy 
                    if (selected_ship_script.player == currentPlayerTurn.id)
                    {
                        selected_ship_script.transform.Translate(x, 0, 0); //Always moves in the Z-axis
                        currentPlayerTurn.actions--;
                        action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;
                        
                        UpdateCellViewValues();
                    }
                    else
                    {
                        selected_ship_script.ship_information.text = "That is not your ship.";
                    }
                }
            }
        }
        /*
        if (currentPlayerTurn.actions != 0)
        {
            if (selected_ship_script != null)
            {
                if (selected_ship_script.player == currentPlayerTurn.id)
                {
                    if (x == 1)
                    {
                        selected_ship_script.transform.Rotate(0, 90, 0);
                    }
                    if (x == 2)
                    {
                        selected_ship_script.transform.Rotate(0, -90, 0);
                    }

                    currentPlayerTurn.actions--;

                }
                else
                {
                    selected_ship_script.ship_information.text = "That is not your ship.";
                }

            }
        }
        */
    }
    //x = 1: Mine
    //x = 2: Probe
    public void Attack(int x)
    {
        
        if (currentPlayerTurn.actions != 0 && selected_ship_script.can_move)
        {
            attacking = true;
            attackCanvas.SetActive(true);
            if (selected_ship_script != null && selected_ship_script.player == currentPlayerTurn.id)
            {
                attackCoordinate = selected_ship_script.transform.position;
            }
        }
        else
        {
            //TODO: NO more actions allowed this turn
        }
        
    }

    public void LaunchAttack(int xcoor, int ycoor, int zcoor)
    {
        attacking = false;
        
        if (battlefield.Contains(xcoor, ycoor, zcoor) && selected_ship_script != null)
        {
            //Debug.Log(selected_ship.transform.position);

            GameObject temp = Object.Instantiate(bullet, selected_ship_script.transform.position, Quaternion.identity, GameObject.Find("MineYard").transform) as GameObject;

            temp.GetComponent<Shoot>().Initialize(15, selected_ship_script.transform.position, new Vector3(xcoor + .5f, ycoor + .5f, zcoor + .5f), currentPlayerTurn.id);
            temp.name = "currentlyActiveMine";
            currentPlayerTurn.actions--;
            action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;

        }
        else
        {
            //TODO: "COORDINATES ARE NOT IN RANGE"
        }
        attackCanvas.SetActive(false);
    }

    public void LaunchAttack()
    {
        attacking = false;
        Text xcoordinate = GameObject.Find("X_attack").GetComponentInChildren<Text>();
        Text ycoordinate = GameObject.Find("Y_attack").GetComponentInChildren<Text>();
        Text zcoordinate = GameObject.Find("Z_attack").GetComponentInChildren<Text>();
        Debug.Log(xcoordinate.text + " " + ycoordinate.text + " " + zcoordinate.text);
        int x = -1;
        int y = -1;
        int z = -1;

        int.TryParse(xcoordinate.text, out x);
        int.TryParse(ycoordinate.text, out y);
        int.TryParse(zcoordinate.text, out z);

        if(battlefield.Contains(x,y,z))
        {

            GameObject temp = Object.Instantiate(bullet, selected_ship.transform.position, Quaternion.identity, GameObject.Find("MineYard").transform) as GameObject;

            temp.GetComponent<Shoot>().Initialize(10,selected_ship.transform.position, new Vector3(x + .5f,y+.5f,z+.5f), currentPlayerTurn.id);
            temp.name = "currentlyActiveMine";
            currentPlayerTurn.actions--;
            action_text.GetComponent<Text>().text = "" + currentPlayerTurn.actions;
        }
        else
        {
            //TODO: "COORDINATES ARE NOT IN RANGE"
        }
        attackCanvas.SetActive(false);
        
    }

    public void Hot_Seat_Between_Turns()
    {
        if (game_type != "hot seat")
            return;

        if (between_turns)
        {
            between_turns = false;
            Change_Player_View(currentPlayerTurn.id);
            UpdatePlayerVision();
           
            /*if (currentPlayerTurn.id == 2)
            {
                Ship temp = null;
                foreach (Ship ship in battlefield.ships)
                {
                    if (ship.player == 2)
                    {
                        temp = ship;
                        break;
                    }
                }

                selected_ship_script = temp;
                selected_ship_script.selected = true;


                float[] coords = player2.getCoordinates();
                
                selected_ship = selected_ship_script.transform.gameObject;
                selected_ship_render = selected_ship.GetComponentInChildren<Renderer>();
                selected_ship_render.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                selected_ship_script.selected = true;


                //LaunchAttack((int)coords[0], (int)coords[1], (int)coords[2]);
                //selected_ship_script = temp;
                //coords = player2.getCoordinates();
                //LaunchAttack((int)coords[0], (int)coords[1], (int)coords[2]);
                //selected_ship_script = temp;
                //coords = player2.getCoordinates();
                //LaunchAttack((int)coords[0], (int)coords[1], (int)coords[2]);
                
            }
            */
        }
        else
        {
            turn_indicator.SetActive(true);
            turn_indicator.GetComponentInChildren<Text>().text = "Player " + currentPlayerTurn.id + "'s Turn";
            between_turns = true;
        }
    }

    public bool Can_Move(Vector3 destination, Ship ship)
    {
        // Check each moving block
        if (ship.can_move)
        {
            Debug.Log("DESTINATION: " + destination);
            foreach (Grid.Coordinate coord in battlefield.GetShipOccupation(ship.ship_number))
            {
                // Destination
                Grid.Coordinate moving_to = new Grid.Coordinate(coord.x + (int)destination.x, coord.y + (int)destination.y, coord.z + (int)destination.z);
                // Check if destination exists on Grid: in case it might move off the grid
                Debug.Log(moving_to.z);
                if (battlefield.Contains(moving_to.x, moving_to.y, moving_to.z))
                {
                    // Check if ship moving does not bump into a component of a different ship
                    if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant != null)
                    {
                       
                        // Check if ship number is the same, if not illegal move would happen.
                        if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant.GetComponentInParent<Ship>().ship_number != ship.ship_number)
                        {
                            Debug.Log("Yo");
                            return false;
                        }
                    }
                    // If Cell doesn't have another ship component, still check if it is occupied: Maybe a mine or a prob for instance
                    if (battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].occupied == true && battlefield.cell_list[moving_to.x, moving_to.y, moving_to.z].current_occupant == null)
                        return false;
                }
                else
                    return false;
            }

        }
        else
            return false;
        return true;
    }
}
