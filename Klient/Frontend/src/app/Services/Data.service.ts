import { Injectable } from '@angular/core';

import {temperaturModel} from "./tempModel";
import {BaseDto, sendAddressesDto, ServerSendsIOTDataToClientsDto} from "../BaseDto";
import {Address} from "../Models/LookupModels";
import {LoginModel, UserModel} from "../Models/userModel";
import {ClientModel} from "../Models/clientModel";


@Injectable({
  providedIn: 'root'
})
export class DataService {

  secCounter:number=0;
  temperatureData: temperaturModel[]=[];
  addressSuggestions: Address[] = [];
  start: boolean=false;

  ws: WebSocket = new WebSocket("ws://localhost:8181")
  graphName: string="";

  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
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
      username: loginModel.username,
      password: loginModel.password

    }
    this.ws.send(JSON.stringify(object));
  }

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

}

