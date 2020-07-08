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
        if (CheckCellRules(currRow, currCol - 1))
        {
            return true;
        }

        return false;
    }


    

    // check if cell is inside maze params and check if it is visited.
    bool CheckCellRules(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns
            && !grid[row, column].isVisited)
        {
            return true;
        }

        return false;
    }

  

    //Hunt and Kill Algorithm
    void StartAlgorithme()
    {
        //set start position and set as visited
        grid[currRow, currCol].isVisited = true;

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
                    if (currRow > 0 && !grid[currRow - 1, currCol].isVisited)
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
                    if (currRow < rows - 1 && !grid[currRow + 1, currCol].isVisited)
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
                    if (currCol > 0 && !grid[currRow, currCol - 1].isVisited)
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
                    if (currCol < columns - 1 && !grid[currRow, currCol + 1].isVisited)
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


    
    void Update()
    {
        
    }
}
