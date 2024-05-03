import { Injectable } from '@angular/core';
import {Address} from "../Models/LookupModels";

import {LoginModel, UserModel} from "../Models/userModel";
import {ClientModel} from "../Models/clientModel";
import {DataService} from "./Data.service";
import {BaseDto, responseStringDto, sendAddressesDto} from "./BaseDto";


@Injectable({
  providedIn: 'root'
})
export class HomeService {


  addressSuggestions: Address[] = [];
    ws: WebSocket = this.dataservice.ws;
  loginResponse: string | undefined="";
    requestLoginUser: string="";

  constructor(public dataservice:DataService) {

    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
    }
  }



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




  responseString(dto: responseStringDto): void

  {

    this.loginResponse=dto.response;

    if (dto.response=="Success")
    {
      this.dataservice.loginUser=this.requestLoginUser; //Da denne infor bruges i andre pages sendes info til fælles dataservice
    }

  }





  saveUser(userModel: UserModel)
  {

    this.requestLoginUser=userModel.email;

    var object = {
      eventType: "saveUser",
      email: userModel.email,
      name: userModel.name,
      password: userModel.password,
      address: userModel.address,
      zipcode: userModel.zip_code,
      cvr: userModel.cvr
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


  checkIfAnyoneHasLoggedIn() {


    var object = {
      eventType: "WhoHasLoggedIn",
      LoggedInInfo: "LogInInfo"

    }
    this.ws.send(JSON.stringify(object));

  }
}

