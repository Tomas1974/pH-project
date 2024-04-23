#include "PHDriver.h"
#include <algorithm> // Include to use std::sort


PHDriver::PHDriver(int PHAnalogPin) 
{
_PHAnalogPin = PHAnalogPin;

}



float PHDriver::measurePH()
{
int U =measuremV();

return makemVTopH(U);

}

void PHDriver::makeCalibration(int pH4, int pH7) {
    
    
  std::vector<WriteModel> writeModel = {
    WriteModel("pH4", pH4),
    WriteModel("pH7", pH7)
};

writeJsonToFile(writeModel, "/pH");

}


void PHDriver::initialize()
{
  if (!SPIFFS.begin(true)) { //Den her linje stabilisere evnen til at læse gemte data. Jeg ved ikke hvorfor.
        Serial.println("SPIFFS Mount Failed");
        return;
    }
}




float PHDriver::makemVTopH(int U)
{
  

  models = readJsonFromFile("/pH"); //Gemmer de gemte værdier i en array

     float a=(float(4)-float(7))/(float(models[0].getValue())-float(models[1].getValue())); //Udregner hældningstallet på en linear funktion
    
    float b=float(4)-a*float(models[0].getValue()); //Udregner skæring med y-aksen også kaldt b værdien
    

return  a*U+b;

}




int PHDriver::measuremV()  
{
    const int numMeasurements = 50;  // Total number of measurements
    int values[numMeasurements];     // Array to store the measurements


  
  for (int i = 0; i < numMeasurements; i++) {
  
    values[i] = analogRead(_PHAnalogPin)* (3300.0 / 4095.0);; // Read the value from the sensor
    delay(10);                         // Short delay between measurements
 
  }
 
   std::sort(values, values + numMeasurements); //Timsort
      

  int sum = 0;  
  int validCount = numMeasurements - numMeasurements/5; // Fjerner 20% af værdierne
  for (int i = numMeasurements/10; i < numMeasurements - numMeasurements/10; i++) { //Vi sortere 10% fra af de højeste og laveste værdier. Så burde vi have ekskluderet alle outliers.
    sum += values[i];
  }
  
  return sum / validCount;
}



void PHDriver::writeJsonToFile(std::vector<WriteModel> writeModel, String fileName) {//Her gemmes de kalibreringsværdier
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




std::vector<WriteModel> PHDriver::readJsonFromFile(  String fileName) { //Her hentes de kalibreringsværdier
    
    std::vector<WriteModel> models;
    File file = SPIFFS.open(fileName, FILE_READ); // Consider making file name dynamic
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

