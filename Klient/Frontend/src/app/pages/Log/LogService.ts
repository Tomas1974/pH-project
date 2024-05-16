import {Observable, Subject} from "rxjs";
import {WebSocketSubject} from "rxjs/internal/observable/dom/WebSocketSubject";
import {webSocket} from "rxjs/webSocket";
import {StatusModel} from "../../Models/SatusModel";
import {BaseDto} from "../../Services/BaseDto";



export class WebSocketService {

  statusUpdates : StatusModel[] = [];
  ws: WebSocket = new WebSocket("ws://localhost:8181")

  constructor() {}
}
