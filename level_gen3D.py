import random 
import numpy as np
from matplotlib import pyplot as plt

def swap(arr):
    return np.asarray([arr[6], arr[3], arr[0], arr[7], arr[4], arr[1], arr[8], arr[5], arr[2]]).reshape(3,3)
def down(state, col):
    state[0:, 0:, col] = swap(state[0:, 0:, col].ravel())
def left(state, row):
    state[row, 0:, 0:] = swap(state[row, 0:, 0:].ravel())

def availableNeighbors(state, pos, visited):
    (i,j,k) = pos
    r = []
    for a in range(-1,2):
        for b in range(-1,2):
            for c in range(-1,2):
                d = i+a
                e = j+b
                f = k+c
                if (d > -1 and d < 3 and
                    e > -1 and e < 3 and
                    f > -1 and f < 3):
                    if not (visited[state[d,e,f]]):
                        r.append(state[d,e,f])
    return r
def cal(pos):
    return pos[0]*9+pos[1]*3+pos[2]
def isGroup1(id):
    if (id in [0,2,6,8,18,20,24,26]):
        return True
    return False
def isGroup2(id):
    if (id in [1,3,5,7,9,11,15,17,19,21,23,25]):
        return True
    return False
def isGroup3(id):
    if (id in [4,12,14,10,16,22]):
        return True
    return False
def isGroup4(id):
    return id == 13
def which(id):
    if (isGroup1(id)):
        return 1      
    elif (isGroup2(id)):
        return 2
    elif (isGroup3(id)):
        return 3
    elif (isGroup4(id)):
        return 4

def geTOthers(state, visited):
    r = []
    for i in range(3):
        for j in range(3):
            for k in range(3):
                p = (i,j,k)
                if not visited[state[p]]:
                    r.append(state[p])
    return r
def updatePos(state, id):
    for j in range(3):
        for k in range(3):
            for l in range(3):
                if state[(j,k,l)] == id:
                    return (j,k,l)

def shuffle(state, max):
    random.seed()
    ops = random.randint(1, max)
    for i in range(ops):
        if (random.randint(0,1) == 0): # Down
            col = random.randint(0,2)
            for j in range(random.randint(1, 3)):
                down(state, col)
        else:
            row = random.randint(0,2)
            for j in range(random.randint(1,3)):
                left(state,row)
def build():
    state = np.asarray([[[i*9+j*3+k for k in range(3)] for j in range(3)] for i in range(3)], np.int)
    edge = []
    random.seed()
    current = (random.randint(0, 2), random.randint(0,2), random.randint(0,2))
    currentID = state[current]
    visited = [False for i in range(27)]

    for i in range(26):
        random.seed()
        visited[currentID] = True
        r = availableNeighbors(state, current, visited)
        if (len(r) == 0):
            r = geTOthers(state, visited)

        n = r[random.randint(0, len(r)-1)]
        edge.append((currentID, n))

        currentID = n
        shuffle(state, 50)
        current = updatePos(state, currentID)
    return edge

def eType(e):
    if (e[0]==1 and e[1] == 1):
        return 1
    elif(e[0] == 2 and e[1] == 2):
        return 5
    return e[0]*e[1]

def analyzeE(edge, eT):
    edges = []
    for i in range(0, len(edge)):
        (f, t) = edge[i]
        pos = (which(f), which(t))
        edges.append(pos)

    for e in edges:
        eT[eType(e)] += 1

eT = [0] * 13
n = 10000
for i in range(n):
    edge = build()
    analyzeE(edge, eT)
    # if (i%100==0):
    #     print(i)

result = np.asarray([eT[1], eT[2], eT[3],eT[4],eT[5],eT[6], eT[8], eT[9],eT[12]]) / n
plt.plot(np.linspace(0,9,9), result, 'o')
plt.show()
# print("1-1: " + str(eT[1] / n))
# print("1-2: " + str(eT[2] / n))
# print("1-3: " + str(eT[3] / n))
# print("1-4: " + str(eT[4] / n))
# print("2-2: " + str(eT[5] / n))
# print("2-3: " + str(eT[6] / n))
# print("2-4: " + str(eT[8] / n))
# print("3-3: " + str(eT[9] / n))
# print("3-4: " + str(eT[12] / n))



