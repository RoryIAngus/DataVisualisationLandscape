// code from https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
// additional code created by Rory Angus and available here - https://github.com/RoryIAngus/DataVisualisationLandscape

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Instantiation : MonoBehaviour
{

    public GameObject brick;
    public GameObject cylinder;
    public GameObject pill;
    public GameObject pyramid;
    public GameObject sphere;
    private GameObject shape;  // Variable to hold the shape that we want to create

    // Instantiates a prefab in a grid

    public float spacing = 2f;  // How much space between each object
    public float x_scale = 1f;  // How big to scale the object in the x direction
    public float y_scale = 1f;  // How big to scale the object in the y direction
    public float z_scale = 1f;  // How big to scale the object in the z direction

    //variables to work with the csvfiles
    private read_file csvfile;  // Holds the csv file that is used as the source of the information
    private int csv_num_rows = 0;  // num of rows in the csv
    private int csv_num_avenue = 0;  //Holds the number of avenues
    private string shape_building = "";  // Holds the value of the shape that we want to display
    private string shape_colour = "";  //Holds the value of the colour we want to set the building to
    private float shape_height = 0f; //Holds the value of the hight we want to set the building to
    private List<read_file.Row> rows_on_avenue_list;  //create a list variable 
    private float ground_adjust = 1f;  //hold the value to move the object by depending on the type of shape
    private Color building_colour = Color.white;  //hold the final colour of the object


    void Start()
    {
        //create the object to hold the CSV file	
        csvfile = gameObject.GetComponent<read_file>();

        // Create a hashstring to hold the unique values of Avenue
        HashSet<string> csv_avenues = new HashSet<string>();

        //find out the number of rows in the csv file
        csv_num_rows = csvfile.NumRows();

        //loop through all rows to get a distinct list of the avenues 
        //Note - A hashSet only allows distinct values to be added
        for (int i = 0; i < csv_num_rows; i++)
        {
            csv_avenues.Add(csvfile.GetAt(i).Avenue);
        }

        csv_num_avenue = (csv_avenues.Count);  //Get the total number of Avenues by counting the unique elements in the hashset
                                               // Convert the avenues from the hash to a list so they can be indexed and retrieved

        List<string> avenues_list = csv_avenues.ToList();

        //loop through the number of buildings
        for (int y = 0; y < csv_num_avenue; y++)
        {
            //as the loop steps through each avenue it uses the index to get the avenues associated rows and puts them in a list
            rows_on_avenue_list = csvfile.FindAll_Avenue(avenues_list[y]);

            //counts the total number or rows (or buildings for the avenue) to determine how long each avenue will be
            csv_num_rows = rows_on_avenue_list.Count();

            //loop through the number of avenues
            for (int x = 0; x < csv_num_rows; x++)
            {

                // read the shape, colour and height for each row
                shape_building = rows_on_avenue_list[x].Building;  //Building = Shape
                shape_colour = rows_on_avenue_list[x].Tenant;  //Tenant = Colour
                shape_height = float.Parse(rows_on_avenue_list[x].Floors);  //Floors = Height - need to convert string to int

                // ----------------- Select the right shape -----------------
                // assign the correct prefabrication depending on the rows shape.
                // not all shapes have the middle set to ground level

                if (shape_building == "Square")
                {
                    shape = brick;
                    ground_adjust = 2.0f;  //value needed for bricks
                }
                else if (shape_building == "Pyramid")
                {
                    shape = pyramid;
                    ground_adjust = 2.0f;  //value needed for pyramids
                }
                else if (shape_building == "Pill")
                {
                    shape = pill;
                    ground_adjust = 1.0f;  //value needed for pill
                }
                else if (shape_building == "Cylinder")
                {
                    shape = cylinder;
                    ground_adjust = 1.0f;  //value needed for cylinder
                }
                else  // if error then use sphere
                {
                    shape = sphere;
                }

                // ----------------- Select the right colour -----------------
                // assign the correct prefabrication depending on the rows shape.
                if (shape_colour == "Red")
                {
                    building_colour = Color.red;
                }
                else if (shape_colour == "Green")
                {
                    building_colour = Color.green;
                }
                else if (shape_colour == "Blue")
                {
                    building_colour = Color.blue;
                }
                else if (shape_colour == "Yellow")
                {
                    building_colour = Color.yellow;
                }
                else  //if error use black
                {
                    building_colour = Color.black;
                }


                //Create the vector location using the x and y coords - Set the Z based on the height of the object
                Vector3 pos = new Vector3(y * spacing, ((shape_height * z_scale) / ground_adjust), x * spacing);    // the position of the y and x determine which way the array faces after it is built 
                                                                                                            // divide by 2 on the height to make sure the bases are level
                                                                                                            // only use spacing on the X & Y coordinates not z otherwise it gets the height wrong

                // use that shape variable to build the object
                // this is the section of code that create the actual object and places it in space
                // uses the vector that was defined above to place it in the correct location in space

                GameObject shape_go = Instantiate(shape, pos, Quaternion.identity) as GameObject;

                // set the colour of the object
                shape_go.GetComponent<MeshRenderer>().material.color = building_colour;

                // set the hight of the object

                shape_go.transform.localScale = new Vector3(x_scale, shape_height * z_scale, y_scale);  // Change the size of the shape using the height from the file and the other 3 dimensions to scale the shape


            }
        }
    }

}