import {Address} from "./LookupModels";
//
// export interface ClientJsonModel {
//   clients: ClientModel[]
// }


export interface ClientModel {
  client_id?: string;
  client_name?: string;
  max_value?: number;
  min_value?: number;
}
