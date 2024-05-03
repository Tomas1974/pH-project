import {AddressAPIJsonResponseModel} from "../../../Models/LookupModels";


export class HomeBaseDto<T> {
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}




export class sendAddressesDto extends HomeBaseDto<sendAddressesDto> {
  results?: AddressAPIJsonResponseModel;
}



export class responseStringDto extends HomeBaseDto<responseStringDto> {
  response?: string;
}
