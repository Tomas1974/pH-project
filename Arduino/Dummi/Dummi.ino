#include <WiFi.h>
#include <PubSubClient.h>
#include <random>
#include <iostream>
#include "passwords_Dummit.h"  // Your WiFi and MQTT credentials

WiFiClient espClient;
PubSubClient client(espClient);

int satisfaction = 0;

String names[]={"AAAA-AAAA","BBBB-BBBB","CCCC-CCCC","DDDD-DDDD","EEEE-EEEE","FFFF-FFFF","GGGG-GGGG","HHHH-HHHH","IIII-IIII","JJJJ-JJJJ","KKKK-KKKK","LLLL-LLLL","MMMM-MMMM","NNNN-NNNN","OOOO-OOOO","PPPP-PPPP","QQQQ-QQQQ","RRRR-RRRR","SSSS-SSSS"
,"TTTT-TTTT","UUUU-UUUU","VVVV-VVVV","XXXX-XXXX","YYYY-YYYY","ZZZZ-ZZZZ"};

int randNumber;

float randNumber1;


void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
    // Attempt to connect
    if (client.connect("ESP32Client", mqttUser, mqttPassword)) {
      Serial.println("Connected to MQTT broker");
    } else {
      Serial.print("Failed with state ");
      Serial.println(client.state());
      delay(2000);
    }
  }
}


void setup() {


Serial.begin(9600);
WiFi.begin(ssid, password);
 
while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Connecting to WiFi..");
}
 
  // Connect to MQTT Broker
  client.setServer(mqttServer, mqttPort);
  reconnect();
randomSeed(analogRead(0));

}





void loop() {

   if (!client.connected()) {
    reconnect();
  }
  client.loop();
 

    char message[20];
  char address[50];

for (int i=0; i<20;i++)
{

randNumber = random(650+i*3, 680+i*3); 
 randNumber1=float(randNumber)/100.0;
 sprintf(message, "%.2f", randNumber1);
sprintf(address, "client/%s", names[i]);

 client.publish(address, message);
    delay(3*60000); 
    //delay(1000); 

}

 




}
