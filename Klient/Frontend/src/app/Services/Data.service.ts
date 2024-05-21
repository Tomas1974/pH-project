import {Injectable} from '@angular/core';


import {
  BaseDto, PhDataDto, postNrDto, responseClient,
  responseStringDto,
  sendAddressesDto,
  SendLoginInfoDto,
  ServerSendsIOTDataToClientsDto, userClientDto, userModelDto
} from "./BaseDto";
import {Address, AddressAPIJsonResponseModel} from "../Models/LookupModels";
import {LogInModel, UserModel} from "../Models/userModel";
import {ClientModel} from "../Models/clientModel";
import {PHModel, series} from "../Models/pHModel";
import {StatusModel} from "../Models/SatusModel";
import {Observable} from "rxjs";



@Injectable({
  providedIn: 'root'
})
export class DataService {




  loginUser: string | undefined = ""; //Her er brugeren der er logget ind gemt

  /***********Home service********************/
  addressSuggestions: Address[] = [];
  loginResponse: string | undefined = "";
  requestLoginUser: string | undefined = "";
  user: UserModel | undefined; //Her gemmes bruger information
  chooseComponent: number = 0;
  timeStamp: number | undefined;
  town: string | undefined = "";

  ws: WebSocket = new WebSocket("ws://localhost:8181")

  graphName: string = "";

  /***********Settings service********************/

  clients: ClientModel[] = [];
  clientsNames: string[] = [];
  duplicatedClient: boolean = false;

  /***********graf service********************/


  selectedClient: string="";

  series: series[] =[];
  pHData: PHModel[] =[];
  client: ClientModel | undefined;
  maxGrafValue: number=10;
  minGrafValue: number=2;
  maxValue: number=0;
  minValue: number=0;

  statusUpdates : StatusModel[] = [];




  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
        this[messageFromServer.eventType].call(this, messageFromServer);

    }
  }

  createStatusEntryOnServer(){

    var object = {
      eventType: "GetServerStatus",
      eventType2: "createEntry"

    }

    this.ws.send(JSON.stringify(object));

  }

  getStatusFromServer() {

    var object = {
      eventType: "GetServerStatus",
    }

    this.ws.send(JSON.stringify(object));


    this.ws.onmessage = (event) => {
      // Parse the response
      var data = JSON.parse(event.data);

      console.log(data);

      this.statusUpdates.push(data);

    }
  }

  getStatusArray(): any[] {
    return this.statusUpdates;
  }



  getClientList() {
    //this.clientsNames=[];

    this.clientsNames=[];

    for (let i = 0; i < this.clients.length; i++) {
      // @ts-ignore
      this.clientsNames.push(this.clients[i].client_name);

    }
  }



  timePromise(): Promise<void> {
    return new Promise<void>((resolve, reject) => {

      const oldTimeStamp = new Date().getTime();
      let counter = 0;

      const intervalId = setInterval(() => {
        counter++;

        if (this.timeStamp! >= oldTimeStamp || counter === 10) {
          clearInterval(intervalId);  // Clear the interval to stop it from running
          resolve();
        }
      }, 100);
    });
  }



  /*********************home-service*********************************/





  sendAddressLine(addressSearchTerm: string) {

    var object = {
      eventType: "getAddresses",
      addressSearchTerm: addressSearchTerm

    }

    this.ws.send(JSON.stringify(object));
  }


  sendAddresses(dto: sendAddressesDto): void {

    const addressSuggestions = dto.results!
    this.addressSuggestions = addressSuggestions.results;
    this.timeStamp = new Date().getTime();
  }




  getUserInfo() {
    var object = {
      eventType: "getUserInfo",

      email: this.loginUser

    }
    this.ws.send(JSON.stringify(object));
  }

  userModel(dto: userModelDto) {

    this.user =
      {
        address: dto.address,
        street_number: dto.street_number,
        cvr: dto.cvr,
        email: dto.email,
        name: dto.name,
        password: "",
        zip_code: dto.zip_code
      }

    this.chooseComponent = 3; //Her vælges new User. Den vælges her, da så ved man at user er opdateret.


  }

  saveOrEditUser(userModel: UserModel, type: string, oldEmail: string) {

    this.requestLoginUser = userModel.email;

    var object = {
      eventType: "saveUser",

      oldEmail: oldEmail,
      type: type,
      email: userModel.email,
      name: userModel.name,
      password: userModel.password,
      address: userModel.address,
      street_number: userModel.street_number,
      zip_code: userModel.zip_code,
      cvr: userModel.cvr
    }
    this.ws.send(JSON.stringify(object));
  }



  LoginUser(loginModel: LogInModel) {
    this.requestLoginUser = loginModel.email;

    var object = {
      eventType: "loginUser",
      email: loginModel.email,
      password: loginModel.password

    }
    this.ws.send(JSON.stringify(object));
  }


   responseString(dto: responseStringDto) { //Svar fra login

    this.loginResponse = dto.response;



    if (dto.response == "Success") {
      {
        this.loginUser = this.requestLoginUser;


      }

      this.chooseComponent = 2;//Da denne info bruges i andre pages sendes info til fælles dataservice
    }


    this.getClient(); //Her hentes client listen til den valgte bruger


  }


  async SendLoginInfo(dto: SendLoginInfoDto) {
    this.loginUser = dto.email;
    this.timeStamp = new Date().getTime();


  }


  getPostNr(postNr: number) {

    var object = {
      eventType: "PostNr",

      postNr: postNr

    }
    this.ws.send(JSON.stringify(object));

  }

  SendTown(dto: postNrDto) {
    this.town = dto.town;


    this.timeStamp = new Date().getTime();

  }


  UserActions(info: string) {


    var object = {
      eventType: "UserActions",
      getLoginInfo: info

    }
    this.ws.send(JSON.stringify(object));

  }

  /**************************************client-service*******************************/

  saveClient(clientModel: ClientModel, email: String) {
    var object = {
      eventType: "saveClient",
      client_id: clientModel.client_id,
      client_name: clientModel.client_name,
      max_value: clientModel.max_value,
      min_value: clientModel.min_value,
      email: email
    }
    this.ws.send(JSON.stringify(object));
  }

  getClient() {
    var object = {
      eventType: "getClient",
      email: this.loginUser
    }
    this.ws.send(JSON.stringify(object));

  }


  responseListOfClients(dto: userClientDto) {


    this.clients = [...dto.clients!];

    this.timeStamp = new Date().getTime();

    this.getClientList(); //Her laves listen over navnene til clienterne. Den bruges til dropdown menu i graf.

  }


  responseClient(dto: responseClient)
  {
    this.duplicatedClient = dto.duplicate!

    this.timeStamp = new Date().getTime();

    console.log(this.duplicatedClient);

    if (!this.duplicatedClient)
    this.getClient(); //Her hentes client listen til den valgte bruger

  }



/*********************graf-service*********************************/



getGraf() {



   this.client=this.clients.find(clientSearch => this.selectedClient ===  clientSearch.client_name);
   // @ts-ignore
  this.minGrafValue=this.client?.min_value-1;
  // @ts-ignore
  this.maxGrafValue=this.client?.max_value+1;







  var object = {
    eventType: "getData",
    clientID: this.client?.client_id,

  }
  this.ws.send(JSON.stringify(object));
}






  PhData(dto: PhDataDto) {



    this.maxValue = dto.series!.reduce((max, current) => {
      return current.value > max ? current.value : max;
    }, dto.series![0].value);



    this.minValue = dto.series!.reduce((min, current) => {
      return current.value < min ? current.value : min;
    }, dto.series![0].value);




    const measured: PHModel = {
      name: this.selectedClient,
      series: dto.series!
    };

    this.pHData.push(measured);



    // @ts-ignore
    const maxliste=this.newSerie(dto.series!,"MaxValue", this.client!.max_value);


    const maxPh: PHModel = {
      name: "Max value",
      series: maxliste
    };

    this.pHData.push(maxPh);

    // @ts-ignore
    const minliste=this.newSerie(dto.series!,"MaxValue", this.client!.min_value);

    // @ts-ignore
    const minPh: PHModel = {
      name: "Min value",
      series: minliste
    };

    this.pHData.push(minPh);


    this.pHData = [... this.pHData];




  }


  newSerie(seri: series[], name: string, value: number): series[] {

      let serie: series[]=[];


      for (let i = 0; i < seri.length; i++) {
       const ser=
         {
           name:seri[i].name,
           value: value
         };
          serie.push(ser);
        }
    return serie;
    }







}
