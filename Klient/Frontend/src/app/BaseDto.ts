import {Address, AddressAPIJsonResponseModel} from "./home/Login/models";

export class BaseDto<T> {
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}



export class ServerSendsIOTDataToClientsDto extends BaseDto<ServerSendsIOTDataToClientsDto> {
  data?: string;
}





export class sendAddressesDto extends BaseDto<sendAddressesDto> {
  results?: AddressAPIJsonResponseModel;
}
