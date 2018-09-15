# 448 price board display driver

## Overview
Basically, we have got a display board named 448 price display. We are just given the board, without any documentation. Searching online doesn't give us much information either. So we decided to reverse engineer it. The reverse engineering process took about one and a half hour, but it was fun. This project is the result of the hacking.
- The price display driver is the arduino sketch that drives the 448 price display.
- The price display service is the web app (.NET Core) that can talk to the driver and display a price!

## Parts
There are a sequence of display boards. Each display board has two chips. One of the chip is named `M5480B7`, it is a LED display driver. Another chip is named `CD4013BCN`, which is a pair of D flip flops. We can easily google the data sheet of the chips online as follow:

### LED display driver
The LED display driver [data sheet](http://pdf.datasheetcatalog.com/datasheet/SGSThomsonMicroelectronics/mXqrq.pdf) indicates it is basically a shift register. After shifting in 36 bits, it will ignore some input, and send out 23 bits of its input to the output pins.

### D Flip Flop
The D Flip Flop chip [data sheet](https://www.fairchildsemi.com/datasheets/CD/CD4013BC.pdf) indicates it is a pair of D flip flops with different clocks. One of the D Flip Flop is used as a buffer to the shift register input, the another is used as a chain of shift register that drive the enable signal of the sequence of LED display driver. 

## Connections
The board itself has six inputs. Two inputs pins are simply power and ground. The another two are two pairs of data and clock. The first data and clock drives the LED display driver, the second data and clock drive the selection of board.

| 448 Display   | Audrino       | 
| :-----------: |:-------------:| 
| 1             | 7 (CLOCK_1)   | 
| 2             | 8 (DATA_1)    | 
| 3             | GND           | 
| 4             | 5V            | 
| 5             | 12 (CLOCK_2)  | 
| 6             | 13 (DATA_2)   | 

* The 448 Display port numbers are numbered from left to right (when the board is placed with the ports pointing upwards)