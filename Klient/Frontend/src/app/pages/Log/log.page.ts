import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from "rxjs";
import {WebSocketService} from "./LogService";
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-Log',
  templateUrl: './log.page.html',
  styleUrls: ['./log.page.scss'],
})

export class LogPage implements OnInit, OnDestroy {

  statusEntries: any[] = [];

  constructor(private dataService : DataService) { }

  ngOnInit(): void {

    this.statusEntries = this.dataService.getStatusArray();
    this.dataService.getStatusFromServer();

  }

  ngOnDestroy(): void {

  }
}
