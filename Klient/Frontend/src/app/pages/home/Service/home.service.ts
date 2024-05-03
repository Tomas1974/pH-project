import { Injectable } from '@angular/core';
import {Address} from "../../../Models/LookupModels";

import {LoginModel, UserModel} from "../../../Models/userModel";
import {ClientModel} from "../../../Models/clientModel";
import {HomeBaseDto, responseStringDto, sendAddressesDto} from "./HomeBaseDto";


@Injectable({
  providedIn: 'root'
})
export class HomeService {

  secCounter:number=0;
  addressSuggestions: Address[] = [];
    ws: WebSocket = new WebSocket("ws://localhost:8181")
  loginResponse: string | undefined="";


  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as HomeBaseDto<any>;
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
    console.log("Check"+dto.response);

  }





  saveUser(userModel: UserModel)
  {

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

    var object = {
      eventType: "loginUser",
      email: loginModel.email,
      password: loginModel.password

    }
    this.ws.send(JSON.stringify(object));
  }








}

