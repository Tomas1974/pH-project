#include <WiFi.h>

#include <PubSubClient.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include "passwords.h"  // Your WiFi and MQTT credentials
#include <LiquidCrystal_I2C.h>
#include "WifiMenu.h"
#include "WifiModel.h"
#include "PHDriver.h"
#include "WriteModel.h"

#include <vector> 



// Ports on ESP32
const int LED_GREEN = 17;

const int BUTTON_Menu = 16;
const int BUTTON_Choise = 4;


//Variables screen
int lcdColumns = 16;
int lcdRows = 2;

std::vector<WriteModel> models1;





//Starter temperatursensoren, hvis den er sand.
bool start=false;
long lastUpdateTime;



//Hardware  varibles

PHDriver pHDriver(36); // Starter pHdriveren med måling på pin 36

//Pubsub til brokeren
WiFiClient espClient;
PubSubClient client(espClient);

//Skærm
LiquidCrystal_I2C lcd(0x27, lcdColumns, lcdRows);  


//Objekter med passwords til WifiMenu
std::vector<WifiModel> wifiNetworks = {
  WifiModel(hjemme_name, hjemme_ssid, hjemme_password), //Strings fra passwords.h
  WifiModel(skole_name, skole_ssid, skole_password),
    
};

//WifiMenu wifiMenu(skærm variabel, netværks array, antal objekter i array, knap et bladre i menu, knap to vælge i menuen);
WifiMenu wifiMenu( lcd, wifiNetworks, BUTTON_Menu, BUTTON_Choise);











//Broker signal fra c# om at starte temperatursensoren
void callback(char* topic, byte* payload, unsigned int length) {
  
Serial.println("Modtaget");

if (start)
  start=false;
  else
  start=true;
  
}



void mqttConnection()
{
   client.setServer(mqttServer, mqttPort);
  while (!client.connected()) //Metoden springes over, hvis der er forbindelse i forvejen.
  {
    Serial.println("Connecting to MQTT...");
    if (client.connect("ESP32Client", mqttUser, mqttPassword)) {
      Serial.println("connected");
      client.setCallback(callback);  // Set the callback function
      client.subscribe("esp/return"); // Subscribe to the topic
    } else {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
      
    }
}
client.publish("esp/test", "Hello from ESP32");
}


void setup() {

if (!SPIFFS.begin(true)) { //Den her linje stabilisere evnen til at læse gemte data. Jeg ved ikke hvorfor.
        Serial.println("SPIFFS Mount Failed");
        return;
    }



  Serial.begin(9600);

  wifiMenu.initialize();
    lcd.init();                    
    lcd.backlight();
  
  pinMode(LED_GREEN, OUTPUT);

//pHDriver.makeCalibration(1944, 1445);

  
}


void loop() {
  
  client.loop();
  
   //long currentTime = millis();
  

   
  if (start)
  {

    //long timeWent=currentTime-lastUpdateTime;

  
  //if (timeWent>=1000)
  {
      
    float pH = pHDriver.measurePH(); // Measure the pH
  //int U=pHDriver.measuremV();
  
  Serial.println(pH); // Print the pH value to the Serial Monitor



     lcd.clear();
     lcd.print("Vis pH. "+ String(pH));
    
    digitalWrite(LED_GREEN, HIGH);  // Update the LED
    
    char message[20];
     sprintf(message, "%.1f", pH);

    client.publish("esp/pH", message);
    
    //lastUpdateTime=currentTime;
    }
       
      

    }
    else
 {

  digitalWrite(LED_GREEN, LOW);  // Slukker led, når der ikke er forbindelse
  
    wifiMenu.wifiMenuSystem();

  
    
    if (wifiMenu.getwifiON() !="") //Den skal ikke forbinde, hvis der ikke er internet.
       mqttConnection(); 
    

 }
 delay(50);
  
}