// Pin numbers (These numbers are on the Audrino board)
#define CLOCK_1 7
#define CLOCK_2 12
#define DATA_1 8
#define DATA_2 13

void setup() {
  pinMode(CLOCK_1, OUTPUT);
  pinMode(CLOCK_2, OUTPUT);
  pinMode(DATA_1, OUTPUT);
  pinMode(DATA_2, OUTPUT);
}

void loop()
{
  //               0,D,2,3,4,5,6,7,8,D,D,D,2,3,4,5,D,D,D,D,0,1,2,3,4,5,D,7,D,D,0,1,2,D,D,D
  //               0,X,1,2,3,4,5,6,7,X,X,X,8,9,A,B,X,X,X,X,C,D,E,F,0,1,X,2,X,X,3,4,5,6,7,X
  int cycle[36] = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};

  for (int k = 0; k < 6; k++)
  {
    digitalWrite(CLOCK_2, LOW);
    digitalWrite(DATA_2, k == 3 ? HIGH : LOW);
    delay(1);
    digitalWrite(CLOCK_2, HIGH);

    for (int j = 1; j < 38; j++)
    {
      cycle[0] = 1;
      switch(j)
      {
        case 1:
        case 9:
        case 10:
        case 11:
        case 16:
        case 17:
        case 18:
        case 19:
        case 26:
        case 28:
        case 29:
        case 34:
        case 35:
        case 36:
          continue;
      }
      
      for (int i = 1; i < 36; i++)
      {
        cycle[i] = (i == j) ? 1 : 0;
      }
      
      for (int i = 0; i < 36; i++)
      {
        digitalWrite(CLOCK_1, LOW);
        delay(1);
        digitalWrite(DATA_1, cycle[i] == 0 ? LOW : HIGH);
        delay(1);
        digitalWrite(CLOCK_1, HIGH);
      }
    }
  } 
}
