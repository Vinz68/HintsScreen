## HintsScreen 
is a super simple Windows 10 IoT application (tested on Raspberry PI 3) to display a text on a screen using REST.

Using an url from any browser, you can show the text on the screen what you want. 


The URI syntax is:
  http://[ip or machine-name]:8888/api/hint/[TimeInSec]/[HintText]


TimeInSec: For future use mainly. For now  -1 = Black Screen (no text) and 1 or higher = show the text 

HintText : is the textual string which must be displayed. Use "%0c" to force a newline (see example)  

### Example:
   http://win10-pi3:8888/api/hint/1/Hint: Keep your eyes open when trying to solve the puzzle%0cGood Luck !

Response of the example (when successfull):
{"TimeInSec":1,"HintText":"Hint: Keep your eyes open when trying to solve the puzzle\fGood Luck !"}

The application is created to show a hint (textual help) for an escape room. 
The project also contains a test program (Windows 10 UWP applicatie).
Program is developed with Visual Studio 2015 with C#.

NOTE: The REST communication is based on: tomkuijsten\restup : Webserver for universal windows platform (UWP) apps 
which can be found here: https://github.com/tomkuijsten/restup
