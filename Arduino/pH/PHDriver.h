#ifndef PHDriver_h
#define PHDriver_h

#include "Arduino.h"
#include <ArduinoJson.h>
#include "FS.h"
#include "SPIFFS.h"
#include "WriteModel.h"
#include <vector>

class PHDriver {
public:
    
    
    float makemVTopH(int U);
    PHDriver(int PHAnalogPin);
    float measurePH();
   void makeCalibration(int pH4, int pH7); 

    void writeJsonToFile(std::vector<WriteModel> writeModel, String fileName);
    std::vector<WriteModel> readJsonFromFile( String fileName);
    int measuremV();
    void initialize();

private:

    
    void sortArray(int array[], int size);
    int _PHAnalogPin;
    std::vector<WriteModel> models;

};

#endif 
