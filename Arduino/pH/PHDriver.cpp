#include "PHDriver.h"
#include <algorithm> // Include to use std::sort


PHDriver::PHDriver(int PHAnalogPin) 
{
_PHAnalogPin = PHAnalogPin;

}


void PHDriver::initialize()
{
  if (!SPIFFS.begin(true)) { //Den her linje instansiere filsystement, så vi kan læse og skrive data.
        Serial.println("SPIFFS Mount Failed");
        return;
    }
}


void PHDriver::makeCalibration(int pH4, int pH7) {
   
  std::vector<WriteModel> writeModel = {
    WriteModel("pH4", pH4),
    WriteModel("pH7", pH7)
};

writeJsonToFile(writeModel, "/pH"); //Her skrives til flash kalibreringsværdierne

}



float PHDriver::measurePh()
{
int U =measureU(); //U da man bruger det bogstav til spænding.
return makeUToPh(U);
}




float PHDriver::makeUToPh(int U)
{
  

  models = readJsonFromFile("/pH"); //Gemmer de gemte værdier i en array

     float a=(float(4)-float(7))/(float(models[0].getValue())-float(models[1].getValue())); //Udregner hældningstallet på en linear funktion
    
    float b=float(4)-a*float(models[0].getValue()); //Udregner skæring med y-aksen også kaldt b værdien
    
  if (a*U+b>14)
  return 14;
  else if(a*U+b<0)
  return 0;
  else
  return  a*U+b;

}



int PHDriver::measureU()  
{
    const int numMeasurements = 50;  // Total number of measurements
    int values[numMeasurements];     // Array to store the measurements


  
  for (int i = 0; i < numMeasurements; i++) {
  
    values[i] = analogRead(_PHAnalogPin)* (3300.0 / 4095.0);; // Læser værdien fra sensoren og omregner til mV. Der gemmes i 12 bit altså 12^2 muligheder. Det giver 4096. Fra 0 til 4095.
    delay(10);                         
 
  } 
 
   std::sort(values, values + numMeasurements); //Her sorteres værdier via Timsort
      

  int sum = 0;  
    
  for (int i = numMeasurements*0.1; i < numMeasurements*0.9; i++) { //Vi sortere 10% fra af de højeste og laveste værdier. Så burde vi have ekskluderet alle outliers.
    sum += values[i];
  }
  
  return sum / (numMeasurements*0.8); //Her tages gennemsnittet af de 80% af værdierne vi bruger
}





  void PHDriver::writeJsonToFile(std::vector<WriteModel> writeModel, String fileName) {//Her gemmes kalibreringsværdierne
        

              File file = SPIFFS.open(fileName, FILE_WRITE); //Her åbnes eller skabes en fil og data 
            
              StaticJsonDocument<256> doc; //Her laves et json dokument kaldt doc

              for ( WriteModel model : writeModel) { //Foreach løkke gennemløber alle værdier i model vektoren
                  doc[model.getName()] = model.getValue(); //Her tilføjes værdier til doc
              }

              serializeJson(doc, file);  //Her konventeres json object til en json string
              file.close();
          
      }




std::vector<WriteModel> PHDriver::readJsonFromFile(  String fileName) { //Her hentes kalibreringsværdierne
    
    std::vector<WriteModel> model; //Tom vektor
    File file = SPIFFS.open(fileName, FILE_READ);  //Her åbnes en fil til læsning 
    if (!file) { //Hvis filen ikke findes returneres en tom vektor og der skrives en fejlmeldning i konsolen
        Serial.println("Failed to open file for reading");
        return model;
    } else {
        StaticJsonDocument<256> doc; //Her laves et json dokument kaldt doc
        deserializeJson(doc, file); //Her skabes et json object.

      for (JsonObject::iterator it = doc.as<JsonObject>().begin(); it != doc.as<JsonObject>().end(); ++it) { //Et for loop meget specielt. Den tager objekter et for et og gennemløber indtil end.
     
        String name = it->key().c_str(); // Her hentes nøglen altså pH4 eller pH7. -> bruges i c++ til at pege på medlemmer i et object her et json object.
        int value = it->value().as<int>(); // Her hentes pH4 eller pH7 spænding i mV
       model.push_back(WriteModel(name, value)); //Her skrives til model 
    }


        file.close(); // Her lukkes filen
    }

    return model;
 }

