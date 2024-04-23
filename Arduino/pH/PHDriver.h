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
    
        
    PHDriver(int PHAnalogPin);
    float measurePh();
    void makeCalibration(int pH4, int pH7); 

    void writeJsonToFile(std::vector<WriteModel> writeModel, String fileName);
    std::vector<WriteModel> readJsonFromFile( String fileName);
    int measureU();
    void initialize();

private:

    
    void sortArray(int array[], int size);
    int _PHAnalogPin;
    float makeUToPh(int U);
    std::vector<WriteModel> models;

};

#endif 
