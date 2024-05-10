import { Injectable } from '@angular/core';

import {temperaturModel} from "./tempModel";

import {ClientModel} from "../Models/clientModel";
import {
  BaseDto, postNrDto,
  responseStringDto,
  sendAddressesDto,
  SendLoginInfoDto,
  ServerSendsIOTDataToClientsDto, userModelDto
} from "./BaseDto";
import {Address, AddressAPIJsonResponseModel} from "../Models/LookupModels";
import {LoginModel, UserModel} from "../Models/userModel";



@Injectable({
  providedIn: 'root'
})
export class DataService {

  secCounter:number=0;
  temperatureData: temperaturModel[]=[];
  start: boolean=false;
  loginUser: string | undefined="";

  /***********Home service********************/
  addressSuggestions: Address[] = [];
  loginResponse: string | undefined="";
  requestLoginUser: string="";
  user: UserModel | undefined; //Her gemmes bruger information
  chooseComponent: number=0;
  oldTimeStamp: number | undefined;
  timeStamp: number | undefined;
  town: string | undefined="";

  ws: WebSocket = new WebSocket("ws://localhost:8181")

  graphName: string="";

  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);

      //this.oldTimeStamp=new Date().getTime();
      //this.timeStamp=this.oldTimeStamp;




    }
  }


  ServerSendsIOTDataToClients(dto: ServerSendsIOTDataToClientsDto) {
    if (dto.data != null) {

      this.secCounter++;
      const existingSeries = this.temperatureData.find(series => series.name === this.graphName);

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
        this.temperatureData.push(newSeries); // Push the new series to temperatureData
      }

      // Ensure this line is inside the if/else block to apply changes
      this.temperatureData = [...this.temperatureData];

    }

  }

  startStop()
  {
    if (this.start)
      this.start=false;
    else
      this.start=true;

    var object = {
      eventType: "StartStop",
      start_stop: "Start"

    }
    this.ws.send(JSON.stringify(object));
  }


  nulstil()
    {

      this.temperatureData=[];

    }

/*********************home-service*********************************/



  sendAddressLine(addressSearchTerm: string)
  {

    var object = {
      eventType: "getAddresses",
      addressSearchTerm: addressSearchTerm

    }



    this.ws.send(JSON.stringify(object));


  }





  sendAddresses(dto: sendAddressesDto): void

  {

    const addressSuggestions=dto.results!
    this.addressSuggestions=addressSuggestions.results;

  }

  timePromise(): Promise<boolean> {



    return new Promise<boolean>((resolve, reject) => {


      const intervalId = setInterval(() => {


        if (this.timeStamp !== undefined && this.oldTimeStamp !== undefined) {
          if (this.timeStamp >= this.oldTimeStamp) {
            resolve(true);
          }

        }
      }, 100);
    });
  }







  responseString(dto: responseStringDto): void

  {

    this.loginResponse=dto.response;

    if (dto.response=="Success")
    {
      this.loginUser=this.requestLoginUser;

     this.chooseComponent=2;//Da denne infor bruges i andre pages sendes info til fælles dataservice
    }

  }



  userModel(dto:userModelDto)
  {

    this.user=
    {
      address: dto.address,
      street_number: dto.street_number,
      cvr: dto.cvr,
      email: dto.email,
      name: dto.name,
      password: "",
      zip_code: dto.zip_code
    }

    this.chooseComponent=3; //Her vælges new User. Den vælges her, da så ved man at user er opdateret.


  }




  SendLoginInfo(dto:SendLoginInfoDto)
  {
    this.loginUser=dto.email;
    this.timeStamp=new Date().getTime();


  }


  getPostNr(postNr: number)
  {

    var object = {
      eventType: "PostNr",

      postNr: postNr

    }
    this.ws.send(JSON.stringify(object));

  }

  async SendTown(dto: postNrDto)
  {
    this.town=dto.town;


    this.timeStamp=new Date().getTime();

  }




  saveOrEditUser(userModel: UserModel, type: string)
  {
    //this.newOrEditUser=type; //Her gemmes om det er update eller newUser
    this.requestLoginUser!=userModel.email;

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

  getUserInfo()

  {
    var object = {
      eventType: "getUserInfo",

      email: this.loginUser

    }
    this.ws.send(JSON.stringify(object));
  }


  LoginUser(loginModel: LoginModel)
  {
    this.requestLoginUser=loginModel.email;

    var object = {
      eventType: "loginUser",
      email: loginModel.email,
      password: loginModel.password

    }
    this.ws.send(JSON.stringify(object));
  }


  UserActions(info: string) {


    var object = {
      eventType: "UserActions",
      getLoginInfo: info

    }
    this.ws.send(JSON.stringify(object));


  }

/**************************************client-service*******************************/

saveClient(clientModel: ClientModel)
{
  var object = {
    eventType: "saveClient",
    client_id: clientModel.client_id,
    client_name: clientModel.client_name,
    max_value: clientModel.max_value,
    min_value: clientModel.min_value
  }
  this.ws.send(JSON.stringify(object));
}

getClient(email: string)
{
  var object = {
    eventType: "getClient",
    email: email
  }
  this.ws.send(JSON.stringify(object));
}

}

