#include <WiFi.h>
#include <PubSubClient.h>
#include <random>
#include <iostream>
#include "passwords_Dummit.h"  // Your WiFi and MQTT credentials

WiFiClient espClient;
PubSubClient client(espClient);

int satisfaction = 0;

void setup() {


Serial.begin(9600);
WiFi.begin(ssid, password);
 
while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Connecting to WiFi..");
}
 
Serial.println("Connected to the WiFi network");


client.setServer(mqttServer, mqttPort);
 
  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
 
    if (client.connect("ESP32Client", mqttUser, mqttPassword )) {
 
      Serial.println("connected");
 
    } else {
 
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);

    }

  }
  client.publish("esp/test", "Hello from ESP32");

  client.publish("Contact/unit", "client1");
    delay(100); // Debounce delay

client.publish("Contact/unit", "client2");
    delay(100); // Debounce delay

    client.publish("Contact/unit", "client3");
    delay(100); // Debounce delay
  
client.publish("Contact/unit", "client4");
    delay(100); // Debounce delay

client.publish("Contact/unit", "client5");
    delay(100); // Debounce delay


randomSeed(analogRead(0));

}

void loop() {

  client.loop();
 
int randNumber = random(7000, 7400); 

float randNumber1=float(randNumber)/1000.0;

    char message[20];


     sprintf(message, "%.2f", randNumber1);
    client.publish("client/AAAA-AAAA", message);
    delay(5000); // Debounce delay


    


 randNumber = random(680, 740); //Jeg havde lavet en fin array til alle disse broker sendinger, men brokeren stoppede med at modtage. Så det blev kopi-paste programmering.
 randNumber1=float(randNumber)/100.0;
 sprintf(message, "%.2f", randNumber1);
 client.publish("client/BBBB-BBBB", message);
    delay(5000); // Debounce delay



 randNumber = random(600, 620); 
 randNumber1=float(randNumber)/100.0;
 sprintf(message, "%.2f", randNumber1);
 client.publish("client/CCCC-CCCC", message);
    delay(5000); // Debounce delay


 randNumber = random(580, 620); 
 randNumber1=float(randNumber)/100.0;
 sprintf(message, "%.2f", randNumber1);
 client.publish("client/DDDD-DDDD", message);
    delay(5000); // Debounce delay



 randNumber = random(740, 780); 
 randNumber1=float(randNumber)/100.0;
 sprintf(message, "%.2f", randNumber1);
 client.publish("client/EEEE-EEEE", message);
    delay(5000); // Debounce delay






}
