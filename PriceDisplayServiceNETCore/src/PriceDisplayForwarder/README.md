# 448 price display forwarder

## description
This is the program responsible for writing to the serial port. It accepts input from a pipe. .NET Core does not suppport SerialPort library, so in order for the web app to write to serial port, we did this.