#include "PHDriver.h"
#include <algorithm> // Include to use std::sort


PHDriver::PHDriver(int PHAnalogPin) 
{
_PHAnalogPin = PHAnalogPin;

}


void PHDriver::initialize()
{
  if (!SPIFFS.begin(true)) { //Den her linje stabilisere evnen til at læse gemte data. Jeg ved ikke hvorfor.
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
  
    values[i] = analogRead(_PHAnalogPin)* (3300.0 / 4095.0);; // Læser værdien fra sensoren og omregner til mV
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
        if (!SPIFFS.begin(true)) {
            Serial.println("An Error has occurred while mounting SPIFFS");
            return;
        } else {

            File file = SPIFFS.open(fileName, FILE_WRITE);
           
            StaticJsonDocument<256> doc;

            for ( WriteModel& model : writeModel) {
                doc[model.getName()] = model.getValue();
            }

            serializeJson(doc, file);  
            file.close();
        }
    }




std::vector<WriteModel> PHDriver::readJsonFromFile(  String fileName) { //Her hentes kalibreringsværdierne
    
    std::vector<WriteModel> models;
    File file = SPIFFS.open(fileName, FILE_READ); 
    if (!file) {
        Serial.println("Failed to open file for reading");
        return models;
    } else {
        StaticJsonDocument<256> docRead;
        deserializeJson(docRead, file);

      for (JsonObject::iterator it = docRead.as<JsonObject>().begin(); it != docRead.as<JsonObject>().end(); ++it) {
     
        String name = it->key().c_str(); // Extract the model name.
        int value = it->value().as<int>(); // Extract the model value, assuming it's an integer.
       models.push_back(WriteModel(name, value)); // Create a new WriteModel object and add it to the vector.
    }


        file.close(); // Close the file after reading
    }

    return models;
 }

