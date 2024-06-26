#include <WiFi.h>

#include <PubSubClient.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include "passwords_pH.h"  // Your WiFi and MQTT credentials
#include <LiquidCrystal_I2C.h>
#include "WifiMenu.h"
#include "WifiModel.h"
#include "PHDriver.h"
#include "WriteModel.h"
#include <vector> 



// Ports on ESP32


const int BUTTON_Menu = 16;
const int BUTTON_Choise = 4;


//Variables screen
int lcdColumns = 16;
int lcdRows = 2;



//Starter temperatursensoren, hvis den er sand.
bool start=false;



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
  WifiModel(magnus_name, magnus_ssid, magnus_password),
  WifiModel(calibrate_name, calibrate_ssid, calibrate_password),
  WifiModel(calibratetwo_name, calibratetwo_ssid, calibratetwo_password)
};

//WifiMenu wifiMenu(skærm variabel, netværks array, antal objekter i array, knap et bladre i menu, knap to vælge i menuen);
WifiMenu wifiMenu( lcd, wifiNetworks, BUTTON_Menu, BUTTON_Choise);




void mqttConnection()
{
   client.setServer(mqttServer, mqttPort);
  while (!client.connected()) //Metoden springes over, hvis der er forbindelse i forvejen.
  {
    Serial.println("Connecting to MQTT...");
    if (client.connect("ESP32Client", mqttUser, mqttPassword)) {
      Serial.println("connected");
     
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


  Serial.begin(9600);

  wifiMenu.initialize();
  pHDriver.initialize();
    lcd.init();                    
    lcd.backlight();
  
  

  
}


void loop() {
  
  client.loop();
  
  
     
  if (start)
  {
      
   float pH = pHDriver.measurePh(); // Measure the pH
  

  Serial.println(pH); // Print the pH value to the Serial Monitor


    //Serial.println(pHDriver.measureU()); 


     lcd.clear();
     lcd.print("pH = "+ String(pH));

     
    
        
    char message[20];
     sprintf(message, "%.2f", pH);

    client.publish("client/ABCD-EFGH", message);
    
    //  delay(60*60000);
      delay(1000);

    }
    else
 {

  

    wifiMenu.wifiMenuSystem();


       if (wifiMenu.getwifiON() !="") //Den skal ikke forbinde, hvis der ikke er internet.
     { mqttConnection(); 
     start=true;
     
     } 

}

 delay(50);
}