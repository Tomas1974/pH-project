import {Injectable} from '@angular/core';


import {
  BaseDto, postNrDto, responseClient,
  responseStringDto,
  sendAddressesDto,
  SendLoginInfoDto,
  ServerSendsIOTDataToClientsDto, userClientDto, userModelDto
} from "./BaseDto";
import {Address, AddressAPIJsonResponseModel} from "../Models/LookupModels";
import {LoginModel, UserModel} from "../Models/userModel";
import {ClientModel} from "../Models/clientModel";
import {PHModel} from "../Models/pHModel";


@Injectable({
  providedIn: 'root'
})
export class DataService {

  secCounter: number = 0;
  pHData: PHModel[] = [];


  loginUser: string | undefined = ""; //Her er brugeren der er logget ind gemt

  /***********Home service********************/
  addressSuggestions: Address[] = [];
  loginResponse: string | undefined = "";
  requestLoginUser: string = "";
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


  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);

    }
  }


  getClientList() {
    //this.clientsNames=[];

    for (let i = 0; i < this.clients.length; i++) {
      // @ts-ignore
      this.clientsNames.push(this.clients[i].client_name);
      console.log(this.clients[i].client_name);
    }


  }


  ServerSendsIOTDataToClients(dto: ServerSendsIOTDataToClientsDto) {
    if (dto.data != null) {

      this.secCounter++;
      const existingSeries = this.pHData.find(series => series.name === this.graphName);

      if (existingSeries) {
        existingSeries.series.push({
          name: this.secCounter.toString(),
          value: parseFloat(dto.data)
        });
      } else {
        const newSeries = {
          name: this.graphName,
          series: [{name: this.secCounter.toString(), value: parseFloat(dto.data)}]
        };
        this.pHData.push(newSeries); // Push the new series to temperatureData
      }

      // Ensure this line is inside the if/else block to apply changes
      this.pHData = [...this.pHData];

    }

  }


  timePromise(): Promise<boolean> {


    return new Promise<boolean>((resolve, reject) => {

      const oldTimeStamp = new Date().getTime(); //Her sættes første tidsstempel

      const intervalId = setInterval(() => {


        if (this.timeStamp !== undefined && oldTimeStamp !== undefined) {
          if (this.timeStamp >= oldTimeStamp) {
            resolve(true);
          }

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


  saveOrEditUser(userModel: UserModel, type: string) {

    this.requestLoginUser != userModel.email;

    var object = {
      eventType: "saveUser",

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


  async responseString(dto: responseStringDto): Promise<void> {

    this.loginResponse = dto.response;

    if (dto.response == "Success") {
      this.loginUser = this.requestLoginUser;

      this.chooseComponent = 2;//Da denne infor bruges i andre pages sendes info til fælles dataservice
    }


    this.getClient(); //Her hentes client listen til den valgte bruger

    await this.timePromise();

    this.getClientList(); //Her laves listen over navnene til clienterne


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


  LoginUser(loginModel: LoginModel) {
    this.requestLoginUser = loginModel.email;

    var object = {
      eventType: "loginUser",
      email: loginModel.email,
      password: loginModel.password

    }
    this.ws.send(JSON.stringify(object));
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

    console.log("Check");
    this.timeStamp = new Date().getTime();

  }


  responseClientDuplicate(dto: responseClient) {

    this.duplicatedClient = dto.duplicate!
    this.timeStamp = new Date().getTime();
  }
}


