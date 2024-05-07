import {Address, AddressAPIJsonResponseModel} from "../Models/LookupModels";


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



export class responseStringDto extends BaseDto<responseStringDto> {
  response?: string;
}




export class SendLoginInfoDto extends BaseDto<SendLoginInfoDto> {
  email?: string;
}



export class userModelDto extends BaseDto<userModelDto> {
  email?: string;
  name?: string;
  address?: string;
  street_number?: string;
  zip_code?: number;
  cvr?: number;
}
