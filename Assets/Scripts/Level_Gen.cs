using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
class Level_Gen{
    static int[] G1 = {0,2,6,8,18,20,24,26};
    static int[] G2 = {1,3,5,7,9,11,15,17,19,21,23,25};
    static int[] G3 = {4,10,12,14,16,22};
    static int[] G4 = {13};
    private static int[] swap(int[] arr){
        return new int[]{arr[6], arr[3],arr[0],arr[7],arr[4],arr[1],arr[8],arr[5],arr[2]};
    }

    private static void down(int[][][] state, int col){
        int[] partition = new int[9];
        int k = 0;
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                partition[k++] = state[i][j][col];
            }
        }
        k = 0;
        partition = swap(partition);
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                state[i][j][col] = partition[k++];
            }
        }
    }
    private static void left(int[][][] state, int row){
        int[] partition = new int[9];
        int k = 0;
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                partition[k++] = state[row][i][j];
            }
        }
        k = 0;
        partition = swap(partition);
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                state[row][i][j] = partition[k++];
            }
        }
    }

    private static ArrayList availableNeighbors(int[][][] state, int[] pos, bool[] visited){
        ArrayList r = new ArrayList();
        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++){
                for (int k = -1; k < 2; k++){
                    int a = i+pos[0];
                    int b = j+pos[1];
                    int c = k+pos[2];
                    if (a > -1 && a < 3 &&
                        b > -1 && b < 3 &&
                        c > -1 && c < 3){
                            if (!visited[state[a][b][c]])
                                r.Add(state[a][b][c]);
                             
                        }
                }
            }
        }
        return r;
    }

    private static int which(int id){
        if (G1.Contains(id))
            return 1;
        else if (G2.Contains(id))
            return 2;
        else if (G3.Contains(id))
            return 3;
        else if (id == 13)
            return 4;
        else 
            return -1;
    }

    private static ArrayList getRest(int[][][] state, bool[] visited){
        ArrayList r = new ArrayList();
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                for (int k = 0; k < 3; k++){
                    if (!visited[state[i][j][k]])
                        r.Add(state[i][j][k]);
                }
            }
        }
        return r;
    }

    private static int[] updatePos(int[][][] state, int id){
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                for (int k = 0; k < 3; k++){
                    if (state[i][j][k] == id)
                        return new int[]{i,j,k};
                }
            }
        }
        return null;
    }

    private static void shuffle(int[][][] state, int max){
        Random.InitState((int) (Time.deltaTime*10000));
        int ops = Mathf.RoundToInt(Random.Range(1, max));
        for (int i = 0; i < ops; i++){
            if (Random.value < 0.5f){
                int col = Mathf.RoundToInt(Random.Range(0,2));
                int n = Mathf.RoundToInt(Random.Range(0,3));
                for (int j = 0; j < n; j++)
                    down(state, col);
            }
            else{
                int row = Mathf.RoundToInt(Random.Range(0,2));
                int n = Mathf.RoundToInt(Random.Range(0,3));
                for (int j = 0; j < n; j++)
                    left(state, row);
            }
        }
    }

    public static ArrayList build(){
        int[][][] state = new int[3][][];
        for (int i = 0; i < 3; i++){
            state[i] = new int[3][];
            for (int j = 0; j < 3; j++){
                state[i][j] = new int[3];
                for (int k = 0; k < 3; k++)
                    state[i][j][k] = i*9+j*3+k;
            }
        }

        ArrayList edges = new ArrayList();

        Random.InitState((int) (Time.deltaTime*10000));
        int[] current = {Mathf.RoundToInt(Random.Range(0,2)),
                Mathf.RoundToInt(Random.Range(0,2)),
                Mathf.RoundToInt(Random.Range(0,2))};
        int currentID = current[0]*9+current[1]*3+current[2];
        bool[] visited = new bool[27];
        int[] degree = new int[27];

        for (int i = 0; i < 26; i++){
            Random.InitState((int) (Time.deltaTime*10000));
            visited[currentID] = true;
            ArrayList n = availableNeighbors(state, current, visited);
            if (n.Count == 0)
                n = getRest(state, visited);
            
            int n_id = (int) n[Mathf.RoundToInt(Random.Range(0, n.Count-1))];
            int[] n_pos = updatePos(state, n_id);
            int counter = 0;
            while(checkDegree(degree, n_pos) && counter < 10){
                shuffle(state, 3);
                current = updatePos(state, currentID);
                n_pos = updatePos(state, n_id);
                counter++;
            }

            incDegree(degree, current, n_pos);
            edges.Add(new int[][]{current, n_pos});

            currentID = n_id;
            shuffle(state, 50);
            current = updatePos(state,currentID);
        }
        return edges;
    } 

    static bool checkDegree(int[] degree, int[] pos){
        if (pos == null)
            return true;
        if (degree[pos[0]*9+pos[1]*3+pos[0]] < 4)
            return false;
        return true;
    }
    static void incDegree(int[] degree, int[]cPos, int[]nPos){
        degree[cPos[0]*9+cPos[1]*3+cPos[0]]++;
        degree[nPos[0]*9+nPos[1]*3+nPos[0]]++;
    }
    public static void printEdges(){
        ArrayList ed = build();
        int[][] e;
        for (int i = 0; i < ed.Count; i++){
            e = (int[][]) ed[i];
            Debug.Log("{"+e[0][0]+", "+e[0][1] + ", " + e[0][2] + "} " +
            "-> " + "{" + e[1][0]+ ", "+e[1][1] + ", " + e[1][2] + "} ");
        }
    }
    
}