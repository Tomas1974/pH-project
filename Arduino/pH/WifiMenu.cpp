#include "WifiMenu.h"
#include "Arduino.h"
#include "PHDriver.h"


WifiMenu::WifiMenu(LiquidCrystal_I2C lcd, std::vector<WifiModel> wifiNetworks , int BUTTON_Menu, int BUTTON_Choise)
: _lcd(lcd),
 _BUTTON_Menu(BUTTON_Menu),
 _BUTTON_Choise(BUTTON_Choise),
 buttonState_Menu(0),
 buttonState_Choise(0),
 lastButtonState_Menu(0),
 lastButtonState_Choise(0),
 wifiON(""), 
 programNumber(-1)

{
    _wifiNetworks = wifiNetworks; 
}





void WifiMenu::initialize() {
  
  _lcd.init();                    
  _lcd.backlight();
  pinMode(_BUTTON_Menu, INPUT_PULLUP);
  pinMode(_BUTTON_Choise, INPUT_PULLUP);
  
       
}




String WifiMenu::wifiConnection(String ssid, String password)
{
  int counter=0;
  //programNumber == 0;
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED && counter<10) 
  {
    counter++;
    delay(500);
    
  }

if (programNumber == 3){
  Serial.println("kalibrere");
  return "calibration";
}
  
if (counter==10)
{
   Serial.println("Ingen forbindelse");
   ssid="";
       
}


else
  Serial.println("Connected to the WiFi network");
 

return ssid;

}



void WifiMenu::button_Menu()
{
    
    buttonState_Menu = digitalRead(_BUTTON_Menu);
        
  if (buttonState_Menu != lastButtonState_Menu) {
    

    if (buttonState_Menu == HIGH) {
            

        programNumber++;

      if (programNumber==_wifiNetworks.size())
      programNumber=0;

          valg();  

       
    }
    
  }

  lastButtonState_Menu = buttonState_Menu;
  

}


void WifiMenu::valg()
{ 
  Serial.println("TEst "+ String(programNumber));
  
       if (programNumber<_wifiNetworks.size()-2)
       {

       _lcd.clear();
      _lcd.print(_wifiNetworks[programNumber].getNetworkName());

      _lcd.setCursor(0,1);
        
      if (getwifiON() == _wifiNetworks[programNumber].getSSID() )
       _lcd.print("-------ON-------");

       else if (_wifiNetworks[programNumber].getNetworkName() == "Calibrate")
        _lcd.print("");
       
      else
       _lcd.print("------OFF-------");
}
else
    {
         
        
         _lcd.clear();
         if (programNumber==_wifiNetworks.size()-2)
          
          _lcd.print("Measure PH4");
          else
          _lcd.print("Measure PH7");

    }


       


}

void WifiMenu::startCalibration(){

      _lcd.clear();

     _lcd.print("Measuring PH4...");

     PHDriver pHDriver(36);

     ph4 = pHDriver.measureU();

     delay (2000);

     _lcd.clear();

     _lcd.print(ph4);

     delay(2000);
 
}

void WifiMenu::startCalibrationtwo(){

      
    _lcd.clear();

     _lcd.print("measuring PH7...");
     PHDriver pHDriver(36);

    ph7 = pHDriver.measureU();

     delay (2000);

     _lcd.clear();

     _lcd.print(ph7);

     delay(2000);

     _lcd.clear();

     _lcd.print("Calibrating");

   
     delay(2000);

     _lcd.clear();
      pHDriver.makeCalibration(ph4, ph7);

     _lcd.print("Calibration fin");

     delay(2000);
     

}


    



void WifiMenu::button_Choise()
{

 buttonState_Choise = digitalRead(_BUTTON_Choise);

    if (buttonState_Choise != lastButtonState_Choise) 
    if (buttonState_Choise == HIGH) 
    {

      
   
    
        if (programNumber<_wifiNetworks.size()-2)
             //Her placeres en String i wificonnection. Det sker ved at kalde wificonnection. (Den etablere wifi og sender navnet på opkoblingen retur). Hvis der ikke var netværk er opkoblingen "" tom. 
       setWifiOn(wifiConnection(_wifiNetworks[programNumber].getSSID(), _wifiNetworks[programNumber].getPassword()));
      else
      {

          if (programNumber==_wifiNetworks.size()-2)
          
          startCalibration();
          else
          startCalibrationtwo();


      }



       valg();
             
    }
     
    lastButtonState_Choise = buttonState_Choise;

}


    void WifiMenu::wifiMenuSystem() //Kalde metoden fra main
    {
      button_Menu();
      button_Choise();
    }




String WifiMenu::getwifiON()
{
  return wifiON;
}

void WifiMenu::setWifiOn(String wifiON1)
{
  wifiON=wifiON1;
}


