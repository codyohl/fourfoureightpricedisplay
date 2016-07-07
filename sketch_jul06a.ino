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
  // Serial.begin(9600);
}

void loop()
{
  //               0,D,2,3,4,5,6,7,8,D,D,D,2,3,4,5,D,D,D,D,0,1,2,3,4,5,D,7,D,D,0,1,2,D,D,D
  //               0,X,1,2,3,4,5,6,7,X,X,X,8,9,A,B,X,X,X,X,C,D,E,F,0,1,X,2,X,X,3,4,5,6,7,X
  int cycle[36] = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
  int segments[] = { 10,17,20,26,32,38,44,51,56,64,0,1,2,3,5,6,-1,3,5,-1,2,3,4,0,6,-1,2,3,4,5,6,-1,1,4,3,5,6,-1,2,1,4,5,6,-1,2,1,0,6,5,4,-1,1,2,3,5,-1,1,2,3,4,0,6,5,-1,1,2,3,4,5,6,-1 };
  int pins[] = { 2,3,4,5,6,7,8,13,14,15,20,21,22,23,24,25,27,30,31,32,33 };

  int row = 2;
  int display = 236;

  for (int k = 6; k > 0; k--)
  {
    digitalWrite(CLOCK_2, LOW);
    digitalWrite(DATA_2, k == row ? HIGH : LOW);
    delay(1);
    digitalWrite(CLOCK_2, HIGH);
  }

  for (int i = 0; i < 36; i++)
  {
    cycle[i] = (i == 0) ? 1 : 0;
  }

  int remainder = display;

  for (int digitIndex = 2; digitIndex >= 0; digitIndex--)
  {
      int digit = remainder % 10;
      remainder -= digit;
      remainder /= 10;
    int index = segments[digit];
    while (segments[index] != -1)
    {
      int segmentNumber = segments[index];
      int pinIndex = digitIndex * 7 + segmentNumber;
      int pinNumber = pins[pinIndex];
      cycle[pinNumber] = 1;
      index++;
      // Serial.println(pinNumber);
    }
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