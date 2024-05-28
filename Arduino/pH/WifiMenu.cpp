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
  
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED && counter<10) 
  {
    counter++;
    delay(500);
    
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
    
    buttonState_Menu = digitalRead(_BUTTON_Menu); //Her læses knappe status
        
  if (buttonState_Menu != lastButtonState_Menu) {   //Her ses om knappen er anderledes end sidste gang enten at man trykker knappen ned eller slipper den
    

    if (buttonState_Menu == HIGH) { //Når kan kun være tilfældet hvis vi slipper knappen.
            

        programNumber++;  //Eftersom det er programmer vil skal bladre i, så hæver vi værdien til næste værdi.

      if (programNumber==_wifiNetworks.size())  //Værdierne skal nulstilles. Det når vi når til sidste menupunkt. Det sker her.
      programNumber=0;

          valg();  //Her vælges tekst til skærmen.

       
    }
    
  }

  lastButtonState_Menu = buttonState_Menu; //Så vi kan sammenligne, når vi trykker på knapperne, så gemmes sidste state her.
  

}


void WifiMenu::valg()
{ 
  Serial.println("TEst "+ String(programNumber));
  
       if (programNumber<_wifiNetworks.size()-2) //De to sidste er til kalibrering. 
       {

       _lcd.clear();
      _lcd.print(_wifiNetworks[programNumber].getNetworkName()); //Her skriver på første linje netværksnavnet.

      _lcd.setCursor(0,1); //Her skriver på anden linje netværksstatus.
        
      if (getwifiON() == _wifiNetworks[programNumber].getSSID() ) //Hvis netværksnavnet gemt under wifi tilslutningen er den samme om netværksnavnet for knaptrykket. Så er den on.
       _lcd.print("-------ON-------");

              
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

      _lcd.setCursor(0,1);
       _lcd.print("-----Measure----");
    
    PHDriver pHDriver(36);
     
if (programNumber==_wifiNetworks.size()-2) //Det er de den som bruges til  kalibrering af ph4
           {
             ph4 = pHDriver.measureU();
             delay(2000);
         
         }
         
          else
          
          {
            ph7 = pHDriver.measureU();
            delay (2000);

          
           pHDriver.makeCalibration(ph4, ph7);
           _lcd.clear();
            _lcd.print("Calibration fin");

          }
      
      

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
            startCalibration();
          

      



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


