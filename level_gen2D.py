import random

state = [[[1,2], [3,4]], [[5,6], [7,8]]]
edge = [] # at least 7 edges

#ACTIONS
def rot(col):
    return [col[2], col[0], col[3], col[1]]
def down_0(s):
    col = [s[0][0][0],s[0][1][0],s[1][0][0],s[1][1][0]]
    col = rot(col)
    s[0][0][0] = col[0]
    s[0][1][0] = col[1]
    s[1][0][0] = col[2]
    s[1][1][0] = col[3]
def down_1(s):
    col = [s[0][0][1],s[0][1][1],s[1][0][1],s[1][1][1]]
    col = rot(col)
    s[0][0][1] = col[0]
    s[0][1][1] = col[1]
    s[1][0][1] = col[2]
    s[1][1][1] = col[3]
def left_0(s):
    col = [s[0][0][0],s[0][0][1],s[0][1][0],s[0][1][1]]
    col = rot(col)
    s[0][0][0] = col[0]
    s[0][0][1] = col[1]
    s[0][1][0] = col[2]
    s[0][1][1] = col[3]
def left_1(s):
    col = [s[1][0][0],s[1][0][1],s[1][1][0],s[1][1][1]]
    col = rot(col)
    s[1][0][0] = col[0]
    s[1][0][1] = col[1]
    s[1][1][0] = col[2]
    s[1][1][1] = col[3]

def getN(a):
    random.seed()
    b = random.randint(0,7)
    while(b == a):
        b = random.randint(0,7)
    return b

for i in range(7):
    a = getN(8)
    b = getN(a)
    duplicate = False
    for t in edge:
        if ((a,b) == t or (b,a) == t):
            i -= 1
            duplicate = True
            break
    if not duplicate:
        edge.append((a,b))

flatten = lambda l: [n for sublist in l for item in sublist for n in item]
state_f = flatten(state)
a = random.sample(state_f, k=len(state_f))
scale = lambda s, acc : [[[s[acc+i*4+j*2+k] for i in range(2)] for j in range(2)] for k in range(2)]
b = scale(a, 0)