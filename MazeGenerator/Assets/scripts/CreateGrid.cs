using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;

    public int rows = 5;
    public int columns = 5;

    int currRow = 0;
    int currCol = 0;


    private Cell[,] grid;

    private bool hasCarvedAndHunted = false;
    void Start()
    {
        
        //make grid for algorithm.
        MakeGrid();
        //start algorithm to make a perfect maze
        StartAlgorithme();
    }

    
       
    void MakeGrid()
    {
        
        grid = new Cell[rows + 1, columns + 1];


        //rows go from 0 to (row amount) on the X axis
        for (int i = 0; i < rows; i++)
        {
            //columns go from 0 to -(column amounts) on the Z axis
            for (int j = 0; j <= columns; j++)
            {
                
                grid[i, j] = new Cell();

                //Place all the walls and the floor

                GameObject floorObj = Instantiate(floor, new Vector3(j * 1, 0, -i * 1), Quaternion.identity);
                floorObj.name = "floor(" + i + "-" + j + ")";

                GameObject wallObj = Instantiate(wall, new Vector3(j * 1, 0.6f, -i * 1 + 0.4f), Quaternion.identity);
                wallObj.name = "wall(" + i + "-" + j + ")";

                GameObject wallObj2 = Instantiate(wall, new Vector3(j * 1, 0.6f, -i * 1 - 0.4f), Quaternion.identity);
                wallObj2.name = "wall2(" + i + "-" + j + ")";

                GameObject sidewall = Instantiate(wall, new Vector3(j * 1 + 0.4f, 0.6f, -i * 1), Quaternion.Euler(0, 90, 0));
                sidewall.name = "sidewall(" + i + "-" + j + ")";

                GameObject sidewall2 = Instantiate(wall, new Vector3(j * 1 - 0.4f, 0.6f, -i * 1), Quaternion.Euler(0, 90, 0));
                sidewall2.name = "sidewall2(" + i + "-" + j + ")";

                //create entrance
                if(i == 0 && j == 0)
                {
                    Destroy(wallObj);
                    floorObj.GetComponent<Renderer>().material.color = Color.green;
                }

                //create exit
                if (i == rows - 1 && j == rows - 1)
                {
                    Destroy(wallObj2);
                    floorObj.GetComponent<Renderer>().material.color = Color.red;
                }


                //make a new cell 
                grid[i, j] = new Cell();
                
                //set walls for cell
                grid[i, j].topWall = wallObj;
                grid[i, j].rightWall = sidewall;
                grid[i, j].leftWall = sidewall2;
                grid[i, j].bottomWall = wallObj2;



                //place all objects under parent (so it doesnt fill the editor)
                floorObj.transform.parent = transform;
                wallObj.transform.parent = transform;
                wallObj2.transform.parent = transform;
                sidewall.transform.parent = transform;
                sidewall2.transform.parent = transform;
            }
        }
     }

    public void ReMakeGrid()
    {
        //destroy all gameobjects from the previous grid
        foreach(Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }

        

        // create Grid
        MakeGrid();

        //reset parameters
        currCol = 0;
        currRow = 0;
        hasCarvedAndHunted = false;

        // use algorithm
        StartAlgorithme();
    }



    //Hunt and Kill Algorithm
    void StartAlgorithme()
    {
        //set start position and set as visited
        grid[currRow, currCol].isVisited = true;


        //carve walls and hunt untill every cell is visited

        while (!hasCarvedAndHunted)
        {
            //go through the maze by destroyings cell walls until you have no neighbors left
            carveWalls();

            //search the grid for unvisited cells next to visited and break a wall between them
            HuntAndKill();
        }
        
        

        
    }

    void carveWalls()
    {
        //if not every cell is visited use the switchcase
        while (VisitedAllNeighbors())
        {
            //get random number to decide what way we go
            int ranNum = UnityEngine.Random.Range(0, 4);
            Debug.Log(ranNum);

            switch (ranNum)
            {
                //top
                case 0:
                    Debug.Log("Check up");
                    //check if its not on the top row and check if the cell above isn't already visited
                    if (CheckCellRules(currRow - 1, currCol))
                    {
                        //check for a wall to destroy
                        if (grid[currRow, currCol].topWall)
                        {
                            //destroy wal
                            Destroy(grid[currRow, currCol].topWall);
                        }


                        currRow--;
                        grid[currRow, currCol].isVisited = true;

                        //check for a wall to destroy in new cell
                        if (grid[currRow, currCol].bottomWall)
                        {
                            //destroy wal
                            Destroy(grid[currRow, currCol].bottomWall);
                        }
                    }
                    break;
                //Bottom
                case 1:
                    Debug.Log("Check Bottom");
                    //check if its not on the bottom row and check if the cell below isn't already visited
                    if (CheckCellRules(currRow + 1, currCol))
                    {
                        if (grid[currRow, currCol].bottomWall)
                        {

                            Destroy(grid[currRow, currCol].bottomWall);
                        }

                        currRow++;
                        grid[currRow, currCol].isVisited = true;

                        if (grid[currRow, currCol].topWall)
                        {

                            Destroy(grid[currRow, currCol].topWall);
                        }
                    }
                    break;
                //Left
                case 2:
                    Debug.Log("Check Left");
                    //check if its not the most left column and check if the cell on the left isn't already visited
                    if (CheckCellRules(currRow, currCol - 1))
                    {
                        if (grid[currRow, currCol].leftWall)
                        {

                            Destroy(grid[currRow, currCol].leftWall);
                        }

                        currCol--;
                        grid[currRow, currCol].isVisited = true;

                        if (grid[currRow, currCol].rightWall)
                        {

                            Destroy(grid[currRow, currCol].rightWall);
                        }
                    }
                    break;
                //Right
                case 3:
                    Debug.Log("Check right");
                    //check if its not the most left column and check if the cell on the right isn't already visited
                    if (CheckCellRules(currRow, currCol + 1))
                    {
                        if (grid[currRow, currCol].rightWall)
                        {

                            Destroy(grid[currRow, currCol].rightWall);
                        }

                        currCol++;
                        grid[currRow, currCol].isVisited = true;

                        if (grid[currRow, currCol].leftWall)
                        {

                            Destroy(grid[currRow, currCol].leftWall);
                        }
                    }
                    break;

            }
        }
    }

    void HuntAndKill()
    {
        hasCarvedAndHunted = true;
        //loop trough the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //if there are unvisited cells
                if (!grid[i, j].isVisited && checkAllNeighbors(i,j))
                {
                    hasCarvedAndHunted = false;
                    //set current position as visited
                    currRow = i;
                    currCol = j;
                    grid[currRow, currCol].isVisited = true;
                    //destroy a wall in a random direction
                    destroyWall();

                    return;
                }
            }
        }
    }

    bool VisitedAllNeighbors()
    {
        // check top wall.
        if (CheckCellRules(currRow - 1, currCol))
        {
            return true;
        }

        // check bottom wall.
        if (CheckCellRules(currRow + 1, currCol))
        {
            return true;
        }

        // check left wall.
        if (CheckCellRules(currRow, currCol + 1))
        {
            return true;
        }

        // check right wall.
        if (CheckCellRules(currRow, currCol ))
        {
            return true;
        }

        return false;
    }


    bool CheckCellRules(int row, int column)
    {
        // check if cell is inside maze params and check if it is not visited.
        if (row >= 0 && row < rows && column >= 0 && column < columns
            && !grid[row, column].isVisited)
        {
            return true;
        }

        return false;
    }

    void destroyWall()
    {
        bool destroyed = false;
        //keep using switch until a wall is destroyed
        while(!destroyed)
        {
            int ranNum = UnityEngine.Random.Range(0, 4);
            Debug.Log(ranNum);

            switch (ranNum)
            {
                //top
                case 0:
                    if (currRow > 0 && grid[currRow - 1, currCol].isVisited)
                    {

                        if (grid[currRow, currCol].topWall)
                        {
                            Destroy(grid[currRow, currCol].topWall);
                        }

                        if (grid[currRow - 1, currCol].bottomWall)
                        {
                            Destroy(grid[currRow - 1, currCol].bottomWall);
                        }
                        destroyed = true;
                    }
                        break;
                     
                //Bottom
                case 1:

                    //check if its not on the bottom row and check if the cell below isn't already visited
                    if (currRow < rows - 1 && grid[currRow + 1, currCol].isVisited)
                    {
                        
                        if (grid[currRow, currCol].bottomWall)
                        {
                            Destroy(grid[currRow, currCol].bottomWall);
                        }

                        if (grid[currRow + 1, currCol].topWall)
                        {
                            Destroy(grid[currRow + 1, currCol].topWall);
                        }

                        destroyed = true;
                    }
                    break;
                //Left
                case 2:
                    
                    //check if its not the most left column and check if the cell on the left isn't already visited
                    if (currCol > 0 && grid[currRow, currCol - 1].isVisited)
                    {
                        Debug.Log("Destroyed right " + currRow + " " + (currCol - 1));

                        if (grid[currRow, currCol].leftWall)
                        {
                            Destroy(grid[currRow, currCol].leftWall);
                        }

                        if (grid[currRow, currCol - 1].rightWall)
                        {
                            Destroy(grid[currRow, currCol - 1].rightWall);
                        }
                        
                            destroyed = true;

                    }
                    break;
                //Right
                case 3:
                    
                    //check if its not the most left column and check if the cell on the right isn't already visited
                    if (currCol < columns - 1 && grid[currRow, currCol + 1].isVisited)
                    {
                        Debug.Log("Destroyed left " + currRow + " " + (currCol + 1));

                        if (grid[currRow, currCol].rightWall)
                        {
                            Destroy(grid[currRow, currCol].rightWall);
                        }

                        if (grid[currRow, currCol + 1].leftWall)
                        {
                            Destroy(grid[currRow, currCol + 1].leftWall);
                        }
                        destroyed = true;

                    }
                    break;

            }
        }
        
    }
    bool checkAllNeighbors(int row, int colum)
    {
        //check cell above
        if(row > 0 && grid[row -1, colum].isVisited)
        {
            return true;
        }
        //check cell below
        if (row < rows -1 && grid[row +1, colum].isVisited)
        {
            return true;
        }
        //check cell Left
        if (colum > 0 && grid[row, colum -1].isVisited)
        {
            return true;
        }
        //check cell right
        if (colum < columns - 1 && grid[row, colum + 1].isVisited)
        {
            return true;
        }
        return false;
    }
    
}
