import { AddressAPIJsonResponseModel} from "../Models/LookupModels";
import {ClientModel} from "../Models/clientModel";
import { series} from "../Models/pHModel";




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

export class userClientDto extends BaseDto<userClientDto>
{
  clients?: ClientModel[];
}

export class responseClient extends BaseDto<responseClient>
{
  duplicate?: boolean;
}

export class postNrDto extends BaseDto<postNrDto>
{
  town?: string;
}



export class PhDataDto extends BaseDto<PhDataDto>
{
  series?: series[];
}
