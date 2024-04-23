#ifndef WriteModel_h
#define WriteModel_h
#include <Arduino.h>

class WriteModel {
public:
    WriteModel(String name, int value);
    String getName() ; // Correctly marked as const
    int getValue() ; // Correctly marked as const
    void setName(String name); // Removed const
    void setValue(int value); // Removed const
    

private:
    String name1; 
    int value1;
};

#endif // WriteModel_h
